public class PlayerDirection
{
    public readonly Direction StartDirection;
    public Direction CurrentDirection;

    public PlayerDirection(Direction startDirection)
    {
        StartDirection = startDirection;
        CurrentDirection = startDirection;
    }
}
