using System;

namespace Molytho.Matrix.Calculation.Providers
{
    internal class DoubleCalculationProvider : ICalculationProvider<double>
    {
        public MatrixBase<double> Add(MatrixBase<double> a, MatrixBase<double> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                ThrowHelper.ThrowDimensionMismatch();

            MatrixBase<double> ret =
                a.Width == 1
                ? new Vector<double>(a.Dimension) as MatrixBase<double>
                : new Matrix<double>(a.Dimension) as MatrixBase<double>;

            AddThis(ret, a, b);

            return ret;
        }

        public MatrixBase<double> Inverse(MatrixBase<double> a)
        {
            throw new NotImplementedException();
        }
        public MatrixBase<double> Multiply(MatrixBase<double> a, MatrixBase<double> b)
        {
            if (a.Width != b.Height)
                ThrowHelper.ThrowDimensionMismatch();

            MatrixBase<double> ret =
                b.Width == 1
                ? new Vector<double>(a.Height) as MatrixBase<double>
                : new Matrix<double>(a.Height, b.Width) as MatrixBase<double>;

            MultiplyThis(ret, a, b);

            return ret;
        }
        public MatrixBase<double> Multiply(MatrixBase<double> a, double b)
        {
            MatrixBase<double> ret =
                a.Width == 1
                ? new Vector<double>(a.Dimension) as MatrixBase<double>
                : new Matrix<double>(a.Dimension) as MatrixBase<double>;

            MultiplyThis(ret, a, b);

            return ret;
        }
        public MatrixBase<double> Substract(MatrixBase<double> a, MatrixBase<double> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                ThrowHelper.ThrowDimensionMismatch();

            MatrixBase<double> ret =
                a.Width == 1
                ? new Vector<double>(a.Dimension) as MatrixBase<double>
                : new Matrix<double>(a.Dimension) as MatrixBase<double>;

            SubstractThis(ret, a, b);

            return ret;
        }
        public MatrixBase<double> Transpose(MatrixBase<double> a)
        {
            MatrixBase<double> ret =
                a.Height == 1
                ? new Vector<double>(a.Width) as MatrixBase<double>
                : new Matrix<double>(a.Width, a.Height) as MatrixBase<double>;
            for (int y = 0; y < a.Height; y++)
                for (int x = 0; x < a.Width; x++)
                {
                    ret[y, x] = a[x, y];
                }
            return ret;
        }

        public void InverseThis(MatrixBase<double> ret, MatrixBase<double> a)
        {
            throw new NotImplementedException();
        }
        public void MultiplyThis(MatrixBase<double> ret, MatrixBase<double> a, MatrixBase<double> b)
        {
            if (a.Width != b.Height || !(ret.Height == a.Height && ret.Width == b.Width))
                ThrowHelper.ThrowDimensionMismatch();

            for (int x = 0; x < b.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    double value = 0;
                    for (int i = 0; i < a.Width; i++)
                    {
                        value += a[i, y] * b[x, i];
                    }
                    ret[x, y] = value;
                }
        }
        public void MultiplyThis(MatrixBase<double> ret, MatrixBase<double> a, double b)
        {
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] * b;
                }
        }
        public void AddThis(MatrixBase<double> ret, MatrixBase<double> a, MatrixBase<double> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret.Dimension))
                ThrowHelper.ThrowDimensionMismatch();

            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] + b[x, y];
                }
        }
        public void SubstractThis(MatrixBase<double> ret, MatrixBase<double> a, MatrixBase<double> b)
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
