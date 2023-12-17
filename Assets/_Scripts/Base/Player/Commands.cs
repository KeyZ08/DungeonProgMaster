using System;

public static class Command
{
    public static void MoveForward(Character player, int count = 1)
    {
        if (player.CurrentDirection == Direction.Top)
            player.CurrentPosition.y += count;
        else if (player.CurrentDirection == Direction.Right)
            player.CurrentPosition.x += count;
        else if (player.CurrentDirection == Direction.Bottom)
            player.CurrentPosition.y -= count;
        else if (player.CurrentDirection == Direction.Left)
            player.CurrentPosition.x -= count;
        else
            throw new NotImplementedException(player.CurrentDirection.ToString());
    }

    public static void RotateRight(Character player)
    {
        var currentDirection = player.CurrentDirection.ToInt();
        player.CurrentDirection = (Direction)((currentDirection + 4 - 1) % 4);
    }

    public static void RotateLeft(Character player)
    {
        var currentDirection = player.CurrentDirection.ToInt();
        player.CurrentDirection = (Direction)((currentDirection + 1) % 4);
    }
}
