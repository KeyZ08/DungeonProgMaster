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
        public PlayerMoveAnim NextMovement { get; private set; }
        public int CurrentFrame { get; private set; }
        public List<Bitmap> Anim { get; private set; }
        public bool IsAnimated { get; set; }

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
            Anim = animations[movement];
            NextMovement = movement;
            CurrentFrame = 0;
            IsAnimated = false;
        }

        public Player(Point position, PlayerMoveAnim defaultAnim, Player p)
        {
            this.position = position;
            movement = defaultAnim;
            targetPosition = position;
            animations = p.animations;
            Anim = animations[movement];
            NextMovement = movement;
            CurrentFrame = 0;
            IsAnimated = false;
        }

        public void GetNextMovement()
        {
            if (movement == PlayerMoveAnim.Right)
                NextMovement = PlayerMoveAnim.Bottom;
            else if (movement == PlayerMoveAnim.Left)
                NextMovement = PlayerMoveAnim.Top;
            else if (movement == PlayerMoveAnim.Top)
                NextMovement = PlayerMoveAnim.Right;
            else if (movement == PlayerMoveAnim.Bottom)
                NextMovement = PlayerMoveAnim.Left;
            CurrentFrame = 0;
        }

        public void GetNextTargetPosition()
        {
            if (movement == PlayerMoveAnim.Right)
                targetPosition.X += 1;
            else if (movement == PlayerMoveAnim.Left)
                targetPosition.X -= 1;
            else if (movement == PlayerMoveAnim.Top)
                targetPosition.Y -= 1;
            else if (movement == PlayerMoveAnim.Bottom)
                targetPosition.Y += 1;
            CurrentFrame = 0;
        }

        public void Move(float distance)
        {
            var pos = position;
            if (movement == PlayerMoveAnim.Right) pos.X += distance;
            else if (movement == PlayerMoveAnim.Left) pos.X -= distance;
            else if (movement == PlayerMoveAnim.Top) pos.Y -= distance;
            else if (movement == PlayerMoveAnim.Bottom) pos.Y += distance;
            position.X = (float)Math.Round(pos.X, 2);
            position.Y = (float)Math.Round(pos.Y, 2);
        }

        public void Rotate()
        {
            movement = NextMovement;
            Anim = animations[movement];
        }

        /// <summary>
        /// Обновляет картинку персонажа
        /// </summary>
        public void UpdatePlayerFrame()
        {
            //вычисление анимации
            var anim = PlayerMoveAnimations(movement);
            if (CurrentFrame >= 0) CurrentFrame++;
            if (CurrentFrame >= anim.Count)
                CurrentFrame = 0;
            this.Anim = anim;
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
