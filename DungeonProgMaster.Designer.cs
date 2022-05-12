
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

        private void PaintImage(Graphics gr, int tale, int x, int y)
        {
            gr.DrawImage(MapData.GetTale(tale),
                            new RectangleF(sizer.floorSize.Width * x, sizer.floorSize.Height * y,
                            sizer.floorSize.Width + 3f, sizer.floorSize.Height + 3f),
                            new RectangleF(PointF.Empty, MapData.GetTale(tale).Size), GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Отрисовывает всё игровое поле
        /// </summary>
        private void Painter(object sender, PaintEventArgs args)
        {
            var gr = args.Graphics;
            //карта
            for (int i = 0; i < sizer.columns; i++)
                for (int j = 0; j < sizer.rows; j++)
                {
                    if (level.map[j, i] != (int)MapData.Tales.Blank)
                        PaintImage(gr, 1, i, j);
                    else PaintImage(gr, 0, i, j);

                    if (level.map[j, i] == (int)MapData.Tales.Finish)
                        PaintImage(gr, 2, i, j);
                }
            var player = level.player;
            //игрок
            gr.DrawImage(player.anim[player.currentFrame], new RectangleF(player.worldPosition, player.worldSize),
                 new RectangleF(PointF.Empty, player.anim[player.currentFrame].Size), GraphicsUnit.Pixel);
        }

        private void WindowResize()
        {
            WorkTableResize();

            var rows = level.map.GetLength(0);
            var columns = level.map.GetLength(1);
            float coeff = (float)gamePlace.Height / columns / 32;
            var imageSize = new SizeF(coeff, coeff) * 32;
            sizer = new Sizer(rows, columns, coeff, imageSize);

            level.player.SetWorldPositionAndSize(sizer);
            gamePlace.Invalidate();
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

        private void GamePlaceCreate()
        {
            gamePlace.Dock = DockStyle.Fill;
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

        private void MenuCreate()
        {
            menu.Dock = DockStyle.Fill;
            menu.BackColor = Color.White;
            menu.Margin = Padding.Empty;
            addButton = CreateStandartMenuButton("AddButton", "Добавить элемент", new EventHandler(AddButtonClick));
            playButton = CreateStandartMenuButton("PlayButton", "Запустить алгоритм", new EventHandler(PlayButtonClick));
            notepadReset = CreateStandartMenuButton("NotepadResetButton", "Очистить алгоритм", new EventHandler(NotepadResetClick));
            notepadItemRemove = CreateStandartMenuButton("RemoveItem", "Удалить выделенную строку", new EventHandler(NotepadRemoveItem));
        }

        private Button CreateStandartMenuButton(string name, string toolTip, EventHandler handler)
        {
            var button = new Button();
            button.Margin = Padding.Empty;
            var toolTip1 = new ToolTip();
            toolTip1.SetToolTip(button, "Добавить элемент");
            button.Location = new Point(menu.Height, 0);
            button.Size = new Size(menu.Height, menu.Height);
            button.BackgroundImage = new Bitmap(Image.FromFile(Application.StartupPath + @"..\..\..\Resources\" + name + ".png"),
                    new Size(menu.Height, menu.Height));
            button.BackgroundImageLayout = ImageLayout.Zoom;
            button.Click += handler;
            menu.Controls.Add(button);
            return button;
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
                addButton_contextMenu.Items.Add(Sketches.sketches[(PlayerMove)i]);

            addButton_contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(AddButtonMenu_ItemClick);
            addButton_contextMenu.Show(addButton, new Point(addButton.Height,0));
        }

        #endregion
    }
}

