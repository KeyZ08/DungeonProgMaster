using UnityEngine;

public class Player
{
    public Position Position;
    public PlayerDirection Direction;

    public Player(Position position, PlayerDirection direction)
    {
        Position = position;
        Direction = direction;
    }
}
