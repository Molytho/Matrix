using System;

namespace Molytho.Matrix.Calculation.Providers
{
    internal class IntCalculationProvider : ICalculationProvider<int>
    {
        public Matrix<int> Add(Matrix<int> a, Matrix<int> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                throw new DimensionMismatchException();

            Matrix<int> ret = new Matrix<int>(a.Dimension);

            AddThis(ret, a, b);

            return ret;
        }

        public Matrix<int> Inverse(Matrix<int> a)
        {
            Matrix<int> ret = new Matrix<int>(a.Dimension);

            InverseThis(ret, a);

            return ret;
        }
        public Matrix<int> Multipy(Matrix<int> a, Matrix<int> b)
        {
            if (a.Width != b.Height)
                throw new DimensionMismatchException();

            Matrix<int> ret = new Matrix<int>(a.Height, b.Width);

            MultipyThis(ret, a, b);

            return ret;
        }
        public Matrix<int> Multipy(Matrix<int> a, int b)
        {
            Matrix<int> ret = new Matrix<int>(a.Dimension);

            MultipyThis(ret, a, b);

            return ret;
        }
        public Matrix<int> Substract(Matrix<int> a, Matrix<int> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                throw new DimensionMismatchException();

            Matrix<int> ret = new Matrix<int>(a.Dimension);

            SubstractThis(ret, a, b);

            return ret;
        }
        public Matrix<int> Transpose(Matrix<int> a)
        {
            Matrix<int> ret = new Matrix<int>(a.Width, a.Height);
            for (int y = 0; y < a.Height; y++)
                for (int x = 0; x < a.Width; x++)
                {
                    ret[y, x] = a[x, y];
                }
            return ret;
        }

        public void InverseThis(Matrix<int> ret, Matrix<int> a)
        {
            throw new NotImplementedException();
        }
        public void MultipyThis(Matrix<int> ret, Matrix<int> a, Matrix<int> b)
        {
            if (a.Width != b.Height || !(ret.Height == a.Height && ret.Width == b.Width))
                throw new DimensionMismatchException();

            for (int x = 0; x < b.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    int value = 0;
                    for (int i = 0; i < a.Width; i++)
                    {
                        value += a[i, y] * b[x, i];
                    }
                    ret[x, y] = value;
                }
        }
        public void MultipyThis(Matrix<int> ret, Matrix<int> a, int b)
        {
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] * b;
                }
        }
        public void AddThis(Matrix<int> ret, Matrix<int> a, Matrix<int> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret.Dimension))
                throw new DimensionMismatchException();

            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] + b[x, y];
                }
        }
        public void SubstractThis(Matrix<int> ret, Matrix<int> a, Matrix<int> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret.Dimension))
                throw new DimensionMismatchException();

            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] - b[x, y];
                }
        }
    }
}
