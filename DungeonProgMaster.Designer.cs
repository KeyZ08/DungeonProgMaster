
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    partial class DungeonProgMaster
    {
        public TableLayoutPanel workTable;
        public TableLayoutPanel gamePlace;
        public TableLayoutPanel notepad;
        public Bitmap groundImage;
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
            this.Name = "DungeonProgMaster";
            this.Text = "DungeonProgMaster";
            this.ResumeLayout(false);
        }

        #endregion


        #region Windows Form Designer by my code

        private void InitializeMyDesign()
        {
            groundImage = new Bitmap(Application.StartupPath + @"..\..\..\Resources\Ground.png");

            gamePlace = new TableLayoutPanel();
            gamePlace.Dock = DockStyle.Fill;
            gamePlace.BackColor = Color.White;
            gamePlace.Padding = Padding.Empty;
            gamePlace.Margin = Padding.Empty;

            gamePlace.Paint += (sender, args) =>
            {
                var gr = args.Graphics;
                var imageSize = new SizeF((float)gamePlace.Height / rowCount, (float)gamePlace.Width / columnCount);
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        gr.DrawImage(groundImage, 
                            new RectangleF(imageSize.Width * j, imageSize.Height * i, 
                                imageSize.Width+1, imageSize.Height+1), 
                            new RectangleF(0, 0, 32, 32), GraphicsUnit.Pixel);
                    }
                }
            };


            notepad = new TableLayoutPanel();
            notepad.Dock = DockStyle.Fill;
            notepad.BackColor = Color.Green;
            notepad.Margin = Padding.Empty;

            var menu = new TableLayoutPanel();
            menu.Dock = DockStyle.Fill;
            menu.BackColor = Color.White;
            menu.Margin = Padding.Empty;

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


            bool IsFirstResize = true;
            Load += (sender, args) => OnSizeChanged(EventArgs.Empty);
           
            SizeChanged += (sender, args) =>
            {
                WorkTableResize();
                //if (IsFirstResize) { this.MinimumSize = this.Size; IsFirstResize = false; };
                Invalidate();
            };
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
    }
}

