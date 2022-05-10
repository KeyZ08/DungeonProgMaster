using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    class Map
    {
        private Player reservePlayer;
        public Player player;
        public readonly int[,] map;
        public List<Script> scripts;

        public Map(Player player, int[,] map)
        {
            this.player = player;
            reservePlayer = new Player(Point.Ceiling(player.position), player.movement);
            this.map = map;
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

    /// <summary>
    /// Содержит словарь с текстом, соответствующим командам
    /// </summary>
    class Sketches
    {
        public static Dictionary<PlayerMove, string> sketches = new Dictionary<PlayerMove, string>()
        {
            { PlayerMove.Right, "Player.MoveRight();"},
            { PlayerMove.Left, "Player.MoveLeft();"},
            { PlayerMove.Top, "Player.MoveTop();"},
            { PlayerMove.Bottom, "Player.MoveBottom();"},
        };
    }
}
