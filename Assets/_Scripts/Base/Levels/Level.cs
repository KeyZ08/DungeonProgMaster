public class Level
{
    public readonly Map Map;
    public Character Player;

    public Level(Map map, Character player)
    {
        Map = map;
        Player = player;
    }
}
