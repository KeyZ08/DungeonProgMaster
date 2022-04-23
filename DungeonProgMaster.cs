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
        
        private readonly int[,] map = new int[,]
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


            //{ 0,0,0,0,0,0,0,0 },
            //{ 1,1,1,1,1,1,1,1 },
            //{ 1,1,1,1,1,1,1,1 },
            //{ 1,0,1,1,1,1,1,1 },
            //{ 1,1,1,1,1,0,1,4 },
            //{ 1,1,1,1,1,1,1,1 },
            //{ 1,1,1,1,1,1,1,1 },
            //{ 0,0,0,0,0,0,0,0 },
            //{ 0,0,0,0,0,0,0,0 },
            //{ 0,0,0,0,0,0,0,0 },
            //{ 0,0,0,0,0,0,0,0 },
            //{ 0,0,0,0,0,0,0,0 }
        };

        Player player;
        //скорость анимации
        private Timer moveAnimator;
        //скорость передвижения
        private Timer animAnimator; 

        public DungeonProgMaster()
        {
            KeyDown +=  new KeyEventHandler(Keyboard);

            InitializeComponent();
            InitializeMyDesign();

            player = new Player(new Point(1, 1), PlayerMove.Bottom);
            
            moveAnimator = new Timer();
            moveAnimator.Interval = 50;
            moveAnimator.Tick += new EventHandler(PlayerMovement);

            animAnimator = new Timer();
            animAnimator.Interval = moveAnimator.Interval / player.anim.Count;
            animAnimator.Tick += new EventHandler(Update);
            animAnimator.Start();
        }

        private void Keyboard(object sender, KeyEventArgs args)
        {
            if (player.isAnimated) return;
            var pos = player.position;
            if (args.KeyCode == Keys.D)
            {
                player.movement = PlayerMove.Right;
                player.targetPosition = new PointF(pos.X+  1, pos.Y);
                player.currentFrame = 1;
            }
            else if (args.KeyCode == Keys.A)
            {
                player.movement = PlayerMove.Left;
                player.targetPosition = new PointF(pos.X - 1, pos.Y);
                player.currentFrame = 1;
            }
            else if (args.KeyCode == Keys.W)
            {
                player.movement = PlayerMove.Top;
                player.targetPosition = new PointF(pos.X, pos.Y - 1);
                player.currentFrame = 1;
            }
            else if (args.KeyCode == Keys.S)
            {
                player.movement = PlayerMove.Bottom;
                player.targetPosition = new PointF(pos.X, pos.Y + 1);
                player.currentFrame = 1;
            }
            else player.currentFrame = 0;
            moveAnimator.Start();
        }

        private void Update(object obj, EventArgs args)
        {
            //вычисление анимации
            var anim = player.PlayerMoveAnim(player.movement);
            if (player.currentFrame != 0) player.currentFrame++;
            if (player.currentFrame >= anim.Count)
            {
                player.currentFrame = 1;
            }
            player.anim = anim;
        }

        private void PlayerMovement(object obj, EventArgs args)
        {
            if (player.position == player.targetPosition)
            {
                player.isAnimated = false; 
                return;
            }

            if (!player.isAnimated)
                player.isAnimated = true;
            var pos = player.position;
            var coeff = moveAnimator.Interval * 0.01f / player.anim.Count;
            if (player.movement == PlayerMove.Right)
                player.position = new PointF((float)Math.Round(pos.X + coeff, 1), pos.Y);
            if (player.movement == PlayerMove.Left)
                player.position = new PointF((float)Math.Round(pos.X - coeff, 1), pos.Y);
            if (player.movement == PlayerMove.Top)
                player.position = new PointF(pos.X, (float)Math.Round(pos.Y - coeff, 1));
            if (player.movement == PlayerMove.Bottom)
                player.position = new PointF(pos.X, (float)Math.Round(pos.Y + coeff, 1));

            player.SetWorldPosition(sizer);

            gamePlace.Invalidate();
        }
    }
}
