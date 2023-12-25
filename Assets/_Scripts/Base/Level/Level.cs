using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public readonly Map Map;
    public readonly Character Character;
    public readonly List<Unit> Units;

    public Level(Map map, Character character, List<Unit> units)
    {
        Map = map;
        Character = character;
        Units = units;
    }
}
