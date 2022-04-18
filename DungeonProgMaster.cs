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
        public readonly int rowCount = 15;
        public readonly int columnCount = 15;

        public DungeonProgMaster()
        {
            InitializeComponent();
            InitializeMyDesign();
        }
    }
}
