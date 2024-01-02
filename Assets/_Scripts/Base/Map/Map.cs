using UnityEngine;

public class Map
{
    public readonly int MapRows;
    public readonly int MapColumns;
    private readonly int[,] Tiles;

    //  ^ y
    //  |
    //  |      x
    //  +------>

    public Map(int[,] tiles)
    {
        MapRows = tiles.GetLength(0);
        MapColumns = tiles.Length / MapRows;
        Tiles = tiles;
    }

    public TileType GetTile(Vector2Int pos)
    {
        var newRow = MapRows - 1 - pos.y; //разворачиваем ось y
        var tile = new Vector2Int(pos.x, newRow);
        return (TileType)Tiles[tile.y, tile.x];
    }

    /// <summary>
    /// Проверка на то что ячейка находится в границах игрового поля
    /// </summary>
    public bool InMapBounds(Vector2Int cell)
    {
        //(x,y) = (column, row)
        if (cell.y >= MapRows || cell.y < 0 ||
            cell.x >= MapColumns || cell.x < 0)
            return false;
        return true;
    }

    public bool IsHole(Vector2Int cell)
    {
        return GetTile(cell) == TileType.Hole;
    }

    public bool IsWall(Vector2Int cell)
    {
        return GetTile(cell) == TileType.Wall;
    }

    public bool IsFinish(Vector2Int cell)
    {
        return GetTile(cell) == TileType.Finish;
    }

    public bool IsGround(Vector2Int cell)
    {
        return GetTile(cell) == TileType.Ground;
    }

    public override string ToString()
    {
        var result = "";
        for (var row = 0; row < MapRows; row++)
            for (var column = 0; column < MapColumns; column++)
            {
                if (column == MapColumns - 1)
                    result += $"{Tiles[row, column]}\n";
                else if (row == 8 && column == 4)
                    result += "O ";
                else
                    result += $"{Tiles[row, column]} ";
            }
        return result;
    }
}
