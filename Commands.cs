using System;
using System.Collections.Generic;

namespace DungeonProgMaster
{
    class Commands
    {
        public static void ForwardMove(Player player)
        {
            if (player.movement == PlayerMoveAnim.Right)
                player.targetPosition.X += 1;
            else if (player.movement == PlayerMoveAnim.Left)
                player.targetPosition.X -= 1;
            else if (player.movement == PlayerMoveAnim.Top)
                player.targetPosition.Y -= 1;
            else if (player.movement == PlayerMoveAnim.Bottom)
                player.targetPosition.Y += 1;
            player.currentFrame = 0;
        }

        public static void Rotate(Player player)
        {
            if (player.movement == PlayerMoveAnim.Right)
                player.nextMovement = PlayerMoveAnim.Bottom;
            else if (player.movement == PlayerMoveAnim.Left)
                player.nextMovement = PlayerMoveAnim.Top;
            else if (player.movement == PlayerMoveAnim.Top)
                player.nextMovement = PlayerMoveAnim.Right;
            else if (player.movement == PlayerMoveAnim.Bottom)
                player.nextMovement = PlayerMoveAnim.Left;
            player.currentFrame = 0;
        }

        public static void RepeatStart(Player player)
        {
            
        }

        public static void RepeatEnd(Player player)
        {

        }

        public static Dictionary<Command, Action<Player>> commands = new()
        {
            {
                Command.Forward,
                ForwardMove
            },
            {
                Command.Rotate,
                Rotate
            },
            {
                Command.RepeatStart,
                RepeatStart
            },
            {
                Command.RepeatEnd,
                RepeatEnd
            }
        };
    }

    enum Command
    {
        Rotate = 0,
        Forward = 1,
        RepeatStart = 2,
        RepeatEnd = 3
    }

    static class Sketches
    {
        public static Dictionary<Command, (string sketch, string declaration)> data = new Dictionary<Command, (string sketch, string declaration)>()
        {
            {Command.Forward, ("Player.MoveForward();", "Player.MoveForward();") },
            {Command.Rotate, ("Player.Rotate();", "Player.Rotate();") },
            {Command.RepeatStart, ("Repeat(5){", "Repeat()") },
            {Command.RepeatEnd, ("}", "EndRepeat()") },
        };
    }
}
