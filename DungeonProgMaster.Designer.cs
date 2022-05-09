
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    partial class DungeonProgMaster
    {
        public TableLayoutPanel workTable;
        public PictureBox gamePlace;
        public ListBox notepad;
        public FlowLayoutPanel menu;
        private Sizer sizer;
        private Button playButton;
        private Button addButton;
        private Button notepadReset;
        private Button notepadItemRemove;
        private ContextMenuStrip addButton_contextMenu;
        private int indexToMove;

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DungeonProgMaster
            // 
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 550);
            this.MinimumSize = new Size(1000 + 16, 550 + 39);
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.Name = "DungeonProgMaster";
            this.Text = "DungeonProgMaster";
            this.ResumeLayout(false);
        }

        #region Windows Form Designer by my code

        private void InitializeDesign()
        {
            WorkTableCreate();

            Load += (sender, args) => OnSizeChanged(EventArgs.Empty);
            SizeChanged += (sender, args) => WindowResize();
        }

        /// <summary>
        /// Отрисовывает всё игровое поле
        /// </summary>
        private void Painter(object sender, PaintEventArgs args)
        {
            var gr = args.Graphics;
            //карта
            for (int i = 0; i < sizer.columns; i++)
            {
                for (int j = 0; j < sizer.rows; j++)
                {
                    gr.DrawImage(MapData.GetTale(map.map[j, i]),
                        new RectangleF(sizer.floorSize.Width * i, sizer.floorSize.Height * j,
                        sizer.floorSize.Width + 3f, sizer.floorSize.Height + 3f),
                        new RectangleF(PointF.Empty, MapData.GetTale(map.map[j, i]).Size), GraphicsUnit.Pixel);
                }
            }

            //игрок
            gr.DrawImage(player.anim[player.currentFrame], new RectangleF(player.worldPosition, player.worldSize),
                 new RectangleF(PointF.Empty, player.anim[player.currentFrame].Size), GraphicsUnit.Pixel);
        }

        private void WindowResize()
        {
            WorkTableResize();

            var rows = map.map.GetLength(0);
            var columns = map.map.GetLength(1);
            float coeff = (float)gamePlace.Height / columns / 32;
            var imageSize = new SizeF(coeff, coeff) * 32;
            sizer = new Sizer(rows, columns, coeff, imageSize);

            player.SetWorldPositionAndSize(sizer);
            gamePlace.Invalidate();
        }

        #endregion

        #region Create All Panels

        private void WorkTableCreate()
        {
            workTable = new TableLayoutPanel();
            workTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            workTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            workTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            workTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));

            gamePlace = new PictureBox();
            notepad = new ListBox();
            menu = new FlowLayoutPanel();

            workTable.SetRowSpan(gamePlace, 2);
            workTable.Controls.Add(gamePlace, 0, 0);
            workTable.Controls.Add(notepad, 1, 0);
            workTable.Controls.Add(menu, 1, 1);

            GamePlaceCreate();
            NotepadCreate();
            MenuCreate();

            Controls.Add(workTable);
        }

        private void WorkTableResize()
        {
            var hcoeff = ClientSize.Height / 440d;
            var wcoeff = ClientSize.Width / 800d;
            var coeff = hcoeff < wcoeff ? hcoeff : wcoeff;
            var widht = (int)(coeff * 800);
            var height = widht * 55 / 100;
            workTable.Size = new Size(widht, height);
        }

        private void GamePlaceCreate()
        {
            gamePlace.Dock = DockStyle.Fill;
            gamePlace.Padding = Padding.Empty;
            gamePlace.Margin = Padding.Empty;
            gamePlace.Paint += new PaintEventHandler(Painter);
        }

        private void NotepadCreate()
        {
            notepad.Dock = DockStyle.Fill;
            notepad.BorderStyle = BorderStyle.None;
            notepad.Margin = Padding.Empty;
            notepad.BackColor = Color.Black;
            notepad.ForeColor = Color.White;
            notepad.ItemHeight = 25;
            notepad.AllowDrop = true;
            notepad.DrawMode = DrawMode.OwnerDrawVariable;

            notepad.MouseMove += new MouseEventHandler(Notepad_MouseMove);
            notepad.DragEnter += new DragEventHandler(Notepad_DragEnter);
            notepad.DragDrop += new DragEventHandler(Notepad_DragDrop);
            notepad.DrawItem += new DrawItemEventHandler(Notepad_DrawItem);
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
            //индекс, куда перемещаем
            //listBox1.PointToClient(new Point(e.X, e.Y)) - необходимо
            //использовать поскольку в e храниться
            //положение мыши в экранных коородинатах, а эта
            //функция позволяет преобразовать в клиентские
            int newIndex = notepad.IndexFromPoint(notepad.PointToClient(new Point(e.X, e.Y)));
            //если вставка происходит в начало списка
            if (indexToMove == -1 || indexToMove == 65535) return;
            //получаем перетаскиваемый элемент
            var item = map.scripts[indexToMove];
            object itemToMove = notepad.Items[indexToMove];
            if (newIndex == -1)
            {
                //удаляем элемент
                map.scripts.RemoveAt(indexToMove);
                notepad.Items.RemoveAt(indexToMove);
                //добавляем в конец списка
                map.scripts.Add(item);
                notepad.Items.Add(itemToMove);
            }
            //вставляем где-то в середину списка
            else if (indexToMove != newIndex)
            {
                //удаляем элемент
                map.scripts.RemoveAt(indexToMove);
                notepad.Items.RemoveAt(indexToMove);
                //вставляем в конкретную позицию
                map.scripts.Insert(newIndex, item);
                notepad.Items.Insert(newIndex, itemToMove);
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
                var st = e.Index + 1;
                e.Graphics.DrawString(st.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Regular),
                     new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
                e.Graphics.DrawString(". " + allString[i], new Font(FontFamily.GenericMonospace, 12, FontStyle.Regular),
                     new SolidBrush(i == 0 ? Color.SkyBlue : e.ForeColor),
                     new RectangleF(e.Bounds.X * i + (i == 0 ? 0 : allString[i - 1].Length) * 13, e.Bounds.Y, allString[i].Length * 16, e.Bounds.Height),
                     StringFormat.GenericDefault);
            }

            e.DrawFocusRectangle();
        }

        private void MenuCreate()
        {
            menu.Dock = DockStyle.Fill;
            menu.BackColor = Color.White;
            menu.Margin = Padding.Empty;
            AddButtonCreate();
            PlayButtonCreate();
            NotepadResetButton();
            NotepadItemRemove();
        }

        private void NotepadItemRemove()
        {
            notepadItemRemove = new Button();
            notepadItemRemove.Margin = Padding.Empty;
            var toolTip3 = new ToolTip();
            toolTip3.SetToolTip(notepadItemRemove, "Удалить выделенную строку");
            notepadItemRemove.Size = new Size(menu.Height, menu.Height);
            notepadItemRemove.BackgroundImage = new Bitmap(Image.FromFile(Application.StartupPath + @"..\..\..\Resources\RemoveItem.png"),
                    new Size(menu.Height, menu.Height));
            notepadItemRemove.BackgroundImageLayout = ImageLayout.Zoom;
            notepadItemRemove.Click += new EventHandler(NotepadRemoveItem);
            menu.Controls.Add(notepadItemRemove);
        }

        private void NotepadResetButton()
        {
            notepadReset = new Button();
            notepadReset.Margin = Padding.Empty;
            var toolTip3 = new ToolTip();
            toolTip3.SetToolTip(notepadReset, "Очистить алгоритм");
            notepadReset.Size = new Size(menu.Height, menu.Height);
            notepadReset.BackgroundImage = new Bitmap(Image.FromFile(Application.StartupPath + @"..\..\..\Resources\NotepadResetButton.png"),
                    new Size(menu.Height, menu.Height));
            notepadReset.BackgroundImageLayout = ImageLayout.Zoom;
            notepadReset.Click += new EventHandler(NotepadResetClick);
            menu.Controls.Add(notepadReset);
        }

        private void PlayButtonCreate()
        {
            playButton = new Button();
            playButton.Margin = Padding.Empty;
            var toolTip2 = new ToolTip();
            toolTip2.SetToolTip(playButton, "Запустить алгоритм");
            playButton.Size = new Size(menu.Height, menu.Height);
            playButton.BackgroundImage = new Bitmap(Image.FromFile(Application.StartupPath + @"..\..\..\Resources\PlayButton.png"),
                    new Size(menu.Height, menu.Height));
            playButton.BackgroundImageLayout = ImageLayout.Zoom;
            playButton.Click += new EventHandler(PlayButtonClick);
            menu.Controls.Add(playButton);
        }

        private void AddButtonCreate()
        {
            addButton = new Button();
            addButton.Margin = Padding.Empty;
            var toolTip1 = new ToolTip();
            toolTip1.SetToolTip(addButton, "Добавить элемент");
            addButton.Location = new Point(menu.Height, 0);
            addButton.Size = new Size(menu.Height, menu.Height);
            addButton.BackgroundImage = new Bitmap(Image.FromFile(Application.StartupPath + @"..\..\..\Resources\AddButton.png"),
                    new Size(menu.Height, menu.Height));
            addButton.BackgroundImageLayout = ImageLayout.Zoom;
            addButton.Click += new EventHandler(AddButtonClick);
            menu.Controls.Add(addButton);
        }

        private void AddButtonClick(object sender, EventArgs args)
        {
            if (addButton_contextMenu != null)
            {
                addButton_contextMenu.Show(addButton, new Point(addButton.Height, 0));
                return;
            }

            addButton_contextMenu = new ContextMenuStrip();
            for (var i = 0; i < 4; i++)
                addButton_contextMenu.Items.Add(Sketches.sketches[(PlayerMove)i]);

            addButton_contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(AddButtonMenu_ItemClick);
            addButton_contextMenu.Show(addButton, new Point(addButton.Height,0));
        }

        #endregion
    }
}

