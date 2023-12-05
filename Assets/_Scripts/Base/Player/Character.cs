using UnityEngine;

public class Character
{
    public readonly Vector2 StartPosition;
    public Vector2 CurrentPosition;

    public readonly Direction StartDirection;
    public Direction CurrentDirection;

    public Character(Vector2 statPosition, Direction startDirection)
    {
        StartPosition = statPosition;
        CurrentPosition = statPosition;

        StartDirection = startDirection;
        CurrentDirection = startDirection;
    }
}
