using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        Game game;
        int cell_size;
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            game = new Game();
            cell_size = 25;
            timer1.Interval = game.cur_speed;
            timer1.Start();
            game.Merge();
            pictureBox1.Invalidate();
            label1.Text = "Level";
            label3.Text = "Score";
        }
        private void DrawMap(Graphics g)
        {
            // Draw green border of game field
            g.DrawLine(Pens.Green, new Point(cell_size, cell_size),
                new Point(cell_size, cell_size + game.height * cell_size));
            g.DrawLine(Pens.Green, new Point(cell_size, cell_size),
                new Point(cell_size + game.width * cell_size, cell_size));
            g.DrawLine(Pens.Green, new Point(cell_size + game.width * cell_size, cell_size),
                new Point(cell_size + game.width * cell_size, cell_size + game.height * cell_size));
            g.DrawLine(Pens.Green, new Point(cell_size, cell_size + game.height * cell_size),
                new Point(cell_size + game.width * cell_size, cell_size + game.height * cell_size));

            // Draw figures' cells
            for (int i = 0; i < game.height; i++) {
                for (int j = 0; j < game.width; j++) {
                    if (game.map[i, j] == 1)
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(245, 96, 66)),
                            cell_size + j * cell_size + 1, cell_size + i * cell_size + 1, cell_size - 1, cell_size - 1);
                    }
                    
                }
            }
        }
        private void ShowNextMatrix(Graphics g)
        {
            for(int i = 0; i < game.next_figure.size; i++)
            {
                for(int j = 0; j < game.next_figure.size; j++)
                {
                    if(game.next_figure.matrix[i, j] == 1)
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(245, 96, 66)),
                            (game.width + 5) * cell_size + j * cell_size + 1,
                            cell_size + cell_size / 2 + i * cell_size + 1, cell_size - 1, cell_size - 1);
                    }
                }
            }
        }
        private void WriteScore()
        {
            label2.Text = game.level.ToString();
            label4.Text = game.score.ToString();
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            timer1.Interval = game.cur_speed;
            DrawMap(e.Graphics);
            ShowNextMatrix(e.Graphics);
            WriteScore();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            game.ResetArea();
            bool game_over = game.MoveDown();
            if(game_over)
            {
                timer1.Stop();
                if (MessageBox.Show("Start again?", "Attention!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Init();
                }
                else
                {
                    Application.Exit();
                }
            }
            game.Merge();
            pictureBox1.Invalidate();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            game.ResetArea();
            if (e.KeyCode.ToString() == "W")
            {
                game.Rotate();
            }
            else if (e.KeyCode.ToString() == "A")
            {
                game.MoveLeft();
            }
            else if (e.KeyCode.ToString() == "D")
            {
                game.MoveRight();
            }
            else if (e.KeyCode.ToString() == "S")
            {
                game.MoveDown(1);
            }
            game.Merge();
            if (e.KeyCode.ToString() == "W" ||
                    e.KeyCode.ToString() == "A" ||
                    e.KeyCode.ToString() == "D" ||
                    e.KeyCode.ToString() == "S")
            {
                pictureBox1.Invalidate();
            }
        }
    }
}