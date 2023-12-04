using System;
using System.Drawing;
using UnityEngine;

public class GameContoller : MonoBehaviour
{
    [SerializeField] private PlayerVisualizer playerV;
    [SerializeField] private MapVisualizer mapV;

    private Size mapSize = new Size(12, 12);
    private Level level;

    private void Start()
    {
        level = LevelsHandler.Levels[0];
        mapV.DrawMap(level.Map);
        playerV.Constructor(mapV.GetCellCenter(Vector2Int.one * 5));
    }

    private void Update()
    {
        var cell = mapV.GetCellByWorlPoint(playerV.Position);
        if(!InMapBounds(cell))
            throw new ArgumentOutOfRangeException("��� �������� ����!");

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
            playerV.MoveTo(mapV.GetCellCenter(startCell + vector));
        }
        if (horizontalInput != 0)
        {
            var vector = horizontalInput > 0 ? Vector2Int.right : Vector2Int.left;
            playerV.MoveTo(mapV.GetCellCenter(startCell + vector));
        }
    }


    /// <summary>
    /// �������� �� �� ��� ������ ��������� � �������� �������� ����
    /// </summary>
    private bool InMapBounds(Vector2Int cell)
    {
        if (cell.y >= mapSize.Height || cell.y < 0 ||
            cell.x >= mapSize.Width || cell.x < 0)
            return false;
        return true;
    }
}
