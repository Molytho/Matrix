using System;

namespace Molytho.Matrix.Calculation.Providers
{
    internal class FloatCalculationProvider : ICalculationProvider<float>
    {
        public MatrixBase<float> Add(MatrixBase<float> a, MatrixBase<float> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                throw new DimensionMismatchException();

            MatrixBase<float> ret =
                a.Width == 1
                ? new Matrix<float>(a.Dimension)/* FIXME Vector*/
                : new Matrix<float>(a.Dimension);

            AddThis(ret, a, b);

            return ret;
        }

        public MatrixBase<float> Inverse(MatrixBase<float> a)
        {
            //FIXME NotImplemented
            MatrixBase<float> ret = null;

            InverseThis(ret, a);

            return ret;
        }
        public MatrixBase<float> Multipy(MatrixBase<float> a, MatrixBase<float> b)
        {
            if (a.Width != b.Height)
                throw new DimensionMismatchException();

            MatrixBase<float> ret =
                b.Width == 1
                ? new Matrix<float>(a.Height, b.Width)/* FIXME Vector*/
                : new Matrix<float>(a.Height, b.Width);

            MultipyThis(ret, a, b);

            return ret;
        }
        public MatrixBase<float> Multipy(MatrixBase<float> a, float b)
        {
            MatrixBase<float> ret =
                a.Width == 1
                ? new Matrix<float>(a.Dimension)/* FIXME Vector*/
                : new Matrix<float>(a.Dimension);

            MultipyThis(ret, a, b);

            return ret;
        }
        public MatrixBase<float> Substract(MatrixBase<float> a, MatrixBase<float> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                throw new DimensionMismatchException();

            MatrixBase<float> ret =
                a.Width == 1
                ? new Matrix<float>(a.Dimension)/* FIXME Vector*/
                : new Matrix<float>(a.Dimension);

            SubstractThis(ret, a, b);

            return ret;
        }
        public MatrixBase<float> Transpose(MatrixBase<float> a)
        {
            MatrixBase<float> ret =
                a.Height == 1
                ? new Matrix<float>(a.Width, a.Height)/* FIXME Vector*/
                : new Matrix<float>(a.Width, a.Height);
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
        public void MultipyThis(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
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
        public void MultipyThis(MatrixBase<float> ret, MatrixBase<float> a, float b)
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
                throw new DimensionMismatchException();

            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] + b[x, y];
                }
        }
        public void SubstractThis(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
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
