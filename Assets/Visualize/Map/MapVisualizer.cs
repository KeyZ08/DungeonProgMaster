using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class MapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [Header("Tiles")]
    [SerializeField] private RuleTile ground;
    [SerializeField] private Tile hole;
    [SerializeField] private Tile wall;
    [SerializeField] private Tile finish;

    private Dictionary<TileType, TileBase> tileByType;

    public void Construct()
    {
        tileByType = new Dictionary<TileType, TileBase>()
        {
            { TileType.Ground, ground },
            { TileType.Hole, hole },
            { TileType.Wall, wall },
            { TileType.Finish, finish },
        };
    }

    public Vector3Int FromMapToVisual(Vector2Int pos, Map map)
    {
        //(x,y) = (row, column) in map
        //строки в map идут сверху вниз а рисуем их мы снизу вверх 
        return new Vector3Int(pos.y, map.MapRows - 1 - pos.x);
    }

    public Vector2Int FromVisualToMap(Vector2Int pos, Map map)
    {
        return new Vector2Int(map.MapRows - 1 - pos.y, pos.x);
    }

    public void DrawMap(Map map)
    {
        tilemap.ClearAllTiles();
        if (tileByType == null) Construct();
        for (int row = 0; row < map.MapRows; row++)
        {
            for (int column = 0; column < map.MapColumns; column++)
            {
                var tile = map.GetTile(row, column);
                tilemap.SetTile(FromMapToVisual(new Vector2Int(row, column), map), tileByType[tile]);
            }
        }
    }

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
