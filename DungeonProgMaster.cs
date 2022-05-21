using System;
using System.Drawing;
using Timers = System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DungeonProgMaster
{
    public partial class DungeonProgMaster : Form
    {
        Level level;

        private PointF WorldPlayerPosition;
        private SizeF WorldPlayerSize;
        private Timers.Timer pieceAnimator;
        private float frameTimeSpeed = 1f;//чем больше тем медленнее
        private readonly Piece pieceData;

        public DungeonProgMaster()
        {
            level = Levels.GetLevel(0);
            InitializeComponent();
            InitializeDesign();

            pieceData = new Piece();
            pieceAnimator = new Timers.Timer(100 * frameTimeSpeed);
            pieceAnimator.Elapsed += PieceUpdateFrame;
            pieceAnimator.Start();
        }

        /// <summary>
        /// Сбрасывает состояние карты к исходному (включая персонажа)
        /// </summary>
        private void LevelReset()
        {
            level.Reset();
            SetPlayerWorldPositionAndSize(sizer);
            gamePlace.Invalidate();
        }

        private void OnKeyDownNotepad(object sender, KeyEventArgs args)
        {
            var key = args.KeyCode;
            if(key == Keys.Back)
            {
                NotepadRemoveItem();
            }
            else if(key == Keys.Enter)
            {
            }
        }

        #region Player

        /// <summary>
        /// Перемещает персонажа по карте соответственно скорости анимации
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        private void PlayerMovement()
        {
            level.PlayerMove();

            SetPlayerWorldPositionAndSize(sizer);
            UpdatePlayerFrame();
            
            if (level.player.position != level.player.targetPosition)
            {
                System.Threading.Thread.Sleep(100);
                PlayerMovement();
            }
            if (level.player.nextMovement != level.player.movement)
            {
                System.Threading.Thread.Sleep(150);
                level.PlayerRotate();
            }

            gamePlace.Invalidate();
        }

        /// <summary>
        /// Обновляет картинку персонажа
        /// </summary>
        private void UpdatePlayerFrame()
        {
            var player = level.player;
            //вычисление анимации
            var anim = player.PlayerMoveAnimations(player.movement);
            if (player.currentFrame >= 0) player.currentFrame++;
            if (player.currentFrame >= anim.Count)
                player.currentFrame = 0;
            player.anim = anim;
        }

        /// <summary>
        /// Устанавливает мировые координаты и размер персонажа соответственно размеру мира
        /// </summary>
        private void SetPlayerWorldPositionAndSize(Sizer sizer)
        {
            WorldPlayerSize = sizer.GetWorldSize(new Size(64, 64));
            WorldPlayerPosition = sizer.GetWorldPosition(level.player.position, WorldPlayerSize);
        }

        private bool WatсhOnTarget()
        {
            var pointFpos = level.player.targetPosition;
            if (pointFpos.X == pointFpos.X && pointFpos.Y == pointFpos.Y)
            {
                var pos = new Point(pointFpos.X, pointFpos.Y);
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

        private void Finished()
        {
            if(level.pickedPieces.Count != level.pieces.Count)
            {
                MessageBox.Show("Для перехода на следующий уровень нужно собрать все монеты!", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LevelReset();
            }
            else if (level.map[(int)level.player.position.Y, (int)level.player.position.X] == (int)MapData.Tales.Finish)
            {
                var message = MessageBox.Show("Уровень пройден! Перейти на следующий уровень? ", "Ура!",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (message == DialogResult.Yes)
                {
                    level = Levels.GetLevel(level.id + 1);
                    LevelReset();
                    //notepad.BeginInvoke(new Action(() => level.ScriptsClear(notepad)));
                }
                else LevelReset();
            }
            else
            {
                MessageBox.Show("Похоже вы не дошли до финиша, попробуйте ещё раз!", "Опля",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LevelReset();
            }
        }

        #endregion

        private void PlayButtonClick(object sender, EventArgs args)
        {
            if (level.player.isAnimated) return;
            LevelReset();

            var task = Task.Run(() =>
            {
                SetEnabledControls(false, menu.Controls);
                var scripts = level.GetScripts();
                for (var i = 0; i < scripts.Count; i++)
                {
                    level.player.isAnimated = true;
                    scripts[i].Play(level.player);
                    if (!WatсhOnTarget())
                    {
                        SetEnabledControls(true, menu.Controls);
                        return;
                    }
                    PlayerMovement();
                }
                Finished();

                SetEnabledControls(true, menu.Controls);
                notepad.BeginInvoke(new Action(() => notepad.Enabled = true));
            });
        }

        private void AddButtonMenu_ItemClick(object sender, ToolStripItemClickedEventArgs args)
        {
            var start = notepad.SelectionStart - 1;
            var end = start + notepad.SelectionLength;
            var command = level.openedScripts[contextMenu.Items.IndexOf(args.ClickedItem)].Move;

            FindSelectedScripts(start, end, out int startS, out int endS);
            if (notepad.SelectionLength == 0) 
            {
                level.ScriptsInsert(startS, new Script(command));
            }
            else
            {
                level.ScriptsRemove(startS, endS - startS);
                level.ScriptsInsert(startS - (endS - startS), new Script(command));
            }
            notepad.Text = ScriptsWrite();
            notepad.Select(end + notepad.Lines[endS].Length, 0);
        }

        private void NotepadResetClick(object sender, EventArgs args)
        {
            level.ScriptsClear();
            notepad.Text = ScriptsWrite();
        }

        private void NotepadRemoveItem()
        {
            var start = notepad.SelectionStart;
            var end = start + notepad.SelectionLength - 1;

            FindSelectedScripts(start, end, out int startS, out int endS);

            level.ScriptsRemove(startS, endS - startS);
            notepad.Text = ScriptsWrite();
            notepad.SelectionStart = start;
        }

        private void FindSelectedScripts(int start, int end, out int startS, out int endS)
        {
            var sum = 0;
            startS = -1;
            endS = -1;
            var str = notepad.Text.Split('\n');
            for (var i = 0; i < str.Length; i++)
            {
                if (i != 0) sum += 1;//1 = '\n'.Length
                if (startS == -1 && sum + str[i].Length >= start) startS = i;
                if (startS != -1 && sum + str[i].Length >= end) endS = i;
                sum += str[i].Length;
                if (endS != -1) break;
            }
        }

        private void AddButtonClick(object sender, EventArgs args)
        {
            if (contextMenu != null)
            {
                contextMenu.Show(addButton, new Point(addButton.Height, 0));
                return;
            }

            contextMenu = new ContextMenuStrip();
            for (var i = 0; i < level.openedScripts.Length; i++)
            {
                contextMenu.Items.Add(level.openedScripts[i].Declaration);
            }

            contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(AddButtonMenu_ItemClick);
            contextMenu.Show(addButton, new Point(addButton.Height, 0));
        }

        private void WindowResize()
        {
            WorkTableResize();

            var rows = level.map.GetLength(0);
            var columns = level.map.GetLength(1);
            float coeff = (float)gamePlace.Height / columns / 32;
            var imageSize = new SizeF(coeff, coeff) * 32;
            sizer = new Sizer(rows, columns, coeff, imageSize);

            SetPlayerWorldPositionAndSize(sizer);
            gamePlace.Invalidate();
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

        private void PieceUpdateFrame(object sender, EventArgs args)
        {
            pieceData.CurrentFrame++;
            gamePlace.Invalidate();
        }
    }
}
