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
        };
        Player player = new Player();

        public DungeonProgMaster()
        {
            InitializeComponent();
            InitializeMyDesign();
        }
    }
}
