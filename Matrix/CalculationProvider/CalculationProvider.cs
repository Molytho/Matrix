using System;
using System.Collections.Generic;
using Molytho.Matrix.Calculation.Providers;

namespace Molytho.Matrix.Calculation
{
    public static class CalculationProvider<T>
    {
        private static ICalculationProvider<T>? _provider;
        internal static ICalculationProvider<T> Provider => _provider ??= CalculationProvider.TryGetCalculationProvider<T>();
    }

    public static class CalculationProvider
    {
        private static readonly Dictionary<Type, Type> _providers =
            new Dictionary<Type, Type>()
            {
                { typeof(int), typeof(IntCalculationProvider) },
                { typeof(double), typeof(DoubleCalculationProvider) }
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
    }
}
