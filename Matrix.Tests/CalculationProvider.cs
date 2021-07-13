using System;
using System.Runtime.CompilerServices;
using Molytho.Matrix;
using Molytho.Matrix.Calculation;
using Xunit;

namespace Molytho.UnitTests.Matrix
{
    public abstract class CalculationProviderHelper<T>
    {
        protected sealed record MatrixInput3(Matrix<T> M1, Matrix<T> M2, Matrix<T> M3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void HelperAdd(MatrixBase<T> a, MatrixBase<T> b, MatrixBase<T> result)
        {
            Assert.Equal(result, a + b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void HelperSubstract(MatrixBase<T> a, MatrixBase<T> b, MatrixBase<T> result)
        {
            Assert.Equal(result, a - b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void HelperMultiply(MatrixBase<T> a, MatrixBase<T> b, MatrixBase<T> result)
        {
            Assert.Equal(result, a * b);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void HelperMultiply(MatrixBase<T> a, T b, MatrixBase<T> result)
        {
            Assert.Equal(result, a * b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void HelperInverse(MatrixBase<T> a, MatrixBase<T> result)
        {
            a.Inverse();
            Assert.Equal(result, a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void HelperTranspose(MatrixBase<T> a, MatrixBase<T> result)
        {
            Assert.Equal(result, a.Transpose);
        }
    }
}