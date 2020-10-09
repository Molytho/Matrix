using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.Matrix
{
    public interface ICalculationProvider<T>
    {
        public void Inverse(Matrix<T> a);
        public void Add(Matrix<T> a, Matrix<T> b);
        public void Substract(Matrix<T> a, Matrix<T> b);
        public void Multipy(Matrix<T> a, Matrix<T> b);
        public void Multipy(Matrix<T> a, T b);
        public void Multipy(T a, Matrix<T> b);
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
            public void Add(Matrix<int> a, Matrix<int> b)
            {
                if (!a.Dimension.Equals(b.Dimension))
                    throw new DimensionMismatchException();

                for(int x = 0; x < a.Width; x++)
                    for(int y = 0; y < a.Height; y++)
                    {
                        a[x, y] += b[x, y];
                    }
            }

            public void Inverse(Matrix<int> a)
            {
                throw new NotImplementedException();
            }

            public void Multipy(Matrix<int> a, Matrix<int> b)
            {
                throw new NotImplementedException();
            }

            public void Multipy(int a, Matrix<int> b) => Multipy(b, a);
            public void Multipy(Matrix<int> a, int b)
            {
                for (int x = 0; x < a.Width; x++)
                    for (int y = 0; y < a.Height; y++)
                    {
                        a[x, y] *= b;
                    }
            }


            public void Substract(Matrix<int> a, Matrix<int> b)
            {
                if (!a.Dimension.Equals(b.Dimension))
                    throw new DimensionMismatchException();

                for (int x = 0; x < a.Width; x++)
                    for (int y = 0; y < a.Height; y++)
                    {
                        a[x, y] -= b[x, y];
                    }
            }
        }
    }
}
