using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DungeonProgMaster
{
    class Levels
    {
        static readonly List<Level> levels = new()
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
                    new Point[]{ new Point(4,1), new Point(5,1), new Point(6,1),new Point(7,1)}, Array.Empty<Command>())
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
                    new Point[]{ new Point(2,1)}, Array.Empty<Command>())
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
        public readonly Script[] openedScripts;

        public Level(int id, int[,] map, Player player, Point[] pieces, Command[] openedCommands)
        {
            this.id = id;
            this.map = map;
            this.player = player;
            reservePlayer = new Player(Point.Ceiling(player.position), player.movement);
            this.pieces = new List<Point>(pieces);
            pickedPieces = new HashSet<Point>();
            scripts = new LinkedList<Script>();

            
            if (openedCommands.Length == 0)
            {
                openedScripts = new Script[4];
                openedScripts[0] = new Script(Command.Forward);
                openedScripts[1] = new Script(Command.Rotate);
                openedScripts[2] = new Script(Command.RepeatStart);
                openedScripts[3] = new Script(Command.RepeatEnd);
            }
            else
            {
                openedScripts = new Script[openedCommands.Length];
                for (var i = 0; i < openedCommands.Length; i++)
                {
                    openedScripts[i] = new Script(openedCommands[i]);
                }
            }
        }

        public void Reset()
        {
            player = new Player(Point.Ceiling(reservePlayer.position), reservePlayer.movement, player);
            pickedPieces.Clear();
        }

        public void ScriptsClear() => scripts.Clear();
        
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

        public List<Script> GetScripts()
        {
            return ScriptParse(scripts.ToList());
        }

        private List<Script> ScriptParse(List<Script> scripts)
        {
            if (scripts == null || scripts.Count == 0) return new List<Script>();
            int hooks = 0;
            var result = new List<Script>();
            var buffer = new LinkedList<List<Script>>();
            foreach (var i in scripts)
            {
                if (i.Move == Command.RepeatStart)
                {
                    hooks++;
                    buffer.AddLast(new List<Script>());
                    buffer.Last.Value.Add(i);
                }
                else if (i.Move == Command.RepeatEnd)
                {
                    hooks--;
                    if (hooks < 0) continue;
                    buffer.Last.Value.Add(i);
                    ScriptRepeatCreate(hooks, result, buffer);
                }
                else if (hooks == 0) result.Add(i);
                else buffer.Last.Value.Add(i);
            }
            return result;
        }

        private void ScriptRepeatCreate(int hooks, List<Script> result, LinkedList<List<Script>> buffer)
        {
            if (hooks == 0)
            {
                buffer.Last.Value.RemoveAt(0);
                buffer.Last.Value.RemoveAt(buffer.Last.Value.Count - 1);
                var comboScript = new List<Script>();
                for (var j = 0; j < 4; j++)
                    comboScript.AddRange(buffer.Last.Value);
                result.AddRange(comboScript);
            }
            else
            {
                var comboScript = ScriptParse(buffer.Last.Value);
                buffer.RemoveLast();
                buffer.Last.Value.AddRange(comboScript);
            }
        }

        public bool ItIsPiece()
        {
            if (player.position != player.targetPosition) return false;
            return pieces.Contains(player.targetPosition);
        }

        public bool ItIsPickedPiece()
        {
            if (player.position != player.targetPosition) return false;
            return pickedPieces.Contains(player.targetPosition);
        }

        public bool AllPiecesAssembled()
        {
            return pickedPieces.Count == pieces.Count;
        }

        public bool IsFinished()
        {
            return map[(int)player.position.Y, (int)player.position.X] == (int)MapData.Tales.Finish;
        }

        public void TakePeace()
        {
            if (player.position != player.targetPosition) throw new Exception("Ошибка расположения игрока");
            if (pickedPieces.Contains(player.targetPosition) || !pieces.Contains(player.targetPosition)) throw new Exception("Монета уже подобрана, либо её нет на этом месте");
            pickedPieces.Add(player.targetPosition);
        }

        public MapData.Tales WatchOnTarget()
        {
            var pos = player.targetPosition;
            if (pos.X < 0 || pos.Y < 0 || pos.X >= map.GetLength(0) || pos.Y >= map.GetLength(1))
            {
                return MapData.Tales.Wall;
            }
            else if (map[pos.Y, pos.X] == (int)MapData.Tales.Blank)
            {
                return MapData.Tales.Blank;
            }
            else return MapData.Tales.Ground;
        }
    }

    //шаблон
    //{
    //    new Level(0,
    //        new int[,]
    //    {
    //        { 0,0,0,0,0,0,0,0,0,0,0,0 },
    //        { 1,1,1,1,1,1,1,1,1,2,1,1 },
    //        { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //        { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //        { 1,1,1,1,1,0,1,0,1,1,1,1 },
    //        { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //        { 1,1,1,1,1,1,1,1,1,1,1,1 },
    //        { 0,0,0,0,0,0,0,0,1,1,1,1 },
    //        { 0,0,0,0,0,0,0,0,1,1,1,1 },
    //        { 0,0,0,0,0,0,0,0,1,1,1,1 },
    //        { 0,0,0,0,0,0,0,0,1,1,1,1 },
    //        { 0,0,0,0,0,0,0,0,1,1,1,1 }
    //    }, 
    //        new Player(new Point(1, 1), PlayerMoveAnim.Right),
    //        new Point[] { new Point(4, 1), new Point(5, 1), new Point(6, 1), new Point(7, 1) }, new Command[0])
    //},
}
