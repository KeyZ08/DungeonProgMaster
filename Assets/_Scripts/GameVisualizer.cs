using UnityEngine;
using UnityEngine.Tilemaps;

public class GameVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [Header("Tiles")]
    [SerializeField] private RuleTile ground;

    public Vector2 GetCellCenter(Vector2Int cell)
    {
        return tilemap.GetCellCenterWorld((Vector3Int)cell);
    }

    public Vector2 GetCellSize()
    {
        return tilemap.cellSize;
    }

    /// <summary>
    /// Находит ячейку по мировым координатам
    /// </summary>
    public Vector2Int GetCellByWorlPoint(Vector3 worldPoint)
    {
        return (Vector2Int)tilemap.WorldToCell(worldPoint);
    }
}
