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
                player.movement = PlayerMoveAnim.Bottom;
            else if (player.movement == PlayerMoveAnim.Left)
                player.movement = PlayerMoveAnim.Top;
            else if (player.movement == PlayerMoveAnim.Top)
                player.movement = PlayerMoveAnim.Right;
            else if (player.movement == PlayerMoveAnim.Bottom)
                player.movement = PlayerMoveAnim.Left;
            player.currentFrame = 0;
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
        };
    }
}
