using System.Collections.Generic;
using UnityEngine;

public static class LevelsHandler
{
    public static readonly List<Level> Levels;
    
    static LevelsHandler()
    {
        var mapLevel1 = new Map(new int[,] {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}});
        var playerLevel1 = new Character(new Vector2Int(8, 4), Direction.Top);

        var mapLevel2 = new Map(new int[,] {
        {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
        {2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2},
        {2, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 2},
        {2, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 2},
        {2, 1, 0, 1, 1, 1, 1, 1, 4, 0, 1, 2},
        {2, 2, 0, 1, 1, 1, 1, 1, 1, 1, 1, 2},
        {2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2},
        {2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2},
        {2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2},
        {2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2},
        {2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2},
        {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}});
        var playerLevel2 = new Character(new Vector2Int(7, 2), Direction.Top);

        Levels = new List<Level>()
        {
            new Level(mapLevel1, playerLevel1),
            new Level(mapLevel2, playerLevel2)
        };
    }
}
