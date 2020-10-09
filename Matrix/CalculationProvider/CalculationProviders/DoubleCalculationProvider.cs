using System;

namespace Molytho.Matrix.Calculation.Providers
{
    internal class DoubleCalculationProvider : ICalculationProvider<double>
    {
        public Matrix<double> Add(Matrix<double> a, Matrix<double> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                throw new DimensionMismatchException();

            Matrix<double> ret = new Matrix<double>(a.Dimension);

            AddThis(ret, a, b);

            return ret;
        }

        public Matrix<double> Inverse(Matrix<double> a)
        {
            Matrix<double> ret = new Matrix<double>(a.Dimension);

            InverseThis(ret, a);

            return ret;
        }
        public Matrix<double> Multipy(Matrix<double> a, Matrix<double> b)
        {
            if (a.Width != b.Height)
                throw new DimensionMismatchException();

            Matrix<double> ret = new Matrix<double>(a.Height, b.Width);

            MultipyThis(ret, a, b);

            return ret;
        }
        public Matrix<double> Multipy(Matrix<double> a, double b)
        {
            Matrix<double> ret = new Matrix<double>(a.Dimension);

            MultipyThis(ret, a, b);

            return ret;
        }
        public Matrix<double> Substract(Matrix<double> a, Matrix<double> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                throw new DimensionMismatchException();

            Matrix<double> ret = new Matrix<double>(a.Dimension);

            SubstractThis(ret, a, b);

            return ret;
        }

        public void InverseThis(Matrix<double> ret, Matrix<double> a)
        {
            throw new NotImplementedException();
        }
        public void MultipyThis(Matrix<double> ret, Matrix<double> a, Matrix<double> b)
        {
            if (a.Width != b.Height || !(ret.Height == a.Height && ret.Width == b.Width))
                throw new DimensionMismatchException();

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
        public void MultipyThis(Matrix<double> ret, Matrix<double> a, double b)
        {
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] * b;
                }
        }
        public void AddThis(Matrix<double> ret, Matrix<double> a, Matrix<double> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret))
                throw new DimensionMismatchException();

            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] + b[x, y];
                }
        }
        public void SubstractThis(Matrix<double> ret, Matrix<double> a, Matrix<double> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret))
                throw new DimensionMismatchException();

            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] - b[x, y];
                }
        }
    }
}
