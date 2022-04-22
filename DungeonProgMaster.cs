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
            //{ 0,0,0,0,0,0,0,0},
            //{ 0,0,0,0,0,0,0,0},
            //{ 0,0,0,0,0,0,0,0 },
            //{ 0,0,0,0,0,0,0,0 }
        };
        Player player = new Player(new Point(1,1));
        private Timer timer = new Timer();
        private PlayerMove movement;
        private int currentFrame;

        public DungeonProgMaster()
        {
            KeyDown +=  new KeyEventHandler(Keyboard);
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
                currentFrame = 1;
            }
            else if (args.KeyCode == Keys.A)
            {
                movement = PlayerMove.Left;
                player.position = new Point(pos.X - 1, pos.Y);
                currentFrame = 1;
            }
            else if (args.KeyCode == Keys.W)
            {
                movement = PlayerMove.Top;
                player.position = new Point(pos.X, pos.Y-1);
                currentFrame = 1;
            }
            else if (args.KeyCode == Keys.S)
            {
                movement = PlayerMove.Bottom;
                player.position = new Point(pos.X, pos.Y+1);
                currentFrame = 1;
            }
            else currentFrame = 0;
        }

        private void Update(object obj, EventArgs args)
        {
            var gr = gamePlace.CreateGraphics();
            var rows = map.GetLength(0);
            var columns = map.GetLength(1);
            float coeff = (float)gamePlace.Height / columns / 32;
            var imageSize = new SizeF(coeff, coeff) * 32;
            
            CreateMap(gr, rows, columns, imageSize);
            PlayerAnimation(gr, coeff, imageSize);
        }
    }
}
