using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
                    { 1,1,1,1,1,1,1,1,1,1,1,1 },
                    { 1,1,1,1,1,0,1,0,1,1,1,1 },
                    { 1,1,1,1,1,1,1,1,1,1,1,1 },
                    { 1,1,1,1,1,1,1,1,1,1,1,1 },
                    { 0,0,0,0,0,0,0,0,1,1,1,1 },
                    { 0,0,0,0,0,0,0,0,1,1,1,1 },
                    { 0,0,0,0,0,0,0,0,1,1,1,1 },
                    { 0,0,0,0,0,0,0,0,1,1,1,1 },
                    { 0,0,0,0,0,0,0,0,1,1,1,1 }
                }, 
                    new Player(new Point(1,1), PlayerMoveAnim.Right),
                    new Point[0])
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
                    new Player(new Point(1,1), PlayerMoveAnim.Bottom),
                    new Point[]{ new Point(2,1)})
            },
        };

        public static Level GetLevel(int id)
        {
            if (levels.Count > id)
                return levels[id];
            else throw new ArgumentOutOfRangeException($"Уровня с ID:{id} ещё нет!");
        }
    }

    class Level
    {
        public readonly int id;
        public readonly int[,] map;
        readonly Player reservePlayer;
        public Player player;
        public readonly List<Point> pieces;
        public LinkedList<Script> scripts;
        public HashSet<Point> pickedPieces;

        public Level(int id, int[,] map, Player player, Point[] pieces)
        {
            this.id = id;
            this.map = map;
            this.player = player;
            reservePlayer = new Player(Point.Ceiling(player.position), player.movement);
            this.pieces = new List<Point>(pieces);
            pickedPieces = new HashSet<Point>();
            scripts = new LinkedList<Script>();
        }

        public void Reset()
        {
            player = new Player(Point.Ceiling(reservePlayer.position), reservePlayer.movement, player);
            pickedPieces.Clear();
        }

        public void ScriptsClear()
        {
            scripts.Clear();
        }

        public void ScriptUpdate(Script str)
        {
            scripts.Clear();
            scripts.AddLast(str);
        }

        public void ScriptAdd(Script str)
        {
            scripts.AddLast(str);
        }

        public void ScriptsRemove(int startS, int count)
        {
            var node = scripts.First;
            for (var i = 0; i < scripts.Count; i++)
            {
                if (i == startS)
                {
                    for (var j = 0; j <= count; j++)
                    {
                        var next = node.Next;
                        scripts.Remove(node);
                        node = next;
                    }
                    break;
                }
                node = node.Next;
            }
        }

        public void ScriptsInsert(int startS, Script script)
        {
            if (scripts.Count == 0) { scripts.AddLast(script); return; };
            var node = scripts.First;
            for (var i = 0; i < scripts.Count; i++)
            {
                if (i == startS)
                {
                    scripts.AddAfter(node, script);
                    break;
                }
                node = node.Next;
            }
        }

        public Script[] GetAllScripts()
        {
            return scripts.ToArray();
        }
    }

    class Script
    {
        public Command Move { get; private set; }
        public string Sketch { get; private set; }

        public Script(Command move)
        {
            Move = move;
            Sketch = Sketches.sketches[move];
        }
    }

    static class Sketches
    {
        public static Dictionary<Command, string> sketches = new Dictionary<Command, string>()
        {
            {Command.Forward, "Player.MoveForward();" },
            {Command.Rotate, "Player.Rotate();" },
        };

        public static Dictionary<string, Command> sketchesByName = new Dictionary<string, Command>()
        {
            {"Player.MoveForward();", Command.Forward },
            {"Player.Rotate();", Command.Rotate},
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
