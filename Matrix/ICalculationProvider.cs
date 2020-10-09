using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.Matrix
{
    public interface ICalculationProvider<T>
    {
        public Matrix<T> Inverse(Matrix<T> a);
        public Matrix<T> Add(Matrix<T> a, Matrix<T> b);
        public Matrix<T> Substract(Matrix<T> a, Matrix<T> b);
        public Matrix<T> Multipy(Matrix<T> a, Matrix<T> b);
        public Matrix<T> Multipy(Matrix<T> a, T b);
        public Matrix<T> Multipy(T a, Matrix<T> b);
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
                for(int x = 0; x < a.Width; x++)
                    for(int y = 0; y < a.Height; y++)
                    {
                        ret[x,y] = a[x, y] + b[x, y];
                    }

                return ret;
            }

            public Matrix<int> Inverse(Matrix<int> a)
            {
                throw new NotImplementedException();
            }

            public Matrix<int> Multipy(Matrix<int> a, Matrix<int> b)
            {
                throw new NotImplementedException();
            }

            public Matrix<int> Multipy(int a, Matrix<int> b) => Multipy(b, a);
            public Matrix<int> Multipy(Matrix<int> a, int b)
            {
                Matrix<int> ret = new Matrix<int>(a.Dimension);
                for (int x = 0; x < a.Width; x++)
                    for (int y = 0; y < a.Height; y++)
                    {
                        ret[x, y] = a[x, y] * b;
                    }
                return ret;
            }


            public Matrix<int> Substract(Matrix<int> a, Matrix<int> b)
            {
                if (!a.Dimension.Equals(b.Dimension))
                    throw new DimensionMismatchException();

                Matrix<int> ret = new Matrix<int>(a.Dimension);
                for (int x = 0; x < a.Width; x++)
                    for (int y = 0; y < a.Height; y++)
                    {
                        ret[x,y] = a[x, y] - b[x, y];
                    }
                return ret;
            }
        }
    }
}
