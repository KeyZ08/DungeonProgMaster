using System;
using System.Drawing;
using Timers = System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    public partial class DungeonProgMaster : Form
    {
        Level level;

        private PointF WorldPlayerPosition;
        private SizeF WorldPlayerSize;
        //скорость анимации
        private Timers.Timer pieceAnimator;
        private Piece pieceData;

        public DungeonProgMaster()
        {
            InitializeComponent();
            InitializeDesign();
            level = Levels.GetLevel(0);

            pieceData = new Piece();
            pieceAnimator = new Timers.Timer(100);
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

        #region Player

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
                if (level.pieces.Contains(player.targetPosition) && !level.pickedPieces.Contains(player.targetPosition))
                    level.pickedPieces.Add(player.targetPosition);
                player.isAnimated = false;
                (sender as Timers.Timer).Dispose();
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
                SetPlayerWorldPositionAndSize(sizer);
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
                    notepad.BeginInvoke(new Action(() => level.ScriptsClear(notepad)));
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
                notepad.BeginInvoke(new Action(() => notepad.Enabled = false));

                for (var i = 0; i < level.ScriptCount; i++)
                {
                    if (level.player.isAnimated) { i--; continue; }
                    level.player.isAnimated = true;
                    Commands.commands[level.GetScript(i).Move].Invoke(level.player);
                    //выделяет исполняемую строку
                    notepad.BeginInvoke(new Action(() => notepad.SelectedIndex = i - 1));
                    if (!WatсhOnTarget())
                    {
                        SetEnabledControls(true, menu.Controls);
                        notepad.BeginInvoke(new Action(() => notepad.Enabled = true));
                        return;
                    }
                    var playerAnimator = level.GetScript(i).Move == Command.Rotate? new Timers.Timer(150) : new Timers.Timer(100);
                    playerAnimator.Elapsed += PlayerMovement;
                    playerAnimator.Start();
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
            var move = (Command)((ToolStrip)sender).Items.IndexOf(item);
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

        private void Notepad_MouseMove(object sender, MouseEventArgs e)
        {
            //если нажата левая кнопка мыши, начинаем Drag&Drop
            if (e.Button == MouseButtons.Left)
            {
                //индекс элемента, который мы перемещаем
                indexToMove = notepad.IndexFromPoint(e.X, e.Y);
                notepad.DoDragDrop(indexToMove, DragDropEffects.Move);
            }
        }

        private void Notepad_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void Notepad_DragDrop(object sender, DragEventArgs e)
        {
            int newIndex = notepad.IndexFromPoint(notepad.PointToClient(new Point(e.X, e.Y)));
            //если вставка происходит в начало списка
            if (indexToMove == -1 || indexToMove == 65535) return;
            //получаем перетаскиваемый элемент
            var item = level.GetScript(indexToMove);
            object itemToMove = notepad.Items[indexToMove];
            if (newIndex == -1)
            {
                //удаляем элемент
                level.ScriptRemoveAt(indexToMove, notepad);
                //добавляем в конец списка
                level.ScriptAdd(item, notepad);
            }
            //вставляем где-то в середину списка
            else if (indexToMove != newIndex)
            {
                //удаляем элемент
                level.ScriptRemoveAt(indexToMove, notepad);
                //вставляем в конкретную позицию
                level.ScriptInsert(newIndex, item, notepad);
            }
        }

        private void Notepad_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;
            e.DrawBackground();

            Graphics g = e.Graphics;
            Brush brush = ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ?
                          Brushes.DarkSlateGray : new SolidBrush(e.BackColor);
            g.FillRectangle(brush, e.Bounds);
            var allString = notepad.Items[e.Index].ToString().Split('.');
            for (var i = 0; i < allString.Length; i++)
            {
                var st = (e.Index + 1).ToString();
                e.Graphics.DrawString(st, new Font(FontFamily.GenericMonospace, 12, FontStyle.Regular),
                     new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);

                e.Graphics.DrawString("." + allString[i], new Font(FontFamily.GenericMonospace, 12, FontStyle.Regular),
                     new SolidBrush(i == 0 ? Color.SkyBlue : e.ForeColor),
                     new RectangleF(st.Length * 12 + e.Bounds.X * i + (i == 0 ? 0 : allString[i - 1].Length) * 12, e.Bounds.Y, allString[i].Length * 16, e.Bounds.Height),
                     StringFormat.GenericDefault);
            }

            e.DrawFocusRectangle();
        }

        private void AddButtonClick(object sender, EventArgs args)
        {
            if (addButton_contextMenu != null)
            {
                addButton_contextMenu.Show(addButton, new Point(addButton.Height, 0));
                return;
            }

            addButton_contextMenu = new ContextMenuStrip();
            for (var i = 0; i < 2; i++)
                addButton_contextMenu.Items.Add(Sketches.sketches[(Command)i]);

            addButton_contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(AddButtonMenu_ItemClick);
            addButton_contextMenu.Show(addButton, new Point(addButton.Height, 0));
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
