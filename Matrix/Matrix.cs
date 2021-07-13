﻿using System;
using System.Diagnostics;

namespace Molytho.Matrix
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Matrix<T> : MatrixBase<T>
        where T : notnull
    {
        private readonly T[,] _data;

        public Matrix(int height, int width) : base(height, width)
        {
            _data = new T[width, height];
        }
        public Matrix(Dimension dimension) : base(dimension)
        {
            _data = new T[dimension.Width, dimension.Height];
        }
        public Matrix(T[,] initialValues) : base(initialValues.GetLength(1), initialValues.GetLength(0))
        {
            if (initialValues == null)
                throw new ArgumentNullException(nameof(initialValues));

            _data = initialValues;
        }

        public override ref T this[int x, int y]
            => ref _data[x, y];

        public static implicit operator Matrix<T>(T[,] values) => new Matrix<T>(values);
        public static explicit operator T[,](Matrix<T> matrix) => matrix._data;

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
