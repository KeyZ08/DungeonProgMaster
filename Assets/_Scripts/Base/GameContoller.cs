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

    private UnitControllerInstaller unitInstaller;

    private void Start() 
    {
        unitInstaller = FindAnyObjectByType<UnitControllerInstaller>();

        level = LevelsHandler.Levels[1];
        units = new List<UnitController>();
        LevelConstruct(level);

        playBtn.onClick.AddListener(PlayBtnClick);
    }

    public void LevelConstruct(Level level)
    {
        mapV.DrawMap(level.Map);
        LevelUnitsCreate();

        var characterPos = level.Character.StartPosition;
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
            var obj = unitInstaller.Instantiate(unit, mapV.GetCellCenter(unit.Position), Quaternion.identity, spawner);
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

            Come(units.Find(x => x.Position == character.CurrentPosition));

            //если следующее дествие - forward, то проверяем, что мы можем его выполнить
            //(никуда не выпали и не уперлись)
            if (index + 1 < playerSteps.Count && playerSteps[index + 1] == "forward")
            {
                if (!level.Map.IsGround(character.CurrentPosition))
                    break;
                if (!IsNextMoveFree(character.CurrentPosition, character.CurrentDirection))
                    break;
            }
        }

        var characterInMap = character.CurrentPosition;
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
            var isNextMoveFree = nextStep == "forward" && IsNextMoveFree(character.CurrentPosition, character.CurrentDirection);
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
            var forwardInMap = character.CurrentPosition + character.Forward;
            var forwardUnit = units.Find(x => x.Position == forwardInMap);
            Attack(forwardUnit, nextStep == "attack");
        }
        else
            throw new NotImplementedException(step);
    }

    public void Attack(UnitController unit, bool nextAlsoAttack)
    {
        characterV.Attack(
            () => { 
                if(unit != null && unit is IAttackeble attackeble)
                    attackeble.OnAttack(ContactDirection.Side, this); 
            }, nextAlsoAttack);
    }

    public void Come(UnitController unit)
    {
        if (unit != null && unit is IOnCome onComeable)
            onComeable.OnCome(ContactDirection.Directly, this);
    }

    public void Take(UnitController unit)
    {
        if (unit != null && unit is ITakeable takeable)
            takeable.OnTake(ContactDirection.Directly, this);
    }

    /// <summary>
    /// true - свободно, false - преграда.
    /// Преградой является то на что нельзя встать, пропасть - не преграда
    /// </summary>
    private bool IsNextMoveFree(Vector2Int position, Direction direction)
    {
        var posInMap = position + direction.Vector();
        if (!level.Map.InMapBounds(posInMap))
            return false;

        if (level.Map.IsWall(posInMap))
            return false;

        var unit = units.Find(x => x.Position == posInMap);
        if (unit != null && unit.Type == Tangibility.Obstacle)
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
