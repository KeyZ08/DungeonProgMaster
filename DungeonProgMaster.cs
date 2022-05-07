using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    public partial class DungeonProgMaster : Form
    {
        Map map;
        Player player;
        //скорость анимации
        private Timer animator;

        public DungeonProgMaster()
        {
            KeyUp += new KeyEventHandler(Keyboard);

            InitializeComponent();
            InitializeMyDesign();
            map = new Map(new Player(new Point(1,1), PlayerMove.Bottom), new int[,]
            {
                { 0,0,0,0,0,0,0,0,0,0,0,0 },
                { 1,1,1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1,1,1 },
                { 1,0,1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,0,1,4,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,1,1,1,1 }
            });
            player = map.player;
            
            animator = new Timer();
            animator.Interval = 50;
            animator.Tick += new EventHandler(PlayerMovement);
        }



        /// <summary>
        /// Сбрасывает состояние карты к исходному (включая персонажа)
        /// </summary>
        private void MapReset()
        {
            map.Reset(sizer);
            player = map.player;
            gamePlace.Invalidate();
        }

        #region Player

        private void Keyboard(object sender, KeyEventArgs args)
        {
            if (player.isAnimated) return;
            player.isAnimated = true;
            if (args.KeyCode == Keys.D)
                Command[PlayerMove.Right].Invoke(player);
            else if (args.KeyCode == Keys.A)
                Command[PlayerMove.Left].Invoke(player);
            else if (args.KeyCode == Keys.W)
                Command[PlayerMove.Top].Invoke(player);
            else if (args.KeyCode == Keys.S)
                Command[PlayerMove.Bottom].Invoke(player);
            else player.currentFrame = 0;

            WatсhOnTarget();

            animator.Start();
        }

        private void WatсhOnTarget()
        {
            var pos = player.targetPosition;
            if (pos.X == (int)pos.X && pos.Y == (int)pos.Y)
            {
                if (pos.X < 0 || pos.Y < 0 || pos.X >= sizer.columns || pos.Y >= sizer.rows)
                {
                    MessageBox.Show
                        ("Вы вышли за пределы карты", "Ой",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MapReset();
                }
                else if (map.map[(int)pos.Y, (int)pos.X] != (int)MapData.Tales.Ground)
                {
                    MessageBox.Show
                        ("Вы упали в дыру в полу", "Ой",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MapReset();
                }
            }
        }

        /// <summary>
        /// Перемещает персонажа по карте соответственно скорости анимации
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private void PlayerMovement(object obj, EventArgs args)
        {
            if (player.position == player.targetPosition)
            {
                player.isAnimated = false;
                animator.Stop();
                return;
            }

            var frame = 1.0f / player.anim.Count;
            var pos = player.position;
            if (player.movement == PlayerMove.Right) pos.X += frame;
            else if (player.movement == PlayerMove.Left) pos.X -= frame;
            else if (player.movement == PlayerMove.Top) pos.Y -= frame;
            else if (player.movement == PlayerMove.Bottom) pos.Y += frame;
            player.position.X = (float)Math.Round(pos.X, 1);
            player.position.Y = (float)Math.Round(pos.Y, 1);

            player.SetWorldPositionAndSize(sizer);
            UpdatePlayerFrame();

            gamePlace.Invalidate();
        }

        /// <summary>
        /// Обновляет картинку персонажа
        /// </summary>
        private void UpdatePlayerFrame()
        {
            //вычисление анимации
            var anim = player.PlayerMoveAnim(player.movement);
            if (player.currentFrame != 0) player.currentFrame++;
            if (player.currentFrame >= anim.Count)
                player.currentFrame = 1;
            player.anim = anim;
        }

        static Dictionary<PlayerMove, Action<Player>> Command = new Dictionary<PlayerMove, Action<Player>>()
        {
            {PlayerMove.Right, new Action<Player>((player)=>
            {
                player.movement = PlayerMove.Right;
                player.targetPosition.X += 1;
                player.currentFrame = 1;
            })},
            {PlayerMove.Left, new Action<Player>((player)=>
            {
                player.movement = PlayerMove.Left;
                player.targetPosition.X -= 1;
                player.currentFrame = 1;
            })},
            {PlayerMove.Top, new Action<Player>((player)=>
            {
                player.movement = PlayerMove.Top;
                player.targetPosition.Y -= 1;
                player.currentFrame = 1;
            })},
            {PlayerMove.Bottom, new Action<Player>((player)=>
            {
                player.movement = PlayerMove.Bottom;
                player.targetPosition.Y += 1;
                player.currentFrame = 1;
            })}
        };
        #endregion

        class Script
        {
            public PlayerMove Move { get; private set; }
            public string Sketch { get; private set; }

            public Script(PlayerMove move)
            {
                Move = move;
                Sketch = sketches[move];
            }
        }

        static Dictionary<PlayerMove, string> sketches = new Dictionary<PlayerMove, string>()
        {
            { PlayerMove.Right, "Player.RightMove()"},
            { PlayerMove.Left, "Player.LeftMove()"},
            { PlayerMove.Top, "Player.TopMove()"},
            { PlayerMove.Bottom, "Player.BottomMove()"},
        };
    }
}
