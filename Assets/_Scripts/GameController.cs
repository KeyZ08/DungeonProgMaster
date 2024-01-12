using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private Transform spawner;

    [Inject] private UIController ui;
    [Inject] private MapVisualizer mapV;
    [Inject] private CompileController compiler;

    [Inject] private IUnitControllerFactory unitFactory;
    [Inject] private MyCharacterController.Factory characterFactory;
    [Inject] private LevelsHandlerScriptableObject levels;

    private Map map;
    private MyCharacterController character;
    private List<BaseUnitController> units;
    private int actualLevel = 1;

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

    private void Start()
    {
        var level = levels.GetLevel(actualLevel);
        LevelConstruct(level);

        ui.ConstructLevelBtns(levels.LevelsCount - 1, this); // ��������� ������� - �������� - ��� �� ��������
        ui.OnPlayBtnClick.AddListener(PlayBtnClick);
        ui.OnResetBtnClick.AddListener(LevelReset);
    }

    public void LevelConstruct(Level level)
    {
        map = level.Map;
        mapV.DrawMap(map);
        LevelUnitsCreate(level.Units);

        var cellPos = mapV.GetCellCenter(level.Character.CurrentPosition);
        var trp = new TransformParameters(spawner, cellPos, Quaternion.identity);
        character = characterFactory.Create(level.Character, level.Map, trp);
    }

    public void LoadLevel(int level)
    {
        actualLevel = level;
        LevelReset();
    }

    private void LevelUnitsCreate(List<Unit> units)
    {
        this.units = new List<BaseUnitController>();
        for (int i = 0; i < units.Count; i++)
        {
            var unit = units[i];
            var trp = new TransformParameters(spawner, mapV.GetCellCenter(unit.Position));
            var obj = unitFactory.Create(unit, trp);
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
        if (map.IsFinish(character.Character.CurrentPosition))
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
