using System;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameContoller : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private PlayerVisualizer playerV;
    [SerializeField] private MapVisualizer mapV;
    [SerializeField] private CompileController compiler;

    [Header("Input Field")]
    [SerializeField] private TMP_InputField inputField;

    [Header("Buttons")]
    [SerializeField] private Button playBtn;

    private Size mapSize = new Size(12, 12);
    private Level level;

    private void Start()
    {
        level = LevelsHandler.Levels[0];
        mapV.DrawMap(level.Map);
        playerV.Constructor(mapV.GetCellCenter(Vector2Int.one * 5));

        playBtn.onClick.AddListener(PlayBtnClick);
    }

    private void Update()
    {
        var cell = mapV.GetCellByWorlPoint(playerV.Position);
        if(!InMapBounds(cell))
            throw new ArgumentOutOfRangeException("Вне игрового поля!");

        PlayerMove(cell);
    }

    private void PlayBtnClick()
    {
        var steps = compiler.Compile(inputField.text);
        foreach(var step in steps)
        {
            Debug.Log(step);
        }
    }

    private void PlayerMove(Vector2Int startCell)
    {
        if (playerV.IsMovement) return;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput != 0)
        {
            var vector = verticalInput > 0 ? Vector2Int.up : Vector2Int.down;
            playerV.MoveTo(mapV.GetCellCenter(startCell + vector));
        }
        if (horizontalInput != 0)
        {
            var vector = horizontalInput > 0 ? Vector2Int.right : Vector2Int.left;
            playerV.MoveTo(mapV.GetCellCenter(startCell + vector));
        }
    }


    /// <summary>
    /// Проверка на то что ячейка находится в границах игрового поля
    /// </summary>
    private bool InMapBounds(Vector2Int cell)
    {
        if (cell.y >= mapSize.Height || cell.y < 0 ||
            cell.x >= mapSize.Width || cell.x < 0)
            return false;
        return true;
    }
}
