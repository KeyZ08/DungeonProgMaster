using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System;

namespace DungeonProgMaster
{
    class Player
    {
        readonly Dictionary<PlayerMoveAnim, List<Bitmap>> animations;
        public PointF position;
        public Point targetPosition;
        public PlayerMoveAnim movement;
        public int currentFrame = 0;
        public List<Bitmap> anim;
        public bool isAnimated = false;

        public Player(Point position, PlayerMoveAnim defaultAnim)
        {
            this.position = position;
            targetPosition = position;
            var images = new Bitmap(Application.StartupPath + @"..\..\..\Resources\Character_SpriteSheet.png");
            animations = new Dictionary<PlayerMoveAnim, List<Bitmap>>();
            CreateAnimations(images, PlayerMoveAnim.Top);
            CreateAnimations(images, PlayerMoveAnim.Bottom);
            CreateAnimations(images, PlayerMoveAnim.Left);
            CreateAnimations(images, PlayerMoveAnim.Right);
            movement = defaultAnim;
            anim = animations[movement];
        }

        public Player(Point position, PlayerMoveAnim defaultAnim, Player p)
        {
            this.position = position;
            movement = defaultAnim;
            targetPosition = position;
            animations = p.animations;
            anim = animations[movement];
        }

        public List<Bitmap> PlayerMoveAnimations(PlayerMoveAnim move)
        {
            return animations.TryGetValue(move, out var result) ? result: throw new ArgumentException($"{move} нет в словаре");
        }

        /// <summary>
        /// Разбивает общий спрайт на его части, группируя в списки для анимации соответственно перемещению
        /// </summary>
        /// <param name="move">Направление движения</param>
        private void CreateAnimations(Bitmap images, PlayerMoveAnim move)
        {
            var list = new List<Bitmap>();
            for(var i = 0; i < 5; i++)
                list.Add(images.Clone(new Rectangle(new Point(i*64, (int)move * 64), new Size(64,64)), images.PixelFormat));
            animations.Add(move, list);
        }
    }

    enum PlayerMoveAnim
    {
        Top = 1,
        Bottom = 0,
        Left = 3,
        Right = 2,
        Rotate = 4,
        Forward = 5
    }
}
