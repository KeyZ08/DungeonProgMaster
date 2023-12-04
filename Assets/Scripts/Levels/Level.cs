using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public readonly Map Map;
    public Player Player;

    public Level(Map map, Player player)
    {
        Map = map;
        Player = player;
    }
}
