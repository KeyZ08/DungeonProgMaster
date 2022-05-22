using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    class Levels
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
                    new Point[]{ new Point(4,1), new Point(5,1), new Point(6,1),new Point(7,1)}, new Command[0])
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
                    new Point[]{ new Point(2,1)}, new Command[0])
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

        public void ScriptsClear()
        {
            scripts.Clear();
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

        public List<Script> GetScripts()
        {
            return GenerateScripts(scripts.ToList());
        }

        private List<Script> GenerateScripts(List<Script> scripts)
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
                    buffer.Last.Value.Add(i);
                    ScriptLoopCreate(hooks, result, buffer);
                }
                else if (hooks == 0) result.Add(i);
                else buffer.Last.Value.Add(i);
            }
            return result;
        }

        private void ScriptLoopCreate(int hooks, List<Script> result, LinkedList<List<Script>> buffer)
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
                var comboScript = GenerateScripts(buffer.Last.Value);
                buffer.RemoveLast();
                buffer.Last.Value.AddRange(comboScript);
            }
        }

        public void PlayerRotate()
        {
            player.Rotate();
        }

        public void PlayerMove(Dictionary<string, (WaveOut wave, string audio)> sounds)
        {
            if (player.position == player.targetPosition)
                return;
            
            var frame = 1.0f / player.anim.Count;
            player.Move(frame);

            if ((player.currentFrame == 2 || player.currentFrame == 4) && sounds.TryGetValue("Floor", out (WaveOut wave, string audio) floor))
            {
                floor.wave.Init(new AudioFileReader(floor.audio));
                floor.wave.Play();
            };

            if (player.position == player.targetPosition)
            {
                if (pieces.Contains(player.targetPosition) && !pickedPieces.Contains(player.targetPosition))
                {
                    pickedPieces.Add(player.targetPosition);
                    if (sounds.TryGetValue("Money", out (WaveOut wave, string audio) money))
                    {
                        money.wave.Init(new AudioFileReader(money.audio));
                        money.wave.Play(); 
                    }
                }
                player.isAnimated = false;
            }
        }
    }

    class Script
    {
        public Command Move { get; private set; }
        
        public string Sketch { get; private set; }

        public string Declaration { get; private set; }

        private readonly Action<Player> Doing;

        public Script(Command move)
        {
            Move = move;
            Sketch = Sketches.data[move].sketch;
            Declaration = Sketches.data[move].declaration;
            Doing = Commands.commands[move];
        }

        public void Play(Player player)
        {
            Doing.Invoke(player);
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
