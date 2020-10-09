using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Molytho.Matrix
{
    public interface ICalculationProvider<T>
    {
        public Matrix<T> Inverse(Matrix<T> a);
        public Matrix<T> Add(Matrix<T> a, Matrix<T> b);
        public Matrix<T> Substract(Matrix<T> a, Matrix<T> b);
        public Matrix<T> Multipy(Matrix<T> a, Matrix<T> b);
        public Matrix<T> Multipy(Matrix<T> a, T b);

        public void InverseThis(Matrix<T> ret, Matrix<T> a);
        public void AddThis(Matrix<T> ret, Matrix<T> a, Matrix<T> b);
        public void SubstractThis(Matrix<T> ret, Matrix<T> a, Matrix<T> b);
        public void MultipyThis(Matrix<T> ret, Matrix<T> a, Matrix<T> b);
        public void MultipyThis(Matrix<T> ret, Matrix<T> a, T b);
    }
    public static class CalculationProvider<T>
    {
        private static ICalculationProvider<T>? _provider;
        internal static ICalculationProvider<T> Provider
        {
            get
            {
                return _provider ??= CalculationProvider.TryGetCalculationProvider<T>();
            }
        }
    }
    public static class CalculationProvider
    {
        private static readonly Dictionary<Type, Type> _providers =
            new Dictionary<Type, Type>()
            {
                { typeof(int), typeof(IntCalculationProvider) }
            };
        internal static ICalculationProvider<T> TryGetCalculationProvider<T>()
        {
            Type providerType;

            try
            {
                providerType = _providers[typeof(T)];
            }
            catch (KeyNotFoundException e)
            {
                throw new NotSupportedException("No calculation provider found for this type", e);
            }

            return (ICalculationProvider<T>)providerType.GetConstructor(types: null).Invoke(null);
        }

        private class IntCalculationProvider : ICalculationProvider<int>
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
                if (!a.Dimension.Equals(b.Dimension) || !a.Dimension.Equals(ret))
                    throw new DimensionMismatchException();

                for (int x = 0; x < a.Width; x++)
                    for (int y = 0; y < a.Height; y++)
                    {
                        ret[x, y] = a[x, y] + b[x, y];
                    }
            }
            public void SubstractThis(Matrix<int> ret, Matrix<int> a, Matrix<int> b)
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
}
