using System.Collections.Generic;
using System;

namespace Commands
{
    public class Player
    {
        public static List<string> MoveArray = new List<string>();

        public static void Forward(int count = 1)
        {
            if (count < 0) throw new ArgumentOutOfRangeException("Не должно быть < 0");
            for (int i = 0; i < count; i++)
            {
                MoveArray.Add("forward");
            }
        }

        public static void TurnRight(int count = 1)
        {
            if (count < 0) throw new ArgumentOutOfRangeException("Не должно быть < 0");
            for (int i = 0; i < count; i++)
            {
                MoveArray.Add("turn_right");
            }
        }

        public static void TurnLeft(int count = 1)
        {
            if (count < 0) throw new ArgumentOutOfRangeException("Не должно быть < 0");
            for (int i = 0; i < count; i++)
            {
                MoveArray.Add("turn_left");
            }
        }

        public static void Attack()
        {
            MoveArray.Add("attack");
        }
    }

    public static class CommandsCompiler
    {
        public static List<string> Script()
        {
            // to do

            return Player.MoveArray;
        }
    }
}