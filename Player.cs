using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System;

namespace DungeonProgMaster
{
    class Player
    {
        private Dictionary<PlayerMoveAnim, List<Bitmap>> animations;
        public PointF position;
        public PointF worldPosition { get; private set; }
        public Point targetPosition;
        public SizeF worldSize { get; private set; }
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
            CreateAnimations(images, global::DungeonProgMaster.PlayerMoveAnim.Top);
            CreateAnimations(images, global::DungeonProgMaster.PlayerMoveAnim.Bottom);
            CreateAnimations(images, global::DungeonProgMaster.PlayerMoveAnim.Left);
            CreateAnimations(images, global::DungeonProgMaster.PlayerMoveAnim.Right);
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

        public List<Bitmap> PlayerMoveAnim(PlayerMoveAnim move)
        {
            return animations.TryGetValue(move, out var result) ? result: throw new ArgumentException($"{move} нет в словаре");
        }

        /// <summary>
        /// Устанавливает мировые координаты и размер персонажа соответственно размеру мира
        /// </summary>
        public void SetWorldPositionAndSize(Sizer sizer)
        {
            SetWorldPosition(sizer);
            SetWorldSize(sizer);
        }

        /// <summary>
        /// Устанавливает мировые координаты игрока соответственно размеру мира
        /// </summary>
        private void SetWorldPosition(Sizer sizer)
        {
            var center = 64 * sizer.coeff / 2;
            var inWorldPosition = new PointF(sizer.floorSize.Width * position.X + (-center + sizer.floorSize.Width / 3),
                sizer.floorSize.Height * position.Y - center);
            worldPosition = inWorldPosition;
        }
        
        /// <summary>
        /// Устанавливает размер персонажа соответственно размеру мира 
        /// </summary>
        private void SetWorldSize(Sizer sizer)
        {
            var inWorldSize = new SizeF(64 * sizer.coeff * 1.2f, 64 * sizer.coeff * 1.2f);
            worldSize = inWorldSize;
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

    enum PlayerMove
    {
        Rotate = 0,
        Forward = 1,
    }
}
