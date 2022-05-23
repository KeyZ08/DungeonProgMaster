using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    static class MapData
    {
        readonly static Dictionary<Tales, Bitmap> images = new()
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
            Wall = 4,
        }

        public class Piece
        {
            private int currentFrame = 0;

            public int CurrentFrame
            {
                get { return currentFrame; }
                set
                {
                    if (value >= Frames.Count) currentFrame = 0;
                    else if (value < 0) currentFrame = Frames.Count - 1;
                    else currentFrame = value;
                }
            }

            public List<Image> Frames { get; private set; }

            public Piece()
            {
                Frames = new List<Image>();
                var images = new Bitmap(Application.StartupPath + @"..\..\..\Resources\Piece_Images.png");
                for (var i = 0; i < 10; i++)
                    Frames.Add(images.Clone(new Rectangle(new Point(i * 32, 0), new Size(32, 32)), images.PixelFormat));
            }
        }
    }
}
