using System;
using System.Collections.Generic;

namespace Molytho.Matrix
{
    public static class CalculationProvider<T>
    {
        private static ICalculationProvider<T>? _provider;
        internal static ICalculationProvider<T> Provider => _provider ??= CalculationProvider.TryGetCalculationProvider<T>();
    }

    public static partial class CalculationProvider
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
    }
}
