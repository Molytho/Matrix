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
                throw new DimensionMismatchException();

            MatrixBase<float> ret =
                a.Width == 1
                ? new Vector<float>(a.Dimension) as MatrixBase<float>
                : new Matrix<float>(a.Dimension) as MatrixBase<float>;

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
                ? new Vector<float>(a.Height) as MatrixBase<float>
                : new Matrix<float>(a.Height, b.Width) as MatrixBase<float>;

            MultipyThis(ret, a, b);

            return ret;
        }
        public MatrixBase<float> Multipy(MatrixBase<float> a, float b)
        {
            MatrixBase<float> ret =
                a.Width == 1
                ? new Vector<float>(a.Dimension) as MatrixBase<float>
                : new Matrix<float>(a.Dimension) as MatrixBase<float>;

            MultipyThis(ret, a, b);

            return ret;
        }
        public MatrixBase<float> Substract(MatrixBase<float> a, MatrixBase<float> b)
        {
            if (!a.Dimension.Equals(b.Dimension))
                throw new DimensionMismatchException();

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

        private unsafe void MultiplyThisAvx2(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            fixed (float* base_a = &a[0, 0], base_b = &b[0, 0], base_ret = &ret[0, 0])
            {
                float* temp_storage;
                Int32* indices = stackalloc Int32[]
                {
                    0,
                    b.Width * 1 * sizeof(float),
                    b.Width * 2 * sizeof(float),
                    b.Width * 3 * sizeof(float),
                    b.Width * 4 * sizeof(float),
                    b.Width * 5 * sizeof(float),
                    b.Width * 6 * sizeof(float),
                    b.Width * 7 * sizeof(float)
                };
                Int32 indexScale = b.Width * 8 * sizeof(float);

                int calculated;
                Vector256<Int32> indexVec;
                Vector256<Int32> indexVecInc = Avx2.BroadcastScalarToVector256(&indexScale);

                if (a.Height <= b.Width)
                {
                    //FIXME: Optimize more
                    for (int i = 0; i < ret.Width * ret.Height; i++)
                        base_ret[i] = 0;

                    temp_storage = base_ret;
                    for (int y = 0; y < ret.Height; y++)
                    {
                        calculated = 0;
                        indexVec = Avx2.LoadVector256(indices);
                        while (calculated + Vector256<float>.Count <= a.Width)
                        {
                            if (calculated > 0)
                                indexVec = Avx2.Add(indexVec, indexVecInc);

                            Vector256<float> aVec = Avx.LoadVector256(base_a + calculated + y * a.Width);
                            for (int x = 0; x < ret.Width; x++)
                            {
                                Vector256<float> bVec = Avx2.GatherVector256(base_b + x, indexVec, 1);
                                Vector256<float> tempVec = Avx2.Multiply(aVec, bVec);
                                tempVec = Avx2.HorizontalAdd(tempVec, tempVec);
                                Vector128<float> tempVec128 = Avx2.Add(Avx2.ExtractVector128(tempVec, 1), Avx2.ExtractVector128(tempVec, 0));
                                float solution = Avx2.HorizontalAdd(tempVec128, tempVec128).ToScalar();
                                temp_storage[x] += solution;
                            }
                            calculated += Vector256<float>.Count;
                        }

                        if (calculated + Vector128<float>.Count <= a.Width)
                        {
                            if (calculated > 0)
                                indexVec = Avx2.Add(indexVec, indexVecInc);

                            Vector128<float> aVec = Avx.LoadVector128(base_a + calculated + y * a.Width);
                            for (int x = 0; x < ret.Width; x++)
                            {
                                Vector128<float> bVec = Avx2.GatherVector128(base_b + x, Avx2.ExtractVector128(indexVec, 0), 1);
                                Vector128<float> tempVec = Avx2.Multiply(aVec, bVec);
                                tempVec = Avx2.HorizontalAdd(tempVec, tempVec);
                                float solution = Avx2.HorizontalAdd(tempVec, tempVec).ToScalar();
                                temp_storage[x] += solution;
                            }
                            calculated += Vector128<float>.Count;
                        }

                        for (; calculated < a.Width; calculated++)
                        {
                            for (int x = 0; x < ret.Width; x++)
                            {
                                temp_storage[x] += *(base_a + calculated + y * a.Width) * *(base_b + x + b.Width * calculated);
                            }
                        }

                        temp_storage += ret.Width;
                    }
                }
                else
                {
                    float* @stackalloc = stackalloc float[ret.Height];
                    temp_storage = @stackalloc;
                    for (int x = 0; x < ret.Width; x++)
                    {
                        calculated = 0;
                        indexVec = Avx2.LoadVector256(indices);
                        while (calculated + Vector256<float>.Count <= a.Width)
                        {
                            if (calculated > 0)
                                indexVec = Avx2.Add(indexVec, indexVecInc);

                            Vector256<float> bVec = Avx2.GatherVector256(base_b + x, indexVec, 1);
                            for (int y = 0; y < ret.Height; y++)
                            {
                                Vector256<float> aVec = Avx.LoadVector256(base_a + calculated + y * a.Width);
                                Vector256<float> tempVec = Avx2.Multiply(aVec, bVec);
                                tempVec = Avx2.HorizontalAdd(tempVec, tempVec);
                                Vector128<float> tempVec128 = Avx2.Add(Avx2.ExtractVector128(tempVec, 1), Avx2.ExtractVector128(tempVec, 0));
                                float solution = Avx2.HorizontalAdd(tempVec128, tempVec128).ToScalar();
                                temp_storage[y] += solution;
                            }
                            calculated += Vector256<float>.Count;
                        }

                        if (calculated + Vector128<float>.Count <= a.Width)
                        {
                            if (calculated > 0)
                                indexVec = Avx2.Add(indexVec, Avx2.BroadcastScalarToVector256(&indexScale));

                            Vector128<float> bVec = Avx2.GatherVector128(base_b + x, Avx2.ExtractVector128(indexVec, 0), 1);
                            for (int y = 0; y < ret.Height; y++)
                            {
                                Vector128<float> aVec = Avx.LoadVector128(base_a + calculated + y * a.Width);
                                Vector128<float> tempVec = Avx2.Multiply(aVec, bVec);
                                tempVec = Avx2.HorizontalAdd(tempVec, tempVec);
                                float solution = Avx2.HorizontalAdd(tempVec, tempVec).ToScalar();
                                temp_storage[y] += solution;
                            }
                            calculated += Vector128<float>.Count;
                        }

                        for (; calculated < a.Width; calculated++)
                        {
                            for (int y = 0; y < ret.Height; y++)
                            {
                                temp_storage[y] += *(base_a + calculated + y * a.Width) * *(base_b + x + b.Width * calculated);
                            }
                        }

                        //FIXME: There should be another solution for this
                        for (int y = 0; y < ret.Height; y++)
                        {
                            ret[x, y] = temp_storage[y];
                            temp_storage[y] = 0;
                        }
                    }
                }
            }
        }
        public void MultipyThis(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            if (a.Width != b.Height || !(ret.Height == a.Height && ret.Width == b.Width))
                throw new DimensionMismatchException();

            if (Avx2.IsSupported)
                MultiplyThisAvx2(ret, a, b);
            else
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

        private unsafe void MultiplyThisSse(MatrixBase<float> ret, MatrixBase<float> a, float b)
        {
            int calculated = 0;
            fixed (float* base_a = &a[0, 0], base_ret = &ret[0, 0])
            {
                Vector128<float> scalar128;
                if (Avx.IsSupported)
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
                    scalar128 = Avx.ExtractVector128(scalar, 0);
                }
                else
                {
                    float* scalarMem = stackalloc float[] { b, b, b, b };
                    scalar128 = Sse.LoadVector128(scalarMem);
                }
                while (calculated + Vector128<float>.Count < ret.Height * ret.Width)
                {
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
        public void MultipyThis(MatrixBase<float> ret, MatrixBase<float> a, float b)
        {
            if (Sse.IsSupported)
                MultiplyThisSse(ret, a, b);
            else
                for (int x = 0; x < a.Width; x++)
                    for (int y = 0; y < a.Height; y++)
                    {
                        ret[x, y] = a[x, y] * b;
                    }
        }

        private unsafe void AddThisSse(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            int calculated = 0;
            fixed (float* base_a = &a[0, 0], base_b = &b[0, 0], base_ret = &ret[0, 0])
            {
                if (Avx.IsSupported)
                {
                    while (calculated + Vector256<float>.Count <= ret.Width * ret.Height)
                    {
                        Vector256<float> solution =
                            Avx.Add(
                                Avx.LoadVector256(base_a + calculated),
                                Avx.LoadVector256(base_b + calculated)
                                );
                        Avx.Store(base_ret + calculated, solution);
                        calculated += Vector256<float>.Count;
                    }
                }
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
                throw new DimensionMismatchException();

            if (Sse.IsSupported)
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

        private unsafe void SubstractThisSse(MatrixBase<float> ret, MatrixBase<float> a, MatrixBase<float> b)
        {
            int calculated = 0;
            fixed (float* base_a = &a[0, 0], base_b = &b[0, 0], base_ret = &ret[0, 0])
            {
                if (Avx.IsSupported)
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
                }
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
                throw new DimensionMismatchException();

            if (Sse.IsSupported)
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
