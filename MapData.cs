using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    static class MapData
    {
        private static Dictionary<Tales, Bitmap> images = new Dictionary<Tales, Bitmap>()
        {
            { Tales.Blank, new Bitmap(Application.StartupPath + @"..\..\..\Resources\Blank.png")},
            { Tales.Ground, new Bitmap(Application.StartupPath + @"..\..\..\Resources\Ground.png")}
        };

        public static Bitmap GetTale(int tale)
        {
            return images.TryGetValue((Tales)tale, out var bitmap) ? bitmap : images[Tales.Blank];
        }

        private enum Tales
        {
            Blank = 0,
            Ground = 1,
        }
    }
}
