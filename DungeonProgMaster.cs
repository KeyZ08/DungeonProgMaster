using System;
using System.Collections.Generic;
using System.Drawing;
using Timers = System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    public partial class DungeonProgMaster : Form
    {
        Map map;
        Player player;
        //скорость анимации
        private Timers.Timer animator;
        public DungeonProgMaster()
        {
            InitializeComponent();
            InitializeDesign();
            map = new Map(new Player(new Point(1,1), PlayerMove.Bottom), new int[,]
            {
                { 0,0,0,0,0,0,0,0,0,0,0,0 },
                { 1,1,1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1,1,1 },
                { 1,0,1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,0,1,4,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,1,1,1,1 }
            });
            player = map.player;
        }

        /// <summary>
        /// Сбрасывает состояние карты к исходному (включая персонажа)
        /// </summary>
        private void MapReset()
        {
            map.Reset(sizer);
            player = map.player;
            gamePlace.Invalidate();
        }

        #region Player

        private bool WatсhOnTarget()
        {
            var pos = player.targetPosition;
            if (pos.X == (int)pos.X && pos.Y == (int)pos.Y)
            {
                if (pos.X < 0 || pos.Y < 0 || pos.X >= sizer.columns || pos.Y >= sizer.rows)
                {
                    MessageBox.Show("Вы вышли за пределы карты, чего делать нельзя.", "Ой",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MapReset();
                    return false;
                }
                else if (map.map[(int)pos.Y, (int)pos.X] != (int)MapData.Tales.Ground)
                {
                    MessageBox.Show("Вы упали в дыру в полу.", "Ой",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MapReset();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Перемещает персонажа по карте соответственно скорости анимации
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private void PlayerMovement(object sender, EventArgs args)
        {
            if (player.position == player.targetPosition)
            {
                player.isAnimated = false;
                animator.Stop();
                return;
            }

            var frame = 1.0f / player.anim.Count;
            var pos = player.position;
            if (player.movement == PlayerMove.Right) pos.X += frame;
            else if (player.movement == PlayerMove.Left) pos.X -= frame;
            else if (player.movement == PlayerMove.Top) pos.Y -= frame;
            else if (player.movement == PlayerMove.Bottom) pos.Y += frame;
            player.position.X = (float)Math.Round(pos.X, 1);
            player.position.Y = (float)Math.Round(pos.Y, 1);

            player.SetWorldPositionAndSize(sizer);
            UpdatePlayerFrame();

            gamePlace.Invalidate();
            return;
        }

        /// <summary>
        /// Обновляет картинку персонажа
        /// </summary>
        private void UpdatePlayerFrame()
        {
            //вычисление анимации
            var anim = player.PlayerMoveAnim(player.movement);
            if (player.currentFrame != 0) player.currentFrame++;
            if (player.currentFrame >= anim.Count)
                player.currentFrame = 1;
            player.anim = anim;
        }

        static Dictionary<PlayerMove, Action<Player>> Command = new()
        {
            {PlayerMove.Right, new Action<Player>((player)=>
            {
                player.movement = PlayerMove.Right;
                player.targetPosition.X += 1;
                player.currentFrame = 1;
            })},
            {PlayerMove.Left, new Action<Player>((player)=>
            {
                player.movement = PlayerMove.Left;
                player.targetPosition.X -= 1;
                player.currentFrame = 1;
            })},
            {PlayerMove.Top, new Action<Player>((player)=>
            {
                player.movement = PlayerMove.Top;
                player.targetPosition.Y -= 1;
                player.currentFrame = 1;
            })},
            {PlayerMove.Bottom, new Action<Player>((player)=>
            {
                player.movement = PlayerMove.Bottom;
                player.targetPosition.Y += 1;
                player.currentFrame = 1;
            })}
        };

        #endregion

        private void PlayButtonClick(object sender, EventArgs args)
        {
            if (player.isAnimated) return;
            MapReset();
            var task = Task.Run(() =>
            {
                SetEnabledControls(false, menu.Controls);
                notepad.BeginInvoke(new Action(() => notepad.Enabled = false));

                for (var i = 0; i < map.scripts.Count; i++)
                {
                    if (player.isAnimated) { i--; continue; }
                    player.isAnimated = true;
                    Command[map.scripts[i].Move].Invoke(player);
                    //выделяет исполняемую строку
                    notepad.BeginInvoke(new Action(() => notepad.SelectedIndex = i - 1));
                    var free = WatсhOnTarget();
                    if (!free)
                    {
                        SetEnabledControls(true, menu.Controls);
                        notepad.BeginInvoke(new Action(() => notepad.Enabled = true));
                        return;
                    }
                    animator = new Timers.Timer(80);
                    animator.Elapsed += PlayerMovement;
                    animator.Start();
                }
                while (player.isAnimated) { /*ждем*/ }
                //notepad.BeginInvoke(new Action(() => map.ScriptsClear(notepad)));
                SetEnabledControls(true, menu.Controls);
                notepad.BeginInvoke(new Action(() => notepad.Enabled = true));
            });
        }

        private void AddButtonMenu_ItemClick(object sender, ToolStripItemClickedEventArgs args)
        {
            var item = args.ClickedItem;
            var move = (PlayerMove)((ToolStrip)sender).Items.IndexOf(item);
            notepad.Items.Add(Sketches.sketches[move]);
            map.scripts.Add(new Script(move));
        }

        private void NotepadResetClick(object sender, EventArgs args)
        {
            notepad.Items.Clear();
            map.scripts.Clear();
        }

        private void NotepadRemoveItem(object sender, EventArgs args)
        {
            var index = notepad.SelectedIndex;
            if (index == -1) return;
            notepad.Items.RemoveAt(index);
            map.scripts.RemoveAt(index);
        }

        /// <summary>
        /// Устанавливает каждому из коллекции Control-ов значение Enabled равным enabled
        /// </summary>
        /// <param name="collection">Коллекция Control-ов</param>
        /// <param name="enabled">Значение, которое нужно установить в Enabled</param>
        private void SetEnabledControls(bool enabled, Control.ControlCollection collection)
        {
            foreach (var i in collection)
            {
                var k = i as Button;
                k.BeginInvoke(new Action(() => k.Enabled = enabled));
            }
        }

        private void ResultNotAchieved()
        {
            MessageBox.Show("Задача не выполнена, попробуйте ещё раз!", "Опля",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            MapReset();
        }
    }
}
