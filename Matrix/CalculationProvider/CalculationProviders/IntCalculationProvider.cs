using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Molytho.Matrix.Calculation.Providers
{
    internal class IntCalculationProvider : ICalculationProvider<int>
    {
        public MatrixBase<int> Add(MatrixBase<int> a, MatrixBase<int> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                throw new DimensionMismatchException();

            MatrixBase<int> ret =
                a.Width == 1
                ? new Vector<int>(a.Dimension) as MatrixBase<int>
                : new Matrix<int>(a.Dimension) as MatrixBase<int>;

            AddThis(ret, a, b);

            return ret;
        }

        public MatrixBase<int> Inverse(MatrixBase<int> a)
        {
            //FIXME NotImplemented
            MatrixBase<int> ret = null;

            InverseThis(ret, a);

            return ret;
        }
        public MatrixBase<int> Multipy(MatrixBase<int> a, MatrixBase<int> b)
        {
            if (a.Width != b.Height)
                throw new DimensionMismatchException();

            MatrixBase<int> ret =
                b.Width == 1
                ? new Vector<int>(a.Height) as MatrixBase<int>
                : new Matrix<int>(a.Height, b.Width);

            MultipyThis(ret, a, b);

            return ret;
        }
        public MatrixBase<int> Multipy(MatrixBase<int> a, int b)
        {
            MatrixBase<int> ret =
                a.Width == 1
                ? new Vector<int>(a.Dimension) as MatrixBase<int>
                : new Matrix<int>(a.Dimension) as MatrixBase<int>;

            MultipyThis(ret, a, b);

            return ret;
        }
        public MatrixBase<int> Substract(MatrixBase<int> a, MatrixBase<int> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                throw new DimensionMismatchException();

            MatrixBase<int> ret =
                a.Width == 1
                ? new Vector<int>(a.Dimension) as MatrixBase<int>
                : new Matrix<int>(a.Dimension) as MatrixBase<int>;

            SubstractThis(ret, a, b);

            return ret;
        }
        public MatrixBase<int> Transpose(MatrixBase<int> a)
        {
            MatrixBase<int> ret =
                a.Height == 1
                ? new Vector<int>(a.Width) as MatrixBase<int>
                : new Matrix<int>(a.Width, a.Height) as MatrixBase<int>;
            for (int y = 0; y < a.Height; y++)
                for (int x = 0; x < a.Width; x++)
                {
                    ret[y, x] = a[x, y];
                }
            return ret;
        }

        public void InverseThis(MatrixBase<int> ret, MatrixBase<int> a)
        {
            throw new NotImplementedException();
        }
        public void MultipyThis(MatrixBase<int> ret, MatrixBase<int> a, MatrixBase<int> b)
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
        public void MultipyThis(MatrixBase<int> ret, MatrixBase<int> a, int b)
        {
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] * b;
                }
        }
        private unsafe void AddThisSse2(MatrixBase<int> ret, MatrixBase<int> a, MatrixBase<int> b)
        {
            int calculated = 0;
            fixed (int* base_a = &a[0, 0], base_b = &b[0, 0], base_ret = &ret[0, 0])
            {
                while (calculated + Vector128<int>.Count <= ret.Width * ret.Height)
                {
                    Vector128<int> solution =
                        Sse2.Add(
                            Sse2.LoadVector128(base_a + calculated),
                            Sse2.LoadVector128(base_b + calculated)
                            );
                    Sse2.Store(base_ret + calculated, solution);
                    calculated += Vector128<int>.Count;
                }
                for(; calculated < ret.Width * ret.Height; calculated++)
                {
                    *(base_ret + calculated) = *(base_a + calculated) + *(base_b + calculated);
                }
            }
        }
        public void AddThis(MatrixBase<int> ret, MatrixBase<int> a, MatrixBase<int> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret.Dimension))
                throw new DimensionMismatchException();

            if (Sse2.IsSupported)
            {
                AddThisSse2(ret, a, b);
            }
            else
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] + b[x, y];
                }
        }
        public void SubstractThis(MatrixBase<int> ret, MatrixBase<int> a, MatrixBase<int> b)
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
