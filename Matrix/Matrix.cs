using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.Matrix
{
    public class Matrix<T>
    {
        private readonly Dimension _dimension;
        private readonly T[,] _data;

        public Matrix(int height, int width)
        {
            _dimension = new Dimension(height, width);
            _data = new T[height, width];
        }

        public Dimension Dimension => _dimension;
        public int Height => _dimension.Height;
        public int Width => _dimension.Width;

        public ref T this[int row, int column]
            => ref _data[row,column];
    }
}
