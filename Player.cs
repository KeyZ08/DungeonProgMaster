using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;

namespace DungeonProgMaster
{
    class Player
    {
        Dictionary<PlayerMove, List<Bitmap>> animation;
        Bitmap images;

        public Player()
        {
            images =new Bitmap(Application.StartupPath + @"..\..\..\Resources\Character_SpriteSheet.png");
            animation = new Dictionary<PlayerMove, List<Bitmap>>();
            CreateAnimations(PlayerMove.Top);
            CreateAnimations(PlayerMove.Bottom);
            CreateAnimations(PlayerMove.Left);
            CreateAnimations(PlayerMove.Right);
        }
         
        public List<Bitmap> PlayerMovement(PlayerMove move)
        {
            return animation.TryGetValue(move, out var result)? result: throw new ArgumentException();
        }

        private void CreateAnimations(PlayerMove move)
        {
            var list = new List<Bitmap>();
            for(var i = 0; i < 5; i++)
            {
                list.Add(images.Clone(new Rectangle(new Point(i*64, (int)move * 64), new Size(64,64)), images.PixelFormat));
            }
            animation.Add(move, list);
        }
    }

    enum PlayerMove
    {
        Top = 1,
        Bottom = 0,
        Left = 3,
        Right = 2
    }
}
