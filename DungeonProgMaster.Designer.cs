
using System;
using System.Collections.Generic;
using System.Drawing;
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
        /// Отрисовывает всё
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
                        new RectangleF(sizer.floorSize.Width * i, sizer.floorSize.Height * j, sizer.floorSize.Width + 3f, sizer.floorSize.Height + 3f),
                        new RectangleF(PointF.Empty, MapData.GetTale(map.map[j, i]).Size), GraphicsUnit.Pixel);
                }
            }

            //игрок
            gr.DrawImage(player.anim[player.currentFrame], new RectangleF(player.worldPosition, player.worldSize),
                 new RectangleF(PointF.Empty, player.anim[player.currentFrame].Size), GraphicsUnit.Pixel);
        }

        private void InitializeMyDesign()
        {
            GamePlaceCreate();
            NotepadAndMenuCreate();
            WorkTableCreate();

            Load += (sender, args) => OnSizeChanged(EventArgs.Empty);
            SizeChanged += (sender, args) => WindowResize();
        }

        private void NotepadAndMenuCreate()
        {
            notepad = new ListBox();

            var scripts = new List<Script>() 
            { 
                new Script(PlayerMove.Right), 
                new Script(PlayerMove.Top), 
            };
            notepad.DataSource = scripts;
            notepad.DisplayMember = "Sketch";
            notepad.ValueMember = "Move";

            notepad.Dock = DockStyle.Fill;
            notepad.BorderStyle = BorderStyle.None;
            notepad.Margin = Padding.Empty;
            notepad.BackColor = Color.Green;

            menu = new TableLayoutPanel();
            menu.Dock = DockStyle.Fill;
            menu.BackColor = Color.White;
            menu.Margin = Padding.Empty;
        }

        private void WorkTableCreate()
        {
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

        private void GamePlaceCreate()
        {
            gamePlace = new PictureBox();
            gamePlace.Dock = DockStyle.Fill;
            gamePlace.Padding = Padding.Empty;
            gamePlace.Margin = Padding.Empty;
            gamePlace.Paint += new PaintEventHandler(Painter);
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
    }
}

