using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private readonly int MapRows;
    private readonly int MapColumns;
    private readonly int[,] Tiles;

    public Map(int[,] tiles)
    {
        MapColumns = tiles.GetLength(0);
        MapRows = tiles.Length / MapColumns;
        Tiles = tiles;
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
