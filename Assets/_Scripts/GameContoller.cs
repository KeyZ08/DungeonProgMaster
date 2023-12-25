using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameContoller : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private Transform spawner;
    [SerializeField] private CharacterVisualizer CharacterPrefab;

    [Header("Prefabs")]
    [SerializeField] private CoinController CoinPrefab;
    [SerializeField] private SkeletonController SkeletonPrefab;

    [Header("Controllers")]
    [SerializeField] private MapVisualizer mapV;
    [SerializeField] private CompileController compiler;

    [Header("Input Field")]
    [SerializeField] private TMP_InputField inputField;

    [Header("UI")]
    [SerializeField] private Transform WinCanvas;
    [SerializeField] private Transform LoseCanvas;

    [Header("Buttons")]
    [SerializeField] private Button playBtn;

    private Level level;
    private Character character;
    private List<UnitController> units;
    private CharacterVisualizer characterV;

    private List<string> playerSteps;

    public int coins;


    private void Start() 
    {
        level = LevelsHandler.Levels[1];
        units = new List<UnitController>();
        LevelConstruct(level);

        playBtn.onClick.AddListener(PlayBtnClick);
    }

    public void LevelConstruct(Level level)
    {
        mapV.DrawMap(level.Map);
        LevelUnitsCreate();

        var characterPos = (Vector2Int)mapV.FromMapToVisual(level.Character.StartPosition, level.Map);
        //координаты map и координаты тайлов расходятся, потому задаем character-у нужную нам позицию
        character = new Character(characterPos, level.Character.StartDirection);
        var cellPos = mapV.GetCellCenter(characterPos);
        characterV = Instantiate(CharacterPrefab, cellPos, Quaternion.identity, spawner);
        characterV.Constructor(cellPos, character.StartDirection);
    }

    private void LevelUnitsCreate()
    {
        for (int i = 0; i < level.Units.Count; i++)
        {
            var unit = level.Units[i].GetCopy(); 
            var pos = (Vector2Int)mapV.FromMapToVisual(unit.Position, level.Map);
            var spawnPos = mapV.GetCellCenter(pos);

            UnitController obj;
            if(unit is Coin)
                obj = Instantiate(CoinPrefab, spawnPos, Quaternion.identity, spawner);
            else if(unit is Skeleton)
                obj = Instantiate(SkeletonPrefab, spawnPos, Quaternion.identity, spawner);
            else
                throw new NotImplementedException();

            obj.Construct(unit, this);
            units.Add(obj);
        }
    }

    public void LevelDelete()
    {
        Destroy(characterV.gameObject);
        characterV = null;
        character = null;
        for(var i = units.Count - 1; i >= 0; i--)
        {
            Destroy(units[i].gameObject);
            units.RemoveAt(i);
        }
    }

    public void LevelReset()
    {
        WinCanvas.gameObject.SetActive(false);
        LoseCanvas.gameObject.SetActive(false);
        LevelDelete();
        LevelConstruct(level);
    }

    private void PlayBtnClick()
    {
        playerSteps = compiler.Compile(inputField.text);
        GameStart();
    }

    private void GameStart()
    {
        StartCoroutine(CharacterWorkCoroutine());
    }

    private IEnumerator CharacterWorkCoroutine()
    {
        for (var index = 0; index < playerSteps.Count; index++)
        {
            var step = playerSteps[index];
            CharacterWork(step, index == playerSteps.Count - 1 ? null : playerSteps[index + 1]);

            while (characterV.IsAnimated)
                yield return null;

            var chInMap = mapV.FromVisualToMap(character.CurrentPosition, level.Map);
            Come(units.Find(x => x.Position == chInMap));

            //если следующее дествие - forward, то проверяем, что мы можем его выполнить
            //(никуда не выпали и не уперлись)
            if (index + 1 < playerSteps.Count && playerSteps[index + 1] == "forward")
            {
                var forward = character.CurrentDirection.Vector();
                var forwardInMap = mapV.FromVisualToMap(character.CurrentPosition + forward, level.Map);
                if (!level.Map.IsGround(chInMap) || !IsFreeForMove(forwardInMap))
                    break;
                var forwardUnit = units.Find(x => x.Position == forwardInMap);
                if (forwardUnit != null && forwardUnit.Type == Tangibility.Obstacle)
                    break;
            }
        }

        var characterInMap = mapV.FromVisualToMap(character.CurrentPosition, level.Map);
        if (level.Map.IsFinish(characterInMap))
            Win();
        else
            Lose();
    }

    private void CharacterWork(string step, string nextStep = null)
    {
        if (step == "forward")
        {
            Command.MoveForward(character);
            var isNextMoveFree = nextStep == "forward";
            if (nextStep == "forward")
            {
                var chInMap = mapV.FromVisualToMap(character.CurrentPosition, level.Map);
                var forward = character.CurrentDirection.Vector();
                var forwardInMap = mapV.FromVisualToMap(character.CurrentPosition + forward, level.Map);
                if (!level.Map.IsGround(chInMap) || !IsFreeForMove(forwardInMap))
                    isNextMoveFree = false;
                var forwardUnit = units.Find(x => x.Position == forwardInMap);
                if (forwardUnit != null && forwardUnit.Type == Tangibility.Obstacle)
                    isNextMoveFree = false;
            }
            characterV.MoveTo(mapV.GetCellCenter(character.CurrentPosition), isNextMoveFree);
        }
        else if (step == "turn_right")
        {
            Command.RotateRight(character);
            characterV.TurnTo(character.CurrentDirection);
        }
        else if (step == "turn_left")
        {
            Command.RotateLeft(character);
            characterV.TurnTo(character.CurrentDirection);
        }
        else if (step == "attack")
        {
            var chInMap = mapV.FromVisualToMap(character.CurrentPosition, level.Map);
            var forward = character.CurrentDirection.Vector();
            var forwardInMap = mapV.FromVisualToMap(character.CurrentPosition + forward, level.Map);

            var forwardUnit = units.Find(x => x.Position == forwardInMap);
            Attack(forwardUnit, nextStep == "attack");
        }
        else
            throw new NotImplementedException(step);
    }

    public void Attack(IUnit unit, bool nextAlsoAttack)
    {
        characterV.Attack(() => 
            { 
                if(unit != null) 
                    unit.OnAttack(ContactDirection.Side, this); 
            }, nextAlsoAttack);
    }

    public void Come(IUnit unit)
    {
        if (unit == null) return;
        unit.OnCome(ContactDirection.Directly, this);
    }

    public void Take(IUnit unit)
    {
        if (unit == null) return;
        //unit.OnPositionalAction("take");
    }

    /// <summary>
    /// true - свободно, false - преграда.
    /// Преградой является то на что нельзя встать, пропасть - не преграда
    /// </summary>
    private bool IsFreeForMove(Vector2Int mapPos)
    {
        var inBounds = level.Map.InMapBounds(mapPos);
        if (!inBounds) 
            return false;

        if (level.Map.IsWall(mapPos)) 
            return false;

        return true;
    }

    public void Win()
    {
        WinCanvas.gameObject.SetActive(true);
    }

    public void Lose()
    {
        LoseCanvas.gameObject.SetActive(true);
    }

    public void OnUnitDestroy(UnitController unit)
    {
        units.Remove(unit);
    }
}
