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
        Level level;
        //скорость анимации
        private Timers.Timer animator;
        public DungeonProgMaster()
        {
            InitializeComponent();
            InitializeDesign();
            level = Levels.GetLevel(0);
        }

        /// <summary>
        /// Сбрасывает состояние карты к исходному (включая персонажа)
        /// </summary>
        private void LevelReset()
        {
            level.Reset(sizer);
            gamePlace.Invalidate();
        }

        #region Player

        private bool WatсhOnTarget()
        {
            var pointFpos = level.player.targetPosition;
            if (pointFpos.X == (int)pointFpos.X && pointFpos.Y == (int)pointFpos.Y)
            {
                var pos = new Point((int)pointFpos.X, (int)pointFpos.Y);
                if (pos.X < 0 || pos.Y < 0 || pos.X >= sizer.columns || pos.Y >= sizer.rows)
                {
                    MessageBox.Show("Вы пытались выйти за пределы карты, чего делать нельзя. Будьте осторожней.", "Ой",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LevelReset();
                    return false;
                }
                else if (level.map[pos.Y, pos.X] == (int)MapData.Tales.Blank)
                {
                    MessageBox.Show("Вы чуть не упали в дыру в полу. Будьте осторожней!", "Ой",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LevelReset();
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
            var player = level.player;
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
            var player = level.player;
            //вычисление анимации
            var anim = player.PlayerMoveAnim(player.movement);
            if (player.currentFrame != 0) player.currentFrame++;
            if (player.currentFrame >= anim.Count)
                player.currentFrame = 1;
            player.anim = anim;
        }

        static Dictionary<PlayerMove, Action<Player>> Commands = new()
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
            if (level.player.isAnimated) return;
            LevelReset();
            var task = Task.Run(() =>
            {
                SetEnabledControls(false, menu.Controls);
                notepad.BeginInvoke(new Action(() => notepad.Enabled = false));

                for (var i = 0; i < level.ScriptCount; i++)
                {
                    if (level.player.isAnimated) { i--; continue; }
                    level.player.isAnimated = true;
                    Commands[level.GetScript(i).Move].Invoke(level.player);
                    //выделяет исполняемую строку
                    notepad.BeginInvoke(new Action(() => notepad.SelectedIndex = i - 1));
                    if (!WatсhOnTarget())
                    {
                        SetEnabledControls(true, menu.Controls);
                        notepad.BeginInvoke(new Action(() => notepad.Enabled = true));
                        return;
                    }
                    animator = new Timers.Timer(80);
                    animator.Elapsed += PlayerMovement;
                    animator.Start();
                }
                while (level.player.isAnimated) { /*ждем*/ }
                Finished();

                SetEnabledControls(true, menu.Controls);
                notepad.BeginInvoke(new Action(() => notepad.Enabled = true));
            });
        }

        private void AddButtonMenu_ItemClick(object sender, ToolStripItemClickedEventArgs args)
        {
            var item = args.ClickedItem;
            var move = (PlayerMove)((ToolStrip)sender).Items.IndexOf(item);
            level.ScriptAdd(new Script(move), notepad);
        }

        private void NotepadResetClick(object sender, EventArgs args)
        {
            level.ScriptsClear(notepad);
        }

        private void NotepadRemoveItem(object sender, EventArgs args)
        {
            var index = notepad.SelectedIndex;
            if (index == -1) return;
            //выделяем соседнюю строку
            notepad.SelectedIndex = (notepad.Items.Count - 1 == index || index - 1 != -1) ? index - 1 : index + 1; 
            level.ScriptRemoveAt(index, notepad);
        }

        /// <summary>
        /// Устанавливает каждому из коллекции Control-ов значение Enabled равным enabled
        /// </summary>
        /// <param name="collection">Коллекция Control-ов</param>
        /// <param name="enabled">Значение, которое нужно установить в Enabled</param>
        private static void SetEnabledControls(bool enabled, Control.ControlCollection collection)
        {
            foreach (var i in collection)
            {
                var k = i as Button;
                k.BeginInvoke(new Action(() => k.Enabled = enabled));
            }
        }

        private void Finished()
        {
            if (level.map[(int)level.player.position.Y, (int)level.player.position.X] == (int)MapData.Tales.Finish)
            {
                MessageBox.Show("Уровень пройдень!", "Ура!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Задача не выполнена, попробуйте ещё раз!", "Опля",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LevelReset();
            }
        }
    }
}
