﻿using System;

namespace Molytho.Matrix.Calculation.Providers
{
    internal class FloatCalculationProvider : ICalculationProvider<float>
    {
        public MatrixBase<float> Add(MatrixBase<float> a, MatrixBase<float> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                ThrowHelper.ThrowDimensionMismatch();

            MatrixBase<float> ret =
                a.Width == 1
                ? new Vector<float>(a.Dimension) as MatrixBase<float>
                : new Matrix<float>(a.Dimension) as MatrixBase<float>;

            AddThis(ret, a, b);

            return ret;
        }

        public MatrixBase<float> Inverse(MatrixBase<float> a)
        {
            throw new NotImplementedException();
        }
        public MatrixBase<float> Multiply(MatrixBase<float> a, MatrixBase<float> b)
        {
            if (a.Width != b.Height)
                ThrowHelper.ThrowDimensionMismatch();

            MatrixBase<float> ret =
                b.Width == 1
                ? new Vector<float>(a.Height) as MatrixBase<float>
                : new Matrix<float>(a.Height, b.Width) as MatrixBase<float>;

            MultiplyThis(ret, a, b);

            return ret;
        }
        public Vector<float> Multiply(Vector<float> a, Vector<float> b)
        {
            if (a.Dimension != b.Dimension)
                ThrowHelper.ThrowDimensionMismatch(DimensionType.Row);

            Vector<float> ret = new Vector<float>(a.Dimension);

            MultiplyThis(ret, a, b);

            return ret;
        }
        public MatrixBase<float> Multiply(MatrixBase<float> a, float b)
        {
            MatrixBase<float> ret =
                a.Width == 1
                ? new Vector<float>(a.Dimension) as MatrixBase<float>
                : new Matrix<float>(a.Dimension) as MatrixBase<float>;

            MultiplyThis(ret, a, b);

            return ret;
        }
        public MatrixBase<float> Substract(MatrixBase<float> a, MatrixBase<float> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                ThrowHelper.ThrowDimensionMismatch();

            MatrixBase<float> ret =
                a.Width == 1
                ? new Vector<float>(a.Dimension) as MatrixBase<float>
                : new Matrix<float>(a.Dimension) as MatrixBase<float>;

            SubstractThis(ret, a, b);

            return ret;
        }
        public MatrixBase<float> Transpose(MatrixBase<float> a)
        {
            MatrixBase<float> ret =
                a.Height == 1
                ? new Vector<float>(a.Width) as MatrixBase<float>
                : new Matrix<float>(a.Width, a.Height) as MatrixBase<float>;
            for (int y = 0; y < a.Height; y++)
                for (int x = 0; x < a.Width; x++)
                {
                    ret[y, x] = a[x, y];
                }
            return ret;
        }

        public void InverseThis(MatrixBase<float> ret, MatrixBase<float> a)
        {
            throw new NotImplementedException();
        }
        public void MultiplyThis(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            if (a.Width != b.Height || !(ret.Height == a.Height && ret.Width == b.Width))
                ThrowHelper.ThrowDimensionMismatch();

            for (int x = 0; x < b.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    float value = 0;
                    for (int i = 0; i < a.Width; i++)
                    {
                        value += a[i, y] * b[x, i];
                    }
                    ret[x, y] = value;
                }
        }
        public void MultiplyThis(Vector<float> ret, Vector<float> a, Vector<float> b)
        {
            if (a.Dimension != b.Dimension || ret.Dimension != a.Dimension)
                ThrowHelper.ThrowDimensionMismatch(DimensionType.Row);

            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] * b[x, y];
                }
        }
        public void MultiplyThis(MatrixBase<float> ret, MatrixBase<float> a, float b)
        {
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] * b;
                }
        }
        public void AddThis(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret.Dimension))
                ThrowHelper.ThrowDimensionMismatch();

            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] + b[x, y];
                }
        }
        public void SubstractThis(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret.Dimension))
                ThrowHelper.ThrowDimensionMismatch();

            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] - b[x, y];
                }
        }
    }
}
