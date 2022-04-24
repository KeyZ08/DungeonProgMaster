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

        private void Keyboard(object sender, KeyEventArgs args)
        {
            if (player.isAnimated) return;
            if (args.KeyCode == Keys.D)
            {
                player.movement = PlayerMove.Right;
                player.targetPosition.X += 1;
                player.currentFrame = 1;
            }
            else if (args.KeyCode == Keys.A)
            {
                player.movement = PlayerMove.Left;
                player.targetPosition.X -= 1;
                player.currentFrame = 1;
            }
            else if (args.KeyCode == Keys.W)
            {
                player.movement = PlayerMove.Top;
                player.targetPosition.Y -= 1;
                player.currentFrame = 1;
            }
            else if (args.KeyCode == Keys.S)
            {
                player.movement = PlayerMove.Bottom;
                player.targetPosition.Y += 1;
                player.currentFrame = 1;
            }
            else player.currentFrame = 0;

            WatсhOnTarget();

            animator.Start();
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

            if (!player.isAnimated)
                player.isAnimated = true;

            var frame = 1.0f / player.anim.Count;
            var pos = player.position;
            if (player.movement == PlayerMove.Right) pos.X += frame;
            else if (player.movement == PlayerMove.Left) pos.X -= frame;
            else if (player.movement == PlayerMove.Top) pos.Y -= frame;
            else if (player.movement == PlayerMove.Bottom) pos.Y += frame;
            player.position.X = (float)Math.Round(pos.X, 1);
            player.position.Y = (float)Math.Round(pos.Y, 1);

            player.SetWorldPosition(sizer);
            UpdatePlayerFrame();

            gamePlace.Invalidate();
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
        /// Сбрасывает состояние карты к исходному (включая персонажа)
        /// </summary>
        private void MapReset()
        {
            map.Reset(sizer);
            player = map.player;
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
    }
}
