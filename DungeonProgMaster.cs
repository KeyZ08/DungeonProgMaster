using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            //{ 0,0,0,0,0,0,0,0,0,0,0,0 },
            //{ 1,1,1,1,1,1,1,1,1,1,1,1 },
            //{ 1,1,1,1,1,1,1,1,1,1,1,1 },
            //{ 1,0,1,1,1,1,1,1,1,1,1,1 },
            //{ 1,1,1,1,1,0,1,4,1,1,1,1 },
            //{ 1,1,1,1,1,1,1,1,1,1,1,1 },
            //{ 1,1,1,1,1,1,1,1,1,1,1,1 },
            //{ 0,0,0,0,0,0,0,0,1,1,1,1 },
            //{ 0,0,0,0,0,0,0,0,1,1,1,1 },
            //{ 0,0,0,0,0,0,0,0,1,1,1,1 },
            //{ 0,0,0,0,0,0,0,0,1,1,1,1 },
            //{ 0,0,0,0,0,0,0,0,1,1,1,1 }


            { 0,0,0,0,0,0,0,0 },
            { 1,1,1,1,1,1,1,1 },
            { 1,1,1,1,1,1,1,1 },
            { 1,0,1,1,1,1,1,1 },
            { 1,1,1,1,1,0,1,4 },
            { 1,1,1,1,1,1,1,1 },
            { 1,1,1,1,1,1,1,1 },
            { 0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0},
            { 0,0,0,0,0,0,0,0},
            { 0,0,0,0,0,0,0,0 },
            { 0,0,0,0,0,0,0,0 }
        };

        Player player = new Player(new Point(1,1));
        private Timer timer = new Timer();
        private PlayerMove movement;

        public DungeonProgMaster()
        {
            KeyDown +=  new KeyEventHandler(Keyboard);

            /*Потом Убрать*/
            KeyUp += new KeyEventHandler((sender, args) => player.currentFrame = 0);

            InitializeComponent();
            InitializeMyDesign();
            timer.Interval = 100;
            timer.Tick += new EventHandler(Update);
            timer.Start();
        }

        private void Keyboard(object sender, KeyEventArgs args)
        {
            var pos = player.position;
            if (args.KeyCode == Keys.D)
            {
                movement = PlayerMove.Right;
                player.position = new Point(pos.X+1, pos.Y);
                player.currentFrame = 1;
            }
            else if (args.KeyCode == Keys.A)
            {
                movement = PlayerMove.Left;
                player.position = new Point(pos.X - 1, pos.Y);
                player.currentFrame = 1;
            }
            else if (args.KeyCode == Keys.W)
            {
                movement = PlayerMove.Top;
                player.position = new Point(pos.X, pos.Y-1);
                player.currentFrame = 1;
            }
            else if (args.KeyCode == Keys.S)
            {
                movement = PlayerMove.Bottom;
                player.position = new Point(pos.X, pos.Y+1);
                player.currentFrame = 1;
            }
            else player.currentFrame = 0;
        }

        private void Update(object obj, EventArgs args)
        {
            //вычисление анимации
            var anim = player.PlayerMovement(movement);
            if (player.currentFrame != 0) player.currentFrame++;
            if (player.currentFrame >= anim.Count) player.currentFrame = 1;
            player.anim = anim;

            //вычисление мировых координат
            var center = 64 * sizer.coeff / 2;
            var inWorldPosition = new PointF(sizer.floorSize.Width * player.position.X + (-center + sizer.floorSize.Width / 3),
                sizer.floorSize.Height * player.position.Y - center);
            var inWorldSize = new SizeF(64 * sizer.coeff * 1.2f, 64 * sizer.coeff * 1.2f);

            player.SetWorldPosition(inWorldPosition);
            player.SetWorldSize(inWorldSize);

            gamePlace.Invalidate();
        }
    }
}
