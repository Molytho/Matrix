using System;

namespace Molytho.Matrix.Calculation.Providers
{
    internal class FloatCalculationProvider : ICalculationProvider<float>
    {
        public Matrix<float> Add(Matrix<float> a, Matrix<float> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                throw new DimensionMismatchException();

            Matrix<float> ret = new Matrix<float>(a.Dimension);

            AddThis(ret, a, b);

            return ret;
        }

        public Matrix<float> Inverse(Matrix<float> a)
        {
            Matrix<float> ret = new Matrix<float>(a.Dimension);

            InverseThis(ret, a);

            return ret;
        }
        public Matrix<float> Multipy(Matrix<float> a, Matrix<float> b)
        {
            if (a.Width != b.Height)
                throw new DimensionMismatchException();

            Matrix<float> ret = new Matrix<float>(a.Height, b.Width);

            MultipyThis(ret, a, b);

            return ret;
        }
        public Matrix<float> Multipy(Matrix<float> a, float b)
        {
            Matrix<float> ret = new Matrix<float>(a.Dimension);

            MultipyThis(ret, a, b);

            return ret;
        }
        public Matrix<float> Substract(Matrix<float> a, Matrix<float> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                throw new DimensionMismatchException();

            Matrix<float> ret = new Matrix<float>(a.Dimension);

            SubstractThis(ret, a, b);

            return ret;
        }
        public Matrix<float> Transpose(Matrix<float> a)
        {
            Matrix<float> ret = new Matrix<float>(a.Width, a.Height);
            for (int y = 0; y < a.Height; y++)
                for (int x = 0; x < a.Width; x++)
                {
                    ret[y, x] = a[x, y];
                }
            return ret;
        }

        public void InverseThis(Matrix<float> ret, Matrix<float> a)
        {
            throw new NotImplementedException();
        }
        public void MultipyThis(Matrix<float> ret, Matrix<float> a, Matrix<float> b)
        {
            if (a.Width != b.Height || !(ret.Height == a.Height && ret.Width == b.Width))
                throw new DimensionMismatchException();

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
        public void MultipyThis(Matrix<float> ret, Matrix<float> a, float b)
        {
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] * b;
                }
        }
        public void AddThis(Matrix<float> ret, Matrix<float> a, Matrix<float> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret.Dimension))
                throw new DimensionMismatchException();

            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] + b[x, y];
                }
        }
        public void SubstractThis(Matrix<float> ret, Matrix<float> a, Matrix<float> b)
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
