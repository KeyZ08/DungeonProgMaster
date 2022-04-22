
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
        
        private void PlayerAnimation(Graphics gr, float coeff, SizeF imageSize)
        {
            var anim = player.PlayerMovement(movement);
            var center = 64 * coeff / 2;
            var inWorldPosition = new PointF(imageSize.Width * player.position.X + (-center + imageSize.Width / 3),
                imageSize.Height * player.position.Y - center);
            var inWorldSize = new SizeF(64 * coeff * 1.2f, 64 * coeff * 1.2f);
            
            var i = currentFrame == 0? 0: currentFrame++;
            if (currentFrame >= anim.Count) currentFrame = 1;
            gr.DrawImage(anim[i],
                    new RectangleF(inWorldPosition, inWorldSize),
                    new RectangleF(PointF.Empty, anim[i].Size), GraphicsUnit.Pixel);
        }

        private void CreateMap(Graphics gr, int rows, int columns, SizeF imageSize)
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    gr.DrawImage(MapData.GetTale(map[j,i]),
                        new RectangleF(imageSize.Width * i, imageSize.Height * j, imageSize.Width + 3f, imageSize.Height + 3f),
                        new RectangleF(PointF.Empty, MapData.GetTale(map[j, i]).Size), GraphicsUnit.Pixel);
                }
            }
        }

        private void InitializeMyDesign()
        {
            gamePlace = new TableLayoutPanel();
            gamePlace.Dock = DockStyle.Fill;
            gamePlace.Padding = Padding.Empty;
            gamePlace.Margin = Padding.Empty;

            gamePlace.Paint += (sender, args) =>
            {
                
                
                
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


            Load += (sender, args) => OnSizeChanged(EventArgs.Empty);
           
            SizeChanged += (sender, args) =>
            {
                WorkTableResize();
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

