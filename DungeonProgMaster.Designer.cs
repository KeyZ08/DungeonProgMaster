
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    partial class DungeonProgMaster
    {
        public TableLayoutPanel table;
        public TableLayoutPanel game;
        public TableLayoutPanel console;
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
            this.ClientSize = new Size(1200, 650);
            this.Name = "DungeonProgMaster";
            this.Text = "DungeonProgMaster";
            this.ResumeLayout(false);
        }

        #endregion


        #region Windows Form Designer by my code

        private void InitializeMyDesign()
        {
            game = new TableLayoutPanel();
            game.Dock = DockStyle.Fill;
            game.BackColor = Color.Blue;

            console = new TableLayoutPanel();
            console.Dock = DockStyle.Fill;
            console.BackColor = Color.Green;

            var output = new TableLayoutPanel();
            output.Dock = DockStyle.Fill;
            output.BackColor = Color.White;

            table = new TableLayoutPanel();
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            table.SetRowSpan(game, 2);
            table.Controls.Add(game, 0, 0);
            table.Controls.Add(console, 1, 0);
            table.Controls.Add(output, 1, 1);
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


        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawLine(new Pen(Color.Black, 5), 0, 0, 50, 100);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.DrawLine(new Pen(Color.Red, 5), 0, 0, 50, 150);
            g.FillRectangle(Brushes.Green, 100, 100, 100, 100);
            g.DrawString("Hello", new Font("Arial", 16), Brushes.Black, 
                new Rectangle(100,100,100,100),
                new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.FitBlackBox});
            
        }

        #endregion
    }
}

