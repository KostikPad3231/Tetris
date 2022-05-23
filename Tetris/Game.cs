using System;

namespace Tetris
{
    public class Game
    {
        private Creator figure_creator;
        public int width { get { return 8; } }
        public int height { get { return 16; } }
        public Figure cur_figure { get; private set; }
        public Figure next_figure { get; private set; }
        public int[,] map { get; private set; }
        public int norm_speed { get; private set; }
        public int cur_speed { get; private set; }
        public int score { get; private set; }
        public int lines { get; private set; }
        public int level { get; private set; }
        public Game()
        {
            cur_speed = 1000;
            norm_speed = 1000;
            score = 0;
            lines = 0;
            level = 1;
            map = new int[height, width];
            cur_figure = GenerateFigure();
            next_figure = GenerateFigure();
        }
        // Transferring the figure to the map
        public void Merge()
        {
            for (int i = cur_figure.Y; i < cur_figure.Y + cur_figure.size; i++)
            {
                for (int j = cur_figure.X; j < cur_figure.X + cur_figure.size; j++)
                {
                    if (i < height && j < width && cur_figure.matrix[i - cur_figure.Y, j - cur_figure.X] != 0)
                    {
                        map[i, j] = cur_figure.matrix[i - cur_figure.Y, j - cur_figure.X];
                    }
                }
            }
        }
        // When moving a figure, remove its previous position from the map
        public void ResetArea()
        {
            for (int i = cur_figure.Y; i < cur_figure.Y + cur_figure.size; i++)
            {
                for (int j = cur_figure.X; j < cur_figure.X + cur_figure.size; j++)
                {
                    if (i >= 0 && j >= 0 && i < height && j < width)
                    {
                        if (cur_figure.matrix[i - cur_figure.Y, j - cur_figure.X] != 0)
                        {
                            map[i, j] = 0;
                        }
                    }
                }
            }
        }
        public void Rotate()
        {
            cur_speed = norm_speed;
            if (!IntersectCollision())
            {
                cur_figure.Rotate();
            }
        }
        public void MoveRight()
        {
            cur_speed = norm_speed;
            if (!HorizontalCollision(1))
            {
                cur_figure.MoveRight();
            }
        }
        public void MoveLeft()
        {
            cur_speed = norm_speed;
            if (!HorizontalCollision(-1))
            {
                cur_figure.MoveLeft();
            }
        }
        public bool MoveDown(int n = 0)
        {
            if (!VerticalCollision())
            {
                if (n == 1)
                {
                    cur_speed = 50;
                }
                cur_figure.MoveDown();
            }
            else
            {
                cur_speed = norm_speed;
                Merge();
                SliceMap();
                cur_figure = next_figure;
                next_figure = GenerateFigure();
                // Game over
                if (VerticalCollision())
                {
                    return true;
                }
            }
            return false;
        }
        // Check if figure can move down
        private bool VerticalCollision()
        {
            for (int i = cur_figure.Y + cur_figure.size - 1; i >= cur_figure.Y; i--)
            {
                for (int j = cur_figure.X; j < cur_figure.X + cur_figure.size; j++)
                {
                    if (cur_figure.matrix[i - cur_figure.Y, j - cur_figure.X] != 0)
                    {
                        if (i + 1 == height)
                        {
                            return true;
                        }
                        else if (map[i + 1, j] != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        // Check if figure can move left or right according to the direction (dir)
        private bool HorizontalCollision(int dir)
        {
            for (int i = cur_figure.Y; i < cur_figure.Y + cur_figure.size; i++)
            {
                for (int j = cur_figure.X; j < cur_figure.X + cur_figure.size; j++)
                {
                    if (cur_figure.matrix[i - cur_figure.Y, j - cur_figure.X] != 0)
                    {
                        if (j + dir > width - 1 || j + dir < 0)
                        {
                            return true;
                        }
                        if (map[i, j + dir] != 0)
                        {
                            if (j - cur_figure.X + dir >= cur_figure.size || j - cur_figure.X + dir < 0)
                            {
                                return true;
                            }
                            if (cur_figure.matrix[i - cur_figure.Y, j - cur_figure.X + dir] == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        // Check if figure can rotate
        private bool IntersectCollision()
        {
            for (int i = cur_figure.Y; i < cur_figure.Y + cur_figure.size; i++)
            {
                for (int j = cur_figure.X; j < cur_figure.X + cur_figure.size; j++)
                {
                    if (j >= 0 && j < width && i >= 0 && i < height)
                    {
                        if (map[i, j] != 0 && cur_figure.matrix[i - cur_figure.Y, j - cur_figure.X] == 0)
                        {
                            return true;
                        }
                    }
                    if (i == height || j == width)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Remove completed lines
        private void SliceMap()
        {
            int count = 0;
            int curRemovedLines = 0;
            for (int i = 0; i < height; i++)
            {
                count = 0;
                for (int j = 0; j < width; j++)
                {
                    if (map[i, j] != 0)
                        count++;
                }
                if (count == width)
                {
                    curRemovedLines++;
                    for (int k = i; k >= 1; k--)
                    {
                        for (int o = 0; o < width; o++)
                            map[k, o] = map[k - 1, o];
                    }
                }
            }
            if (curRemovedLines == 1)
                score += 100;
            else if (curRemovedLines == 2)
                score += 300;
            else if (curRemovedLines == 3)
                score += 700;
            else if (curRemovedLines == 4)
                score += 1500;
            if (lines / 10 < (lines + curRemovedLines) / 10)
            {
                level++;
                cur_speed -= 50;
                if (cur_speed < 50)
                {
                    cur_speed = 50;
                }
                norm_speed = cur_speed;
            }
            lines += curRemovedLines;
        }
        private Figure GenerateFigure()
        {
            string figures_types = "LJOTIZS";
            Random r = new Random();
            int n = r.Next(figures_types.Length);
            if (figures_types[n] == 'L')
            {
                figure_creator = new CreatorL();
            }
            else if (figures_types[n] == 'J')
            {
                figure_creator = new CreatorJ();
            }
            else if (figures_types[n] == 'O')
            {
                figure_creator = new CreatorO();
            }
            else if (figures_types[n] == 'T')
            {
                figure_creator = new CreatorT();
            }
            else if (figures_types[n] == 'I')
            {
                figure_creator = new CreatorI();
            }
            else if (figures_types[n] == 'Z')
            {
                figure_creator = new CreatorZ();
            }
            else
            {
                figure_creator = new CreatorS();
            }
            return figure_creator.Create();
        }
    }
}