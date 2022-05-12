using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonProgMaster
{
    class Sizer
    {
        public readonly int rows;
        public readonly int columns;
        public readonly float coeff;
        public readonly SizeF floorSize;

        public Sizer(int rows, int columns, float coeff, SizeF floorSize)
        {
            this.rows = rows;
            this.columns = columns;
            this.coeff = coeff;
            this.floorSize = floorSize;
        }
    }
}
