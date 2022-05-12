using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    static class Levels
    {
        static readonly List<Level> levels = new List<Level>()
        {
            {
                new Level(0,
                    new int[,]
                {
                    { 0,0,0,0,0,0,0,0,0,0,0,0 },
                    { 1,1,1,1,1,1,1,1,1,2,1,1 },
                    { 1,1,1,1,1,1,1,1,1,1,1,1 },
                    { 1,0,1,1,1,1,1,1,1,1,1,1 },
                    { 1,1,1,1,1,0,1,0,1,1,1,1 },
                    { 1,1,1,1,1,1,1,1,1,1,1,1 },
                    { 1,1,1,1,1,1,1,1,1,1,1,1 },
                    { 0,0,0,0,0,0,0,0,1,1,1,1 },
                    { 0,0,0,0,0,0,0,0,1,1,1,1 },
                    { 0,0,0,0,0,0,0,0,1,1,1,1 },
                    { 0,0,0,0,0,0,0,0,1,1,1,1 },
                    { 0,0,0,0,0,0,0,0,1,1,1,1 }
                }, 
                    new Player(new Point(1,1), PlayerMoveAnim.Bottom))
            },
            {
                new Level(1,
                    new int[,]
                {
                    { 1,1,1,0,1,1,1,1,1,1,1,1 },
                    { 1,1,1,0,1,1,1,1,1,1,1,1 },
                    { 1,1,1,0,1,1,1,1,1,1,1,1 },
                    { 0,1,0,0,1,1,1,1,1,1,1,1 },
                    { 0,1,0,0,0,0,0,1,1,1,1,1 },
                    { 0,1,1,1,1,1,0,1,1,1,1,1 },
                    { 0,0,0,0,0,1,0,1,1,1,1,1 },
                    { 1,1,1,1,0,1,0,1,1,1,1,1 },
                    { 1,1,1,1,0,1,0,0,0,0,0,1 },
                    { 1,1,1,1,0,1,1,1,1,2,0,1 },
                    { 1,1,1,1,0,0,0,0,0,0,0,1 },
                    { 1,1,1,1,1,1,1,1,1,1,1,1 }
                }, 
                    new Player(new Point(1,1), PlayerMoveAnim.Bottom))
            },
        };

        public static Level GetLevel(int id)
        {
            if (levels.Count > id)
                return levels[id];
            else throw new ArgumentOutOfRangeException("Уровня с таким ID ещё нет!");
        }
    }

    class Level
    {
        public readonly int id;
        public readonly int[,] map;
        private Player reservePlayer;
        public Player player;
        private List<Script> scripts;
        public int ScriptCount { get { return scripts.Count; } }

        public Level(int id, int[,] map, Player player)
        {
            this.id = id;
            this.map = map;
            this.player = player;
            reservePlayer = new Player(Point.Ceiling(player.position), player.movement);
            scripts = new List<Script>();
        }

        public void Reset(Sizer sizer)
        {
            player = new Player(Point.Ceiling(reservePlayer.position), reservePlayer.movement, player);
            player.SetWorldPositionAndSize(sizer);
        }

        public void ScriptsClear(ListBox notepad)
        {
            notepad.Items.Clear();
            scripts.Clear();
        }

        public Script GetScript(int index)
        {
            return scripts[index];
        }

        public void ScriptRemoveAt(int index, ListBox notepad)
        {
            scripts.RemoveAt(index);
            notepad.Items.RemoveAt(index);
        }
        
        public void ScriptInsert(int index, Script script, ListBox notepad)
        {
            scripts.Insert(index, script);
            notepad.Items.Insert(index, script.Sketch);
        }

        public void ScriptAdd( Script script, ListBox notepad)
        {
            scripts.Add(script);
            notepad.Items.Add(script.Sketch);
        }
    }

    class Script
    {
        public PlayerMove Move { get; private set; }
        public string Sketch { get; private set; }

        public Script(PlayerMove move)
        {
            Move = move;
            Sketch = Sketches.sketches[move];
        }
    }

    static class Sketches
    {
        public static Dictionary<PlayerMove, string> sketches = new Dictionary<PlayerMove, string>()
        {
            {PlayerMove.Forward, "Player.MoveForward()" },
            {PlayerMove.Rotate, "Player.Rotate()" },
        };
    }

    //шаблон
    //new int[,]
    //            {
    //                { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //                { 1,1,1,1,1,1,1,1,1,2,1,1 },
    //                { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //                { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //                { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //                { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //                { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //                { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //                { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //                { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //                { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //                { 1,1,1,1,1,1,1,1,1,1,1,1 }
    //            }, 
}
