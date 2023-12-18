using UnityEngine;

public class Map
{
    public readonly int MapRows;
    public readonly int MapColumns;
    private readonly int[,] Tiles;

    public Map(int[,] tiles)
    {
        MapRows = tiles.GetLength(0);
        MapColumns = tiles.Length / MapRows;
        Tiles = tiles;
    }

    public TileType GetTile(int row, int column)
    {
        return (TileType)Tiles[row, column];
    }

    public TileType GetTile(Vector2Int pos)
    {
        return (TileType)Tiles[pos.x, pos.y];
    }

    /// <summary>
    /// Проверка на то что ячейка находится в границах игрового поля
    /// </summary>
    public bool InMapBounds(Vector2Int cell)
    {
        //(x,y) = (row, column)
        if (cell.y >= MapColumns || cell.y < 0 ||
            cell.x >= MapRows || cell.x < 0)
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
