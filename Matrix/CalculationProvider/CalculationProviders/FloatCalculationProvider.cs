using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

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
        private unsafe void MultiplyThisAvx(MatrixBase<float> ret, MatrixBase<float> a, float b)
        {
            int calculated = 0;
            fixed (float* base_a = &a[0, 0], base_ret = &ret[0, 0])
            {
                Vector256<float> scalar = Avx.BroadcastScalarToVector256(&b);
                while (calculated + Vector256<float>.Count <= ret.Width * ret.Height)
                {
                    Vector256<float> solution =
                        Avx.Multiply(
                            Avx.LoadVector256(base_a + calculated),
                            scalar
                            );
                    Avx.Store(base_ret + calculated, solution);
                    calculated += Vector256<float>.Count;
                }
                if(calculated + Vector128<float>.Count < ret.Height * ret.Width)
                {
                    Vector128<float> scalar128 = Avx.ExtractVector128(scalar, 0);
                    Vector128<float> solution =
                        Sse.Multiply(
                            Sse.LoadVector128(base_a + calculated),
                            scalar128
                            );
                    Sse.Store(base_ret + calculated, solution);
                    calculated += Vector128<float>.Count;
                }
                for (; calculated < ret.Width * ret.Height; calculated++)
                {
                    *(base_ret + calculated) = *(base_a + calculated) * b;
                }
            }
        }
        public void MultiplyThis(MatrixBase<float> ret, MatrixBase<float> a, float b)
        {
            if (Avx.IsSupported)
                MultiplyThisAvx(ret, a, b);
            else
                for (int x = 0; x < a.Width; x++)
                    for (int y = 0; y < a.Height; y++)
                    {
                        ret[x, y] = a[x, y] * b;
                    }
        }

        private unsafe void AddThisAvx(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            int calculated = 0;
            fixed (float* base_a = &a[0, 0], base_b = &b[0, 0], base_ret = &ret[0, 0])
            {
                while (calculated + Vector256<int>.Count <= ret.Width * ret.Height)
                {
                    Vector256<float> solution =
                        Avx.Add(
                            Avx.LoadVector256(base_a + calculated),
                            Avx.LoadVector256(base_b + calculated)
                            );
                    Avx.Store(base_ret + calculated, solution);
                    calculated += Vector256<float>.Count;
                }
                if (calculated + Vector128<float>.Count <= ret.Width * ret.Height)
                {
                    Vector128<float> solution =
                        Sse.Add(
                            Sse.LoadVector128(base_a + calculated),
                            Sse.LoadVector128(base_b + calculated)
                            );
                    Sse.Store(base_ret + calculated, solution);
                    calculated += Vector128<float>.Count;
                }
                for (; calculated < ret.Width * ret.Height; calculated++)
                {
                    *(base_ret + calculated) = *(base_a + calculated) + *(base_b + calculated);
                }
            }
        }
        private unsafe void AddThisSse(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            int calculated = 0;
            fixed (float* base_a = &a[0, 0], base_b = &b[0, 0], base_ret = &ret[0, 0])
            {
                while (calculated + Vector128<float>.Count <= ret.Width * ret.Height)
                {
                    Vector128<float> solution =
                        Sse.Add(
                            Sse.LoadVector128(base_a + calculated),
                            Sse.LoadVector128(base_b + calculated)
                            );
                    Sse.Store(base_ret + calculated, solution);
                    calculated += Vector128<float>.Count;
                }
                for (; calculated < ret.Width * ret.Height; calculated++)
                {
                    *(base_ret + calculated) = *(base_a + calculated) + *(base_b + calculated);
                }
            }
        }
        public void AddThis(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret.Dimension))
                ThrowHelper.ThrowDimensionMismatch();

            if (Avx.IsSupported)
            {
                AddThisAvx(ret, a, b);
            }
            else if (Sse.IsSupported)
            {
                AddThisSse(ret, a, b);
            }
            else
                for (int x = 0; x < a.Width; x++)
                    for (int y = 0; y < a.Height; y++)
                    {
                        ret[x, y] = a[x, y] + b[x, y];
                    }
        }

        private unsafe void SubstractThisAvx(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            int calculated = 0;
            fixed (float* base_a = &a[0, 0], base_b = &b[0, 0], base_ret = &ret[0, 0])
            {
                while (calculated + Vector256<float>.Count <= ret.Width * ret.Height)
                {
                    Vector256<float> solution =
                        Avx.Subtract(
                            Avx.LoadVector256(base_a + calculated),
                            Avx.LoadVector256(base_b + calculated)
                            );
                    Avx.Store(base_ret + calculated, solution);
                    calculated += Vector256<float>.Count;
                }
                if (calculated + Vector128<float>.Count <= ret.Width * ret.Height)
                {
                    Vector128<float> solution =
                        Sse.Subtract(
                            Sse.LoadVector128(base_a + calculated),
                            Sse.LoadVector128(base_b + calculated)
                            );
                    Sse.Store(base_ret + calculated, solution);
                    calculated += Vector128<float>.Count;
                }
                for (; calculated < ret.Width * ret.Height; calculated++)
                {
                    *(base_ret + calculated) = *(base_a + calculated) + *(base_b + calculated);
                }
            }
        }
        private unsafe void SubstractThisSse(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            int calculated = 0;
            fixed (float* base_a = &a[0, 0], base_b = &b[0, 0], base_ret = &ret[0, 0])
            {
                while (calculated + Vector128<float>.Count <= ret.Width * ret.Height)
                {
                    Vector128<float> solution =
                        Sse.Subtract(
                            Sse.LoadVector128(base_a + calculated),
                            Sse.LoadVector128(base_b + calculated)
                            );
                    Sse.Store(base_ret + calculated, solution);
                    calculated += Vector128<float>.Count;
                }
                for (; calculated < ret.Width * ret.Height; calculated++)
                {
                    *(base_ret + calculated) = *(base_a + calculated) + *(base_b + calculated);
                }
            }
        }
        public void SubstractThis(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret.Dimension))
                ThrowHelper.ThrowDimensionMismatch();

            if (Avx.IsSupported)
            {
                SubstractThisAvx(ret, a, b);
            }
            else if (Sse.IsSupported)
            {
                SubstractThisSse(ret, a, b);
            }
            else
                for (int x = 0; x < a.Width; x++)
                    for (int y = 0; y < a.Height; y++)
                    {
                        ret[x, y] = a[x, y] - b[x, y];
                    }
        }
    }
}
