using System;
using System.Collections.Generic;

namespace DungeonProgMaster
{
    class Commands
    {
        public static void ForwardMove(Player player)
        {
            player.GetNextTargetPosition();
        }

        public static void Rotate(Player player)
        {
            player.GetNextMovement();
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
        public static Dictionary<Command, (string sketch, string declaration)> data = new()
        {
            {Command.Forward, ("Player.Forward();", "Player.Forward();") },
            {Command.Rotate, ("Player.Rotate();", "Player.Rotate();") },
            {Command.RepeatStart, ("Repeat(4){", "Repeat(4)") },
            {Command.RepeatEnd, ("}", "EndRepeat()") },
        };
    }
}
