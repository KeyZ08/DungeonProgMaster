using UnityEngine;

public class Position
{
    public readonly Vector2 StartPosition;
    public Vector2 CurrentPosition;

    public Position(Vector2 startPosition)
    {
        StartPosition = startPosition;
        CurrentPosition = startPosition;
    }
}
