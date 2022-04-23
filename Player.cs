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
        private Dictionary<PlayerMove, List<Bitmap>> animations;
        private Bitmap images;
        public Point position;
        public PointF worldPosition { get; private set; }
        public SizeF worldSize { get; private set; }
        public int currentFrame;
        public List<Bitmap> anim;

        public Player(Point position)
        {
            this.position = position;
            images =new Bitmap(Application.StartupPath + @"..\..\..\Resources\Character_SpriteSheet.png");
            animations = new Dictionary<PlayerMove, List<Bitmap>>();
            CreateAnimations(PlayerMove.Top);
            CreateAnimations(PlayerMove.Bottom);
            CreateAnimations(PlayerMove.Left);
            CreateAnimations(PlayerMove.Right);

            anim = animations[0];
            currentFrame = 0;
        }
         
        public List<Bitmap> PlayerMovement(PlayerMove move)
        {
            return animations.TryGetValue(move, out var result) ? result: throw new ArgumentException();
        }

        public void SetWorldPosition(PointF pos)
        {
            this.worldPosition = pos;
        }
        
        public void SetWorldSize(SizeF size)
        {
            this.worldSize = size;
        }

        /// <summary>
        /// Разбивает общий спрайт на его части, группируя соответственно перемещению
        /// </summary>
        /// <param name="move">Направление движения</param>
        private void CreateAnimations(PlayerMove move)
        {
            var list = new List<Bitmap>();
            for(var i = 0; i < 5; i++)
            {
                list.Add(images.Clone(new Rectangle(new Point(i*64, (int)move * 64), new Size(64,64)), images.PixelFormat));
            }
            animations.Add(move, list);
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
