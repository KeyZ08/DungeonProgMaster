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

        #region Methods
        #endregion
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