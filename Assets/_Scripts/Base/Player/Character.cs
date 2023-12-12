using UnityEngine;

public class Character
{
    public readonly Vector2Int StartPosition;
    public Vector2Int CurrentPosition;

    public readonly Direction StartDirection;
    public Direction CurrentDirection;

    public Character(Vector2Int startPosition, Direction startDirection)
    {
        StartPosition = startPosition;
        CurrentPosition = startPosition;

        StartDirection = startDirection;
        CurrentDirection = startDirection;
    }
}
