using System;

public class Command
{
    public delegate void CommandMethod(Player player, string stepsOrDirection);

    public void MoveForward(Player player, string numOfSteps)
    {
        var steps = Convert.ToInt32(numOfSteps);
        if (player.CurrentDirection == Direction.Top)
            player.CurrentPosition.y += steps;
        else if (player.CurrentDirection == Direction.Right)
            player.CurrentPosition.x += steps;
        else if (player.CurrentDirection == Direction.Bottom)
            player.CurrentPosition.y -= steps;
        else
            player.CurrentPosition.x -= steps;
    }

    public void Rotate(Player player, string direction)
    {
        direction = direction.ToLower();
        var currentPosition = player.CurrentDirection.ToInt();
        if (direction == "right")
        {
            player.CurrentDirection = (Direction)(((currentPosition - 1) % 4 + 4) % 4);
        }
        else if (direction == "left")
        {
            player.CurrentDirection = (Direction)(currentPosition + 1 % 4);
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
