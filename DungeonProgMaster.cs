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
                    MessageBox.Show("Похоже вы пытались выйти за пределы карты, чего делать нельзя. Будьте осторожней.", "Ой",
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
        private void PlayerMovementAnim(object sender, EventArgs args)
        {
            var player = level.player;
            if (player.position == player.targetPosition)
            {
                player.isAnimated = false;
                animator.Dispose();
            }
            else
            {
                var frame = 1.0f / player.anim.Count;
                var pos = player.position;
                if (player.movement == PlayerMoveAnim.Right) pos.X += frame;
                else if (player.movement == PlayerMoveAnim.Left) pos.X -= frame;
                else if (player.movement == PlayerMoveAnim.Top) pos.Y -= frame;
                else if (player.movement == PlayerMoveAnim.Bottom) pos.Y += frame;
                player.position.X = (float)Math.Round(pos.X, 2);
                player.position.Y = (float)Math.Round(pos.Y, 2);


                player.SetWorldPositionAndSize(sizer);
            }
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
            {
                PlayerMove.Forward,
                new Action<Player>((player) =>
                {
                    if(player.movement == PlayerMoveAnim.Right)
                        player.targetPosition.X += 1;
                    else if (player.movement == PlayerMoveAnim.Left)
                        player.targetPosition.X -= 1;
                    else if (player.movement == PlayerMoveAnim.Top)
                        player.targetPosition.Y -= 1;
                    else if (player.movement == PlayerMoveAnim.Bottom)
                        player.targetPosition.Y += 1;
                    player.currentFrame = 1;
                })
            },
            {
                PlayerMove.Rotate,
                new Action<Player>((player) =>
                {
                    if (player.movement == PlayerMoveAnim.Right)
                        player.movement = PlayerMoveAnim.Bottom;
                    else if (player.movement == PlayerMoveAnim.Left)
                        player.movement = PlayerMoveAnim.Top;
                    else if (player.movement == PlayerMoveAnim.Top)
                        player.movement = PlayerMoveAnim.Right;
                    else if (player.movement == PlayerMoveAnim.Bottom)
                        player.movement = PlayerMoveAnim.Left;
                    player.currentFrame = 0;
                })
            },
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
                    animator = level.GetScript(i).Move == PlayerMove.Rotate? new Timers.Timer(150) : new Timers.Timer(100);
                    animator.Elapsed += PlayerMovementAnim;
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
                var message = MessageBox.Show("Уровень пройден! Перейти на следующий уровень? ", "Ура!", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (message == DialogResult.Yes)
                {
                    level = Levels.GetLevel(level.id + 1);
                    LevelReset();
                    notepad.BeginInvoke(new Action(() => level.ScriptsClear(notepad)));
                }
                else LevelReset();
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
