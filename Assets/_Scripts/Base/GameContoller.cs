using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using EasyButtons;

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

    private Map map;
    private Character character;
    private List<UnitController> units;

    private CharacterVisualizer characterV;
    private List<string> playerSteps;
    private int actualLevel = 1;

    private UnitControllerInstaller unitInstaller;
    private LevelsHandlerScriptableObject levels;

    public int coins;
    private bool _isPlayed;

    private bool IsPlayed
    {
        get => _isPlayed;
        set
        {
            _isPlayed = value;
            if (_isPlayed)
            {
                playBtn.interactable = false;
            }
            else
            {
                playBtn.interactable = true;
            }
        }
    }

    private void Start() 
    {
        unitInstaller = FindAnyObjectByType<UnitControllerInstaller>();
        levels = Resources.Load<LevelsHandlerScriptableObject>("LevelsHandler");

        var level = levels.GetLevel(actualLevel);
        LevelConstruct(level);

        playBtn.onClick.AddListener(PlayBtnClick);
    }

    [Button]
    public void Test()
    {
        var l = levels.GetLevel(0);
        Debug.Log(l);
    }

    public void LevelConstruct(Level level)
    {
        map = level.Map;
        mapV.DrawMap(map);
        LevelUnitsCreate(level.Units);

        character = level.Character;
        var cellPos = mapV.GetCellCenter(character.CurrentPosition);
        characterV = Instantiate(CharacterPrefab, cellPos, Quaternion.identity, spawner);
        characterV.Constructor(cellPos, character.StartDirection);
    }

    private void LevelUnitsCreate(List<Unit> units)
    {
        this.units = new List<UnitController>();
        for (int i = 0; i < units.Count; i++)
        {
            var unit = units[i]; 
            var obj = unitInstaller.Instantiate(unit, mapV.GetCellCenter(unit.Position), Quaternion.identity, spawner);
            this.units.Add(obj);
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
        coins = 0;
        LevelConstruct(levels.GetLevel(actualLevel));
    }

    private void PlayBtnClick()
    {
        IsPlayed = true;
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
                if (!map.IsGround(character.CurrentPosition))
                    break;
                if (!IsNextMoveFree(character.CurrentPosition, character.CurrentDirection))
                    break;
            }
        }

        var characterInMap = character.CurrentPosition;
        if (map.IsFinish(characterInMap))
            Win();
        else
            Lose();
        IsPlayed = false;
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
        if (!map.InMapBounds(posInMap))
            return false;

        if (map.IsWall(posInMap))
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
