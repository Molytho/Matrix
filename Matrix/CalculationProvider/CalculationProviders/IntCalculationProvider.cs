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
                ThrowHelper.ThrowDimensionMismatch();

            MatrixBase<int> ret =
                a.Width == 1
                ? new Vector<int>(a.Dimension) as MatrixBase<int>
                : new Matrix<int>(a.Dimension) as MatrixBase<int>;

            AddThis(ret, a, b);

            return ret;
        }
        public MatrixBase<int> Inverse(MatrixBase<int> a)
        {
            throw new NotImplementedException();
        }
        public MatrixBase<int> Multiply(MatrixBase<int> a, MatrixBase<int> b)
        {
            if (a.Width != b.Height)
                ThrowHelper.ThrowDimensionMismatch();

            MatrixBase<int> ret =
                b.Width == 1
                ? new Vector<int>(a.Height) as MatrixBase<int>
                : new Matrix<int>(a.Height, b.Width);

            MultiplyThis(ret, a, b);

            return ret;
        }
        public Vector<int> Multiply(Vector<int> a, Vector<int> b)
        {
            if (a.Dimension != b.Dimension)
                ThrowHelper.ThrowDimensionMismatch(DimensionType.Row);

            Vector<int> ret = new Vector<int>(a.Dimension);

            MultiplyThis(ret, a, b);

            return ret;
        }
        public MatrixBase<int> Multiply(MatrixBase<int> a, int b)
        {
            MatrixBase<int> ret =
                a.Width == 1
                ? new Vector<int>(a.Dimension) as MatrixBase<int>
                : new Matrix<int>(a.Dimension) as MatrixBase<int>;

            MultiplyThis(ret, a, b);

            return ret;
        }
        public MatrixBase<int> Substract(MatrixBase<int> a, MatrixBase<int> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                ThrowHelper.ThrowDimensionMismatch();

            MatrixBase<int> ret =
                a.Width == 1
                ? new Vector<int>(a.Dimension) as MatrixBase<int>
                : new Matrix<int>(a.Dimension) as MatrixBase<int>;

            SubstractThis(ret, a, b);

            return ret;
        }
        private unsafe void TransposeAvx2(MatrixBase<int> ret, MatrixBase<int> a)
        {
            fixed (int* base_a = &a[0, 0], base_ret = &ret[0, 0])
            {
                int calculated = 0;
                int* indices = stackalloc int[]
                {
                    0,
                    a.Width * 1 * sizeof(int),
                    a.Width * 2 * sizeof(int),
                    a.Width * 3 * sizeof(int),
                    a.Width * 4 * sizeof(int),
                    a.Width * 5 * sizeof(int),
                    a.Width * 6 * sizeof(int),
                    a.Width * 7 * sizeof(int)
                };
                Vector256<int> indexVec = Avx.LoadVector256(indices);
                if (a.Height >= Vector256<int>.Count)
                {
                    int indexScale = a.Width * 8 * sizeof(int);
                    Vector256<int> indexVecInc = Avx2.BroadcastScalarToVector256(&indexScale);
                    while (calculated + Vector256<int>.Count < a.Height)
                    {
                        if (calculated > 0)
                            indexVec = Avx2.Add(indexVec, indexVecInc);

                        for (int x = 0; x < a.Width; x++)
                        {
                            Vector256<int> storeVec = Avx2.GatherVector256(base_a + x, indexVec, 1);
                            Avx.Store(base_ret + x * ret.Width + calculated, storeVec);
                        }
                        calculated += Vector256<int>.Count;
                    }
                    if (a.Height % Vector256<int>.Count > 1)
                        indexVec = Avx2.Add(indexVec, indexVecInc);
                }
                if (a.Height % Vector256<int>.Count > 1)
                {
                    int* masks = stackalloc int[Vector256<int>.Count];
                    for (int i = 0; i < Vector256<int>.Count; i++)
                        //Most significant bit is used as mask
                        masks[i] = a.Height - calculated - i > 0 ? unchecked((int)(0x80000000)) : 0;

                    Vector256<int> maskVec = Avx.LoadVector256(masks);
                    for (int x = 0; x < a.Width; x++)
                    {
                        Vector256<int> storeVec = Avx2.GatherMaskVector256(Vector256<int>.Zero, base_a + x, indexVec, maskVec, 1);
                        Avx2.MaskStore(base_ret + x * ret.Width + calculated, maskVec, storeVec);
                    }
                }
                else if (a.Height % Vector256<int>.Count == 1)
                {
                    for (int x = 0; x < a.Width; x++)
                    {
                        *(base_ret + x * ret.Width + calculated) = *(base_a + x + calculated * a.Width);
                    }
                }
            }
        }
        public unsafe MatrixBase<int> Transpose(MatrixBase<int> a)
        {
            MatrixBase<int> ret =
                a.Height == 1
                ? new Vector<int>(a.Width) as MatrixBase<int>
                : new Matrix<int>(a.Width, a.Height) as MatrixBase<int>;
            if (a.Height == 1 || a.Width == 1)
            {
                fixed (int* base_a = &a[0, 0], base_ret = &ret[0, 0])
                    MemoryOperation.memcpy(base_ret, base_a, a.Height * a.Width * sizeof(int));
            }
            else if (Avx2.IsSupported)
                TransposeAvx2(ret, a);
            else
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
        public void MultiplyThis(MatrixBase<int> ret, MatrixBase<int> a, MatrixBase<int> b)
        {
            if (a.Width != b.Height || !(ret.Height == a.Height && ret.Width == b.Width))
                ThrowHelper.ThrowDimensionMismatch();

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
        public void MultiplyThis(Vector<int> ret, Vector<int> a, Vector<int> b)
        {
            if (a.Dimension != b.Dimension || ret.Dimension != a.Dimension)
                ThrowHelper.ThrowDimensionMismatch(DimensionType.Row);

            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] * b[x, y];
                }
        }
        public void MultiplyThis(MatrixBase<int> ret, MatrixBase<int> a, int b)
        {
            if(ret.Dimension != a.Dimension)
                ThrowHelper.ThrowDimensionMismatch();

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
                if (Avx2.IsSupported)
                {
                    while (calculated + Vector256<int>.Count <= ret.Width * ret.Height)
                    {
                        Vector256<int> solution =
                            Avx2.Add(
                                Avx.LoadVector256(base_a + calculated),
                                Avx.LoadVector256(base_b + calculated)
                                );
                        Avx.Store(base_ret + calculated, solution);
                        calculated += Vector256<int>.Count;
                    }
                }
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
                for (; calculated < ret.Width * ret.Height; calculated++)
                {
                    *(base_ret + calculated) = *(base_a + calculated) + *(base_b + calculated);
                }
            }
        }
        public void AddThis(MatrixBase<int> ret, MatrixBase<int> a, MatrixBase<int> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret.Dimension))
                ThrowHelper.ThrowDimensionMismatch();

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

        private unsafe void SubstractThisSse2(MatrixBase<int> ret, MatrixBase<int> a, MatrixBase<int> b)
        {
            int calculated = 0;
            fixed (int* base_a = &a[0, 0], base_b = &b[0, 0], base_ret = &ret[0, 0])
            {
                if (Avx2.IsSupported)
                {
                    while (calculated + Vector256<int>.Count <= ret.Width * ret.Height)
                    {
                        Vector256<int> solution =
                            Avx2.Subtract(
                                Avx.LoadVector256(base_a + calculated),
                                Avx.LoadVector256(base_b + calculated)
                                );
                        Avx.Store(base_ret + calculated, solution);
                        calculated += Vector256<int>.Count;
                    }
                }
                while (calculated + Vector128<int>.Count <= ret.Width * ret.Height)
                {
                    Vector128<int> solution =
                        Sse2.Subtract(
                            Sse2.LoadVector128(base_a + calculated),
                            Sse2.LoadVector128(base_b + calculated)
                            );
                    Sse2.Store(base_ret + calculated, solution);
                    calculated += Vector128<int>.Count;
                }
                for (; calculated < ret.Width * ret.Height; calculated++)
                {
                    *(base_ret + calculated) = *(base_a + calculated) + *(base_b + calculated);
                }
            }
        }
        public void SubstractThis(MatrixBase<int> ret, MatrixBase<int> a, MatrixBase<int> b)
        {
            if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret.Dimension))
                ThrowHelper.ThrowDimensionMismatch();

            if (Sse2.IsSupported)
            {
                SubstractThisSse2(ret, a, b);
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
