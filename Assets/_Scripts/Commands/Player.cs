using System;
using System.Collections.Generic;

namespace Commands
{
    public class Player
    {
        public static List<string> MoveArray = new List<string>();

        private static void AddMove(string move)
        {
            if (MoveArray.Count <= 100)
                MoveArray.Add(move);
            else
                throw new Exception("Привышено максимальное количество шагов");
        }

        public static void Forward(int count = 1)
        {
            if (count < 0) throw new ArgumentOutOfRangeException("Не должно быть < 0");
            for (int i = 0; i < count; i++)
            {
                AddMove("forward");
            }
        }

        public static void TurnRight(int count = 1)
        {
            if (count < 0) throw new ArgumentOutOfRangeException("Не должно быть < 0");
            for (int i = 0; i < count; i++)
            {
                AddMove("turn_right");
            }
        }

        public static void TurnLeft(int count = 1)
        {
            if (count < 0) throw new ArgumentOutOfRangeException("Не должно быть < 0");
            for (int i = 0; i < count; i++)
            {
                AddMove("turn_left");
            }
        }

        public static void Attack()
        {
            AddMove("attack");
        }
    }

    public static class CommandsCompiler
    {
        public static List<string> Script()
        {
            #region Task
            #endregion

            return Player.MoveArray;
        }
    }
}