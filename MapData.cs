using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    static class MapData
    {
        readonly static Dictionary<Tales, Bitmap> images = new Dictionary<Tales, Bitmap>()
        {
            { Tales.Blank, new Bitmap(Application.StartupPath + @"..\..\..\Resources\Blank.png")},
            { Tales.Ground, new Bitmap(Application.StartupPath + @"..\..\..\Resources\Ground.png")},
            { Tales.Finish, new Bitmap(Application.StartupPath + @"..\..\..\Resources\Finish.png")},
        };

        public static Bitmap GetTale(int tale)
        {
            return images.TryGetValue((Tales)tale, out var bitmap) ? bitmap : images[Tales.Blank];
        }

        public enum Tales
        {
            Blank = 0,
            Ground = 1,
            Finish = 2,
        }
    }
}
