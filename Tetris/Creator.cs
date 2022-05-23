using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public abstract class Creator
    {
        abstract public Figure Create();
    }
    class CreatorL : Creator
    {
        public override Figure Create()
        {
            return new FigureL();
        }
    }
    class CreatorJ : Creator
    {
        public override Figure Create()
        {
            return new FigureJ();
        }
    }
    class CreatorO : Creator
    {
        public override Figure Create()
        {
            return new FigureO();
        }
    }
    class CreatorT : Creator
    {
        public override Figure Create()
        {
            return new FigureT();
        }
    }
    class CreatorI : Creator
    {
        public override Figure Create()
        {
            return new FigureI();
        }
    }
    class CreatorZ : Creator
    {
        public override Figure Create()
        {
            return new FigureZ();
        }
    }
    class CreatorS : Creator
    {
        public override Figure Create()
        {
            return new FigureS();
        }
    }

    public abstract class Figure
    {
        public int[,] matrix { get; protected set;  }
        public int size { get; protected set; }
        public int X { get; set; }
        public int Y { get; set; }
        protected Figure()
        {
            X = 3;
            Y = 0;
        }
        public void MoveDown()
        {
            Y++;
        }
        public  void MoveRight()
        {
            X++;
        }
        public void MoveLeft()
        {
            X--;
        }
        public void Rotate()
        {
            int[,] t_matrix = new int[size, size];
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    t_matrix[i, j] = matrix[j, size - i - 1];
                }
            }
            matrix = t_matrix;
            Align();
        }
        // If the figure has moved during rotation, align
        private void Align()
        {
            int count = 0;
            while (count == 0)
            {
                for (int i = 0; i < size; i++)
                {
                    count += matrix[0, i];
                }
                if (count == 0)
                {
                    for (int i = 0; i < size - 1; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            matrix[i, j] = matrix[i + 1, j];
                        }
                    }
                    for (int i = 0; i < size; i++)
                    {
                        matrix[size - 1, i] = 0;
                    }
                }
            }
            count = 0;
            while (count == 0)
            {
                for (int i = 0; i < size; i++)
                {
                    count += matrix[i, 0];
                }
                if (count == 0)
                {
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 1; j < size - 1; j++)
                        {
                            matrix[i, j] = matrix[i, j + 1];
                        }
                    }
                    for (int i = 0; i < size; i++)
                    {
                        matrix[i, size - 1] = 0;
                    }
                }
            }
        }
    }
    class FigureL: Figure
    {
        public FigureL()
        {
            size = 3;
            matrix = new int[,] {
                {1, 0, 0 },
                {1, 0, 0 },
                {1, 1, 0 }};
        }
    }
    class FigureJ : Figure
    {
        public FigureJ()
        {
            size = 3;
            matrix = new int[,] {
                {0, 1, 0 },
                {0, 1, 0 },
                {1, 1, 0 }};
        }
    }
    class FigureO : Figure
    {
        public FigureO()
        {
            size = 2;
            matrix = new int[,] {
                {1, 1 },
                {1, 1 }};
        }
    }
    class FigureT : Figure
    {
        public FigureT()
        {
            size = 3;
            matrix = new int[,] {
                {0, 1, 0 },
                {1, 1, 1 },
                {0, 0, 0 }};
        }
    }
    class FigureI : Figure
    {
        public FigureI()
        {
            size = 4;
            matrix = new int[,] {
                {1, 0, 0, 0 },
                {1, 0, 0, 0 },
                {1, 0, 0, 0 },
                {1, 0, 0, 0 }};
        }
    }
    class FigureZ : Figure
    {
        public FigureZ()
        {
            size = 3;
            matrix = new int[,] {
                {1, 1, 0 },
                {0, 1, 1 },
                {0, 0, 0 }};
        }
    }
    class FigureS : Figure
    {
        public FigureS()
        {
            size = 3;
            matrix = new int[,] {
                {0, 1, 1 },
                {1, 1, 0 },
                {0, 0, 0 }};
        }
    }
}
