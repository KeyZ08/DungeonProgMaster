using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameContoller : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private Transform spawner;

    [Header("Character")]
    [SerializeField] private CharacterController characterPrefab;

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
    private CharacterController character;
    private List<UnitController> units;
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

    public void LevelConstruct(Level level)
    {
        map = level.Map;
        mapV.DrawMap(map);
        LevelUnitsCreate(level.Units);

        var cellPos = mapV.GetCellCenter(level.Character.CurrentPosition);
        character = Instantiate(characterPrefab, cellPos, Quaternion.identity, spawner);
        character.Construct(level.Character, mapV, map, this);
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
        Destroy(character.gameObject);
        for (var i = units.Count - 1; i >= 0; i--)
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
        var playerSteps = compiler.Compile(inputField.text);
        IsPlayed = true;
        character.Play(playerSteps);
    }

    /// <summary>
    /// true - свободно, false - преграда.
    /// Преградой является то на что нельзя встать, пропасть - не преграда
    /// </summary>
    public bool IsNextMoveFree(Vector2Int position, Direction direction)
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

    public void OnCharacterMoveEnd()
    {
        IsPlayed = false; 
        var characterInMap = character.Position;
        if (map.IsFinish(characterInMap))
            Win();
        else
            Lose();
    }

    public UnitController GetUnitController(Vector2Int target)
    {
        return units.Find(x => x.Position == target);
    }
}
