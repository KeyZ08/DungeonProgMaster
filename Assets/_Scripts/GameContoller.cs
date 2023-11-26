using System;
using System.Drawing;
using UnityEngine;

public class GameContoller : MonoBehaviour
{
    [SerializeField] private PlayerVisualizer playerV;
    [SerializeField] private GameVisualizer gameV;

    private Size mapSize = new Size(12, 12);

    private void Start()
    {
        playerV.Constructor(gameV.GetCellCenter(Vector2Int.one * 5));
    }

    private void Update()
    {
        var cell = gameV.GetCellByWorlPoint(playerV.Position);
        if(!InMapBounds(cell))
            throw new ArgumentOutOfRangeException("Вне игрового поля!");

        PlayerMove(cell);
    }

    private void PlayerMove(Vector2Int startCell)
    {
        if (playerV.IsMovement) return;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput != 0)
        {
            var vector = verticalInput > 0 ? Vector2Int.up : Vector2Int.down;
            playerV.MoveTo(gameV.GetCellCenter(startCell + vector));
        }
        if (horizontalInput != 0)
        {
            var vector = horizontalInput > 0 ? Vector2Int.right : Vector2Int.left;
            playerV.MoveTo(gameV.GetCellCenter(startCell + vector));
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
