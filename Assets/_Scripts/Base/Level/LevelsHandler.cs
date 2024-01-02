using System.Collections.Generic;
using UnityEngine;

public static class LevelsHandler
{
    public static readonly List<Level> Levels;
    
    static LevelsHandler()
    {
        //  ^ y
        //  |
        //  |      x
        //  +------>

        Levels = new List<Level>()
        {
            new Level(
                new Map(new int[,] {
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
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}}),
                new Character(new Vector2Int(4, 3), Direction.Top),
                new List<Unit>()),

            new Level(
                new Map(new int[,] {
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
                {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}}),
                new Character(new Vector2Int(2, 4), Direction.Top),
                new List<Unit>()
                {
                    new Coin(new Vector2Int(4, 4)),
                    new Coin(new Vector2Int(5, 4)),
                    new Coin(new Vector2Int(6, 4)),
                    new Skeleton(new Vector2Int(6, 3), 150),
                })
        };
    }
}
