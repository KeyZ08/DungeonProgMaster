using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private Transform spawner;

    [Header("Character")]
    [SerializeField] private MyCharacterController characterPrefab;

    [Header("Controllers")]
    [SerializeField] private UIController ui;
    [SerializeField] private MapVisualizer mapV;
    [SerializeField] private CompileController compiler;

    [Header("DI Container")]
    [SerializeField] private SceneContext container;
    private Map map;
    private MyCharacterController character;
    private List<BaseUnitController> units;
    private int actualLevel = 1;

    private UnitControllerInstaller unitInstaller;
    [Inject] private LevelsHandlerScriptableObject levels;

    public int coins;
    private bool _isPlayed;

    private bool IsPlayed
    {
        get => _isPlayed;
        set
        {
            _isPlayed = value;
            ui.SetInteractable(!_isPlayed);
        }
    }

    [Inject]
    private void Construct(MyCharacterController.Factory characterFactory, Level level)
    {
        unitInstaller = FindAnyObjectByType<UnitControllerInstaller>();
        LevelConstruct(level);

        character = characterFactory.Create(level.Character, level.Map, this);

        ui.OnPlayBtnClick.AddListener(PlayBtnClick);
        ui.OnResetBtnClick.AddListener(LevelReset);
    }


    //private void Start() 
    //{
    //    unitInstaller = FindAnyObjectByType<UnitControllerInstaller>();
    //    levels = Resources.Load<LevelsHandlerScriptableObject>("LevelsHandler");

    //    var level = levels.GetLevel(actualLevel);
    //    LevelConstruct(level);

    //    ui.OnPlayBtnClick.AddListener(PlayBtnClick);
    //    ui.OnResetBtnClick.AddListener(LevelReset);
    //}

    public void LevelConstruct(Level level)
    {
        map = level.Map;
        mapV.DrawMap(map);
        LevelUnitsCreate(level.Units);

        var cellPos = mapV.GetCellCenter(level.Character.CurrentPosition);
        //var test = container.Container.Instantiate<MyCharacterController.Factory>();
        //var testch = test.Create(level.Character, map, this);
        //character = Instantiate(characterPrefab, cellPos, Quaternion.identity, spawner);
        //character.Construct(level.Character, map, this);
    }

    private void LevelUnitsCreate(List<Unit> units)
    {
        this.units = new List<BaseUnitController>();
        for (int i = 0; i < units.Count; i++)
        {
            var unit = units[i];
            var obj = unitInstaller.Instantiate(unit, mapV.GetCellCenter(unit.Position), spawner);
            this.units.Add(obj);
        }
    }

    private void PlayBtnClick()
    {
        var playerSteps = compiler.Compile();
        IsPlayed = true;
        character.Play(playerSteps);
    }

    public void OnCharacterMoveEnd()
    {
        IsPlayed = false;
        if (map.IsFinish(character.Position))
            ui.WinShow(true);
        else
            ui.LoseShow(true);
    }

    public void OnUnitDestroy(BaseUnitController unit)
    {
        units.Remove(unit);
    }

    public void LevelDelete()
    {
        Destroy(character.gameObject);
        for (var i = units.Count - 1; i >= 0; i--)
        {
            Destroy(units[i].gameObject);
            units.RemoveAt(i);
        }
    }

    public void LevelReset()
    {
        ui.WinShow(false);
        ui.LoseShow(false);
        LevelDelete();
        coins = 0;
        LevelConstruct(levels.GetLevel(actualLevel));
    }

    public BaseUnitController GetUnitController(Vector2Int target)
    {
        return units.Find(x => x.Position == target);
    }

    public Vector2 GetPosByMap(Vector2Int position)
    {
        return mapV.GetCellCenter(position);
    }
}
