using System;

public class Command
{
    public delegate void CommandMethod(Player player, string stepsOrDirection);

    public void MoveForward(Player player, string numOfSteps)
    {
        var steps = Convert.ToInt32(numOfSteps);
        if (player.Direction.CurrentDirection == Direction.Top)
            player.Position.CurrentPosition.y += steps;
        else if (player.Direction.CurrentDirection == Direction.Right)
            player.Position.CurrentPosition.x += steps;
        else if (player.Direction.CurrentDirection == Direction.Bottom)
            player.Position.CurrentPosition.y -= steps;
        else
            player.Position.CurrentPosition.x -= steps;
    }

    public void Rotate(Player player, string direction)
    {
        direction = direction.ToLower();
        var currentPosition = player.Direction.CurrentDirection.ToInt();
        if (direction == "right")
        {
            player.Direction.CurrentDirection = (Direction)(((currentPosition - 1) % 4 + 4) % 4);
        }
        else if (direction == "left")
        {
            player.Direction.CurrentDirection = (Direction)(currentPosition + 1 % 4);
        }
        else
            throw new ArgumentException();
    }

    public void Repeat(Player player, string stepsOrDirection, CommandMethod command, int numberOfRepetitions)
    {
        CommandMethod commandMethod;
        commandMethod = command;
        for (var i = 0; i < numberOfRepetitions; i++)
        {
            commandMethod(player, stepsOrDirection);
        }
    }
}
