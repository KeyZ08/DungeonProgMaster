
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    partial class DungeonProgMaster
    {
        public TableLayoutPanel table;
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
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.MinimumSize = new Size(800, 439);
            this.ClientSize = new Size(800, 440);
            this.Name = "DungeonProgMaster";
            this.Text = "DungeonProgMaster";
            this.ResumeLayout(false);
        }

        #endregion


        #region Windows Form Designer by my code

        private void InitializeMyDesign()
        {
            var ground = new PictureBox() { Image = Image.FromFile(Application.StartupPath + @"..\..\..\Resources\Ground.png") };
            ground.Dock = DockStyle.Fill;
            ground.SizeMode = PictureBoxSizeMode.Zoom;
            ground.Margin = Padding.Empty;

            gamePlace = new TableLayoutPanel();
            for (int i = 0; i < rowCount; i++)
                gamePlace.RowStyles.Add(new RowStyle(SizeType.Percent, 100/rowCount));
            for (int i = 0; i < columnCount; i++)
                gamePlace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / columnCount));
            gamePlace.Controls.Add(ground, rowCount-1, columnCount-1);
            //gamePlace.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            gamePlace.Dock = DockStyle.Fill;
            gamePlace.BackColor = Color.Blue;
            gamePlace.Padding = Padding.Empty;
            gamePlace.Margin = Padding.Empty;
            

            notepad = new TableLayoutPanel();
            notepad.Dock = DockStyle.Fill;
            notepad.BackColor = Color.Green;
            notepad.Margin = Padding.Empty;

            var menu = new TableLayoutPanel();
            menu.Dock = DockStyle.Fill;
            menu.BackColor = Color.White;
            menu.Margin = Padding.Empty;

            table = new TableLayoutPanel();
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));

            table.SetRowSpan(gamePlace, 2);
            table.Controls.Add(gamePlace, 0, 0);
            table.Controls.Add(notepad, 1, 0);
            table.Controls.Add(menu, 1, 1);
            table.Dock = DockStyle.Fill;

            Controls.Add(table);
           
            

            FormClosing += (sender, eventArgs) =>
            {
                var result = MessageBox.Show("Действительно закрыть?", "",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    eventArgs.Cancel = true;
            };

            Load += (sender, args) => OnSizeChanged(EventArgs.Empty);
           
            SizeChanged += (sender, args) =>
            {
            };
        }


        /*protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawLine(new Pen(Color.Black, 5), 0, 0, 50, 100);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawLine(new Pen(Color.Red, 5), 0, 0, 50, 150);
            g.FillRectangle(Brushes.Green, 100, 100, 100, 100);
            g.DrawString("Hello", new Font("Arial", 16), Brushes.Black, 
                new Rectangle(100,100,100,100),
                new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.FitBlackBox});
            
        }*/

        #endregion
    }
}

