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
        public readonly int rowCount = 10;
        public readonly int columnCount = 10;

        public DungeonProgMaster()
        {
            InitializeComponent();
            InitializeMyDesign();
        }
    }
}
