
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
        public TableLayoutPanel menu;
        private Sizer sizer;
        private Image PlayButton;

        int indexToMove;
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

        #region Windows Form Designer generated code

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

        #endregion


        #region Windows Form Designer by my code

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

        private void InitializeDesign()
        {
            WorkTableCreate();

            Load += (sender, args) => OnSizeChanged(EventArgs.Empty);
            SizeChanged += (sender, args) => WindowResize();
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
            GamePlaceCreate();
            NotepadCreate();
            MenuCreate();

            workTable = new TableLayoutPanel();
            workTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            workTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            workTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            workTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));

            workTable.SetRowSpan(gamePlace, 2);
            workTable.Controls.Add(gamePlace, 0, 0);
            workTable.Controls.Add(notepad, 1, 0);
            workTable.Controls.Add(menu, 1, 1);

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
            gamePlace = new PictureBox();
            gamePlace.Dock = DockStyle.Fill;
            gamePlace.Padding = Padding.Empty;
            gamePlace.Margin = Padding.Empty;
            gamePlace.Paint += new PaintEventHandler(Painter);
        }

        private void NotepadCreate()
        {
            notepad = new ListBox();

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

        private void MenuCreate()
        {
            menu = new TableLayoutPanel();
            menu.Dock = DockStyle.Fill;
            menu.BackColor = Color.White;
            menu.Margin = Padding.Empty;
            if (PlayButton == default) 
                PlayButton = new Bitmap(Image.FromFile(Application.StartupPath + @"..\..\..\Resources\PlayButton.png"),
                    new Size(menu.Height, menu.Height));

            var playButton = new Button();
            playButton.Margin = Padding.Empty;
            playButton.Size = new Size(menu.Height, menu.Height);
            playButton.Image = PlayButton;
            menu.Controls.Add(playButton);

            playButton.Click += new EventHandler(PlayButtonClick);
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
            if (indexToMove == -1) return;
            //получаем перетаскиваемый элемент
            var item = scripts[indexToMove];
            object itemToMove = notepad.Items[indexToMove];
            if (newIndex == -1)
            {
                //удаляем элемент
                scripts.RemoveAt(indexToMove);
                notepad.Items.RemoveAt(indexToMove);
                //добавляем в конец списка
                scripts.Add(item);
                notepad.Items.Add(itemToMove);
            }
            //вставляем где-то в середину списка
            else if (indexToMove != newIndex)
            {
                //удаляем элемент
                scripts.RemoveAt(indexToMove);
                notepad.Items.RemoveAt(indexToMove);
                //вставляем в конкретную позицию
                scripts.Insert(newIndex, item);
                notepad.Items.Insert(newIndex, itemToMove);
            }
        }

        private void Notepad_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            Graphics g = e.Graphics;
            Brush brush = ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ?
                          Brushes.Black : new SolidBrush(e.BackColor);
            g.FillRectangle(brush, e.Bounds);
            var allString = notepad.Items[e.Index].ToString().Split('.');
            for(var i = 0; i < allString.Length; i++)
            {
                var st = e.Index + 1;
                e.Graphics.DrawString(st.ToString(), new Font(FontFamily.GenericMonospace, 12, FontStyle.Regular),
                     new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
                e.Graphics.DrawString(". " + allString[i], new Font(FontFamily.GenericMonospace, 12, FontStyle.Regular),
                     new SolidBrush(i == 0? Color.Blue: e.ForeColor), 
                     new RectangleF(e.Bounds.X * i + (i == 0 ? 0 : allString[i-1].Length) * 13, e.Bounds.Y , allString[i].Length * 16, e.Bounds.Height), 
                     StringFormat.GenericDefault);
            }
            
            e.DrawFocusRectangle();
        }

        #endregion
    }
}

