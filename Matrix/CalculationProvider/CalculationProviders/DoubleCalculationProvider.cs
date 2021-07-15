using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

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
        public Vector<double> Multiply(Vector<double> a, Vector<double> b)
        {
            if (a.Dimension != b.Dimension)
                ThrowHelper.ThrowDimensionMismatch(DimensionType.Row);

            Vector<double> ret = new Vector<double>(a.Dimension);

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

        #region Transpose(MatrixBase<double> a)
        public unsafe MatrixBase<double> Transpose(MatrixBase<double> a)
        {
            MatrixBase<double> ret =
                a.Height == 1
                ? new Vector<double>(a.Width) as MatrixBase<double>
                : new Matrix<double>(a.Width, a.Height) as MatrixBase<double>;
            if (a.Height == 1 || a.Width == 1)
            {
                fixed (double* base_a = &a[0, 0], base_ret = &ret[0, 0])
                    MemoryOperation.memcpy(base_ret, base_a, a.Height * a.Width * sizeof(double));
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
        private unsafe void TransposeAvx2(MatrixBase<double> ret, MatrixBase<double> a)
        {
            double* masks = stackalloc double[Vector256<double>.Count];
            Vector256<double> maskVect = Vector256<double>.Zero;
            int* indices = stackalloc int[]
            {
                0,
                a.Width * 1 * sizeof(double),
                a.Width * 2 * sizeof(double),
                a.Width * 3 * sizeof(double),
            };
            Vector128<int> indexVec;

            int indexScale = a.Width * 4 * sizeof(double);
            Vector128<int> indexVecInc = Avx2.BroadcastScalarToVector128(&indexScale);

            fixed (double* base_a = &a[0, 0], base_ret = &ret[0, 0])
            {
                for (int x = 0; x < a.Width; x++)
                {
                    fixed (double* base_ret_row = &ret[0, x])
                    {
                        int calculated = 0;
                        indexVec = Avx.LoadVector128(indices);
                        for (; calculated + Vector256<double>.Count <= a.Height; calculated += Vector256<double>.Count)
                        {
                            Vector256<double> storeVec = Avx2.GatherVector256(&base_a[x], indexVec, 1); //Load through column of source
                            Avx.Store(&base_ret_row[calculated], storeVec); //And save it linear as row in destination

                            //Increase out indices
                            indexVec = Avx2.Add(indexVec, indexVecInc);
                        }

                        if (a.Height - calculated > 1) // if we only have one value don't use vectors
                        {
                            if (x == 0) // We need to calculate the mask only once
                            {
                                // We have to set the MSB
                                for (int i = 0; calculated + i < a.Height; i++)
                                    ((byte*)&masks[i + 1])[-1] = 0b10000000;

                                maskVect = Avx2.LoadVector256(masks);
                            }

                            Vector256<double> storeVect = Avx2.GatherMaskVector256(Vector256<double>.Zero, &base_a[x], indexVec, maskVect, 1);
                            Avx2.MaskStore(&base_ret_row[calculated], maskVect, storeVect);
                        }
                        else if (a.Height - calculated == 1)
                        {
                            base_ret_row[calculated] = a[x, calculated];
                        }
                    }
                }
            }
        }
        #endregion

        public void InverseThis(MatrixBase<double> ret, MatrixBase<double> a)
        {
            throw new NotImplementedException();
        }

        #region MultiplyThis(MatrixBase<double> ret, MatrixBase<double> a, MatrixBase<double> b)
        public void MultiplyThis(MatrixBase<double> ret, MatrixBase<double> a, MatrixBase<double> b)
        {
            if (a.Width != b.Height || !(ret.Height == a.Height && ret.Width == b.Width))
                ThrowHelper.ThrowDimensionMismatch();

            if (Avx.IsSupported)
                MultiplyThisAvx(ret, a, b);
            else
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe Vector128<int> BroadcastScalarToVector128(int* value)
        {
            //We need this to emulate the missing instruction in AVX
            if (Avx2.IsSupported)
                return Avx2.BroadcastScalarToVector128(value);
            else
            {
                // 4 = 128 / 8 / sizeof(int)
                int* buffer = stackalloc int[4]
                {
                    *value,
                    *value,
                    *value,
                    *value,
                };
                return Avx.LoadVector128(buffer);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe Vector256<double> GatherVector256(double* baseAddress, Vector128<int> index, byte scale)
        {
            //We need this to emulate the missing instruction in AVX
            if (Avx2.IsSupported)
                return Avx2.GatherVector256(baseAddress, index, scale);
            else
            {
                double* buffer = stackalloc double[]
                {
                    baseAddress[index.GetElement(0) / sizeof(double)],
                    baseAddress[index.GetElement(1) / sizeof(double)],
                    baseAddress[index.GetElement(2) / sizeof(double)],
                    baseAddress[index.GetElement(3) / sizeof(double)],
                };
                return Avx.LoadVector256(buffer);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe Vector256<double> GatherMaskVector256(Vector256<double> source, double* baseAddress, Vector128<int> index, Vector256<double> mask, byte scale)
        {
            //We need this to emulate the missing instruction in AVX
            if (Avx2.IsSupported)
                return Avx2.GatherMaskVector256(source, baseAddress, index, mask, scale);
            else
            {
                double* buffer = stackalloc double[4];
                for (int i = 0; i < 4; i++)
                {
                    double maskValue = mask.GetElement(i);
                    byte* msb = (byte*)(&(&maskValue)[1]) - 1;
                    if (*msb >= 0b10000000)
                        buffer[i] = baseAddress[index.GetElement(i) / sizeof(double)];
                }
                return Avx.LoadVector256(buffer);
            }
        }
        private unsafe void MultiplyThisAvx(MatrixBase<double> ret, MatrixBase<double> a, MatrixBase<double> b)
        {
            double* masks = stackalloc double[Vector256<double>.Count];
            Vector256<double> masksVect = Vector256<double>.Zero;
            int* indices = stackalloc int[]
            {
                0 * b.Width * sizeof(double),
                1 * b.Width * sizeof(double),
                2 * b.Width * sizeof(double),
                3 * b.Width * sizeof(double),
            };

            int indexInc = 4 * b.Width * sizeof(double);
            Vector128<int> indexIncVect = BroadcastScalarToVector128(&indexInc);

            fixed (double* base_b = &b[0, 0])
                for (int y = 0; y < a.Height; y++) //First we go through Width the through Height
                {
                    fixed (double* base_a_row = &a[0, y])
                    {
                        for (int x = 0; x < b.Width; x++) //Do it for every column of b
                        {
                            int calculated = 0;
                            Vector256<double> storageVect = Vector256<double>.Zero;

                            Vector128<int> indexVect = Avx.LoadVector128(indices); //Load the indices needed for gather in b

                            for (; calculated + Vector256<double>.Count <= a.Width; calculated += Vector256<double>.Count) //Load without mask
                            {
                                Vector256<double> valuesAVect = Avx.LoadVector256(&base_a_row[calculated]);
                                Vector256<double> valuesBVect = GatherVector256(&base_b[x], indexVect, 1);

                                Vector256<double> product = Avx.Multiply(valuesAVect, valuesBVect);
                                storageVect = Avx.Add(storageVect, product); // Add this to our storage this wastes one instruction but code looks sooooo much better

                                indexVect = Avx.Add(indexVect, indexIncVect);
                            }

                            if (calculated < a.Width) //Load with mask
                            {
                                if (y == 0 && x == 0) //First time here. Calculate needed mask
                                {
                                    for (int i = 0; i < a.Width - calculated; i++)
                                        // We have to set the MSB
                                        ((byte*)&masks[i + 1])[-1] = 0b10000000;
                                    masksVect = Avx.LoadVector256(masks);
                                }

                                Vector256<double> valuesAVect = Avx.MaskLoad(&base_a_row[calculated], masksVect);
                                Vector256<double> valuesBVect = GatherMaskVector256(Vector256<double>.Zero, &base_b[x], indexVect, masksVect, 1);

                                Vector256<double> product = Avx.Multiply(valuesAVect, valuesBVect);
                                storageVect = Avx.Add(storageVect, product); // Add this to our storage. This works because masks loads set masked values to 0
                            }

                            //Add out storage to an scalar
                            storageVect = Avx.HorizontalAdd(storageVect, storageVect); // (a1 * a2, b1 * b2, a3 * a4, b3 * b4) = (a1, a2, a3, a4) * (b1, b2, b3, b4)
                            Vector128<double> split1 = Avx.ExtractVector128(storageVect, 0);
                            Vector128<double> split2 = Avx.ExtractVector128(storageVect, 1);
                            double solution = Avx.Add(split1, split2).ToScalar();

                            //Now save solution to ret
                            ret[x, y] = solution;
                        }
                    }
                }
        }
        #endregion

        #region MultiplyThis(Vector<double> ret, Vector<double> a, Vector<double> b)
        public void MultiplyThis(Vector<double> ret, Vector<double> a, Vector<double> b)
        {
            if (a.Dimension != b.Dimension || ret.Dimension != a.Dimension)
                ThrowHelper.ThrowDimensionMismatch(DimensionType.Row);

            if (Sse2.IsSupported)
                MultiplyThisSse2(ret, a, b);
            else
                for (int x = 0; x < a.Width; x++)
                    for (int y = 0; y < a.Height; y++)
                    {
                        ret[x, y] = a[x, y] * b[x, y];
                    }
        }
        public unsafe void MultiplyThisSse2(Vector<double> ret, Vector<double> a, Vector<double> b)
        {
            int calculated = 0;
            fixed (double* base_a = &a[0], base_b = &b[0], base_ret = &ret[0])
            {
                if (Avx.IsSupported)
                {
                    while (calculated + Vector256<double>.Count <= a.Size)
                    {
                        Vector256<double> aVect = Avx.LoadVector256(&base_a[calculated]);
                        Vector256<double> bVect = Avx.LoadVector256(&base_b[calculated]);
                        Vector256<double> product = Avx.Multiply(aVect, bVect);
                        Avx.Store(&base_ret[calculated], product);

                        calculated += Vector256<double>.Count;
                    }
                }
                while (calculated + Vector128<double>.Count <= a.Size)
                {
                    Vector128<double> aVect = Sse2.LoadVector128(&base_a[calculated]);
                    Vector128<double> bVect = Sse2.LoadVector128(&base_b[calculated]);
                    Vector128<double> product = Sse2.Multiply(aVect, bVect);
                    Sse2.Store(&base_ret[calculated], product);

                    calculated += Vector128<double>.Count;
                }
                for (; calculated < a.Size; calculated++)
                {
                    base_ret[calculated] = a[calculated] * b[calculated];
                }
            }
        }
        #endregion
        public void MultiplyThis(MatrixBase<double> ret, MatrixBase<double> a, double b)
        {
            for (int x = 0; x < a.Width; x++)
                for (int y = 0; y < a.Height; y++)
                {
                    ret[x, y] = a[x, y] * b;
                }
        }

        private unsafe void AddThisSse2(MatrixBase<double> ret, MatrixBase<double> a, MatrixBase<double> b)
        {
            int calculated = 0;
            fixed (double* base_a = &a[0, 0], base_b = &b[0, 0], base_ret = &ret[0, 0])
            {
                if (Avx.IsSupported)
                {
                    while (calculated + Vector256<double>.Count <= ret.Width * ret.Height)
                    {
                        Vector256<double> solution =
                            Avx.Add(
                                Avx.LoadVector256(base_a + calculated),
                                Avx.LoadVector256(base_b + calculated)
                                );
                        Avx.Store(base_ret + calculated, solution);
                        calculated += Vector256<double>.Count;
                    }
                }
                while (calculated + Vector128<double>.Count <= ret.Width * ret.Height)
                {
                    Vector128<double> solution =
                        Sse2.Add(
                            Sse2.LoadVector128(base_a + calculated),
                            Sse2.LoadVector128(base_b + calculated)
                            );
                    Sse2.Store(base_ret + calculated, solution);
                    calculated += Vector128<double>.Count;
                }
                for (; calculated < ret.Width * ret.Height; calculated++)
                {
                    *(base_ret + calculated) = *(base_a + calculated) + *(base_b + calculated);
                }
            }
        }
        public void AddThis(MatrixBase<double> ret, MatrixBase<double> a, MatrixBase<double> b)
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

        private unsafe void SubstractThisSse2(MatrixBase<double> ret, MatrixBase<double> a, MatrixBase<double> b)
        {
            int calculated = 0;
            fixed (double* base_a = &a[0, 0], base_b = &b[0, 0], base_ret = &ret[0, 0])
            {
                if (Avx.IsSupported)
                {
                    while (calculated + Vector256<double>.Count <= ret.Width * ret.Height)
                    {
                        Vector256<double> solution =
                            Avx.Subtract(
                                Avx.LoadVector256(base_a + calculated),
                                Avx.LoadVector256(base_b + calculated)
                                );
                        Avx.Store(base_ret + calculated, solution);
                        calculated += Vector256<double>.Count;
                    }
                }
                while (calculated + Vector128<double>.Count <= ret.Width * ret.Height)
                {
                    Vector128<double> solution =
                        Sse2.Subtract(
                            Sse2.LoadVector128(base_a + calculated),
                            Sse2.LoadVector128(base_b + calculated)
                            );
                    Sse2.Store(base_ret + calculated, solution);
                    calculated += Vector128<double>.Count;
                }
                for (; calculated < ret.Width * ret.Height; calculated++)
                {
                    *(base_ret + calculated) = *(base_a + calculated) - *(base_b + calculated);
                }
            }
        }
        public void SubstractThis(MatrixBase<double> ret, MatrixBase<double> a, MatrixBase<double> b)
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
