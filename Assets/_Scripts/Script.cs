﻿using System;
using System.Collections.Generic;

namespace Player
{
    public class Player
    {
        public static List<string> MoveArray = new List<string>();

        public static void Go()
        {
            MoveArray.Add("go");
        }

        public static void TurnRight()
        {
            MoveArray.Add("right");
        }

        public static void TurnLeft()
        {
            MoveArray.Add("left");
        }
    }

    public static class Program
    {
        public static List<string> Script()
        {
            // to do

            return Player.MoveArray;
        }
    }
}
