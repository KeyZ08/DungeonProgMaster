
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DungeonProgMaster
{
    partial class DungeonProgMaster
    {
        public TableLayoutPanel workTable;
        public PictureBox gamePlace;
        public RichTextBox notepad;
        public FlowLayoutPanel menu;
        private Sizer sizer;
        private Button playButton;
        private Button addButton;
        private Button notepadReset;
        private ContextMenuStrip addButton_contextMenu;

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
                for (int j = 0; j < sizer.rows; j++)
                {
                    if (level.map[j, i] != (int)MapData.Tales.Blank)
                        PaintMapImage(gr, 1, i, j);
                    else PaintMapImage(gr, 0, i, j);

                    if (level.map[j, i] == (int)MapData.Tales.Finish)
                        PaintMapImage(gr, 2, i, j);
                }

            //монетки
            var worldSize = sizer.GetWorldSize(new Size(32, 32));
            for (var i = 0; i < level.pieces.Count; i++)
            {
                var pos = level.pieces[i];
                if (level.pickedPieces.Contains(pos)) continue;
                gr.DrawImage(pieceData.Frames[pieceData.CurrentFrame], new RectangleF(sizer.GetWorldPosition(new PointF(pos.X + 0.05f, pos.Y + 0.1f), worldSize), worldSize),
                    new RectangleF(PointF.Empty, pieceData.Frames[pieceData.CurrentFrame].Size), GraphicsUnit.Pixel);
            }

            //игрок
            var player = level.player;
            gr.DrawImage(player.anim[player.currentFrame], new RectangleF(WorldPlayerPosition, WorldPlayerSize),
                 new RectangleF(PointF.Empty, player.anim[player.currentFrame].Size), GraphicsUnit.Pixel);
            
        }

        private void PaintMapImage(Graphics gr, int tale, int x, int y)
        {
            gr.DrawImage(MapData.GetTale(tale),
                            new RectangleF(sizer.floorSize.Width * x, sizer.floorSize.Height * y,
                            sizer.floorSize.Width + 3f, sizer.floorSize.Height + 3f),
                            new RectangleF(PointF.Empty, MapData.GetTale(tale).Size), GraphicsUnit.Pixel);
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

        private void WorkTableCreate()
        {
            workTable = new TableLayoutPanel();
            workTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            workTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            workTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            workTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));

            gamePlace = new PictureBox();
            notepad = new RichTextBox();
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
            notepad.Multiline = true;
            notepad.AcceptsTab = true;
            notepad.WordWrap = false;
            notepad.ReadOnly = true;
            notepad.ScrollBars = RichTextBoxScrollBars.Both;
            notepad.Font = new Font(FontFamily.GenericSansSerif, 12f,FontStyle.Regular);
            notepad.TextChanged += NotepadTextChanged;
            notepad.KeyDown += OnKeyDownNotepad;
            notepad.Text = ScriptsWrite();
        }

        private void NotepadTextChanged(object sender, EventArgs args)
        {
            var oldSelect = notepad.SelectionStart;
            var start = WordFind("Player", notepad.Text);
            if (start.Count == 0) return;
            for (var i = 0; i < start.Count; i++)
            {
                notepad.Select(start[i], 6);
                notepad.SelectionColor = Color.LightSkyBlue;
            }
            notepad.Select(oldSelect, 0);
        }

        /// <summary>
        /// Находит все позиции word в sender
        /// </summary>
        /// <param name="word">Слово, позиции которого нужно найти</param>
        /// <param name="sender">Строка в которой ищем</param>
        /// <returns>Все позиции слова в строке</returns>
        private List<int> WordFind(string word, string sender)
        {
            var result = new List<int>();
            for (var i = 0; i < sender.Length; i++)
            {
                if (sender.Length - i < word.Length) break;
                if (sender[i] == word[0])
                {
                    var isWord = true;
                    for (var j = 0; j < word.Length; j++)
                        if (word[j] != sender[i + j]) { isWord = false; break; };
                    if (isWord) result.Add(i);
                }
            }
            return result;
        }

        /// <summary>
        /// Задает отображение скриптов
        /// </summary>
        /// <returns>Строка для отображения в notepad</returns>
        private string ScriptsWrite()
        {
            var str = new System.Text.StringBuilder();
            var num = 0;
            foreach (var i in level.scripts)
            {
                var transfer = "";
                if (num != 0) transfer += "\r\n";
                str.Append($"{transfer}  {num}. {i.Sketch}");
                num++;
            }
            if (str.Length == 0) return "  0.";
            return str.ToString();
        }

        private void MenuCreate()
        {
            menu.Dock = DockStyle.Fill;
            menu.BackColor = Color.White;
            menu.Margin = Padding.Empty;
            addButton = CreateStandartMenuButton("AddButton", "Добавить элемент", new EventHandler(AddButtonClick));
            playButton = CreateStandartMenuButton("PlayButton", "Запустить алгоритм", new EventHandler(PlayButtonClick));
            notepadReset = CreateStandartMenuButton("NotepadResetButton", "Очистить алгоритм", new EventHandler(NotepadResetClick));
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

        #endregion
    }
}

