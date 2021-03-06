﻿using System;
using System.Collections.Generic;
using Molytho.Matrix.Calculation.Providers;

namespace Molytho.Matrix.Calculation
{
    public static class CalculationProvider<T>
        where T : notnull
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
                { typeof(double), typeof(DoubleCalculationProvider) },
                { typeof(float), typeof(FloatCalculationProvider) }
            };
        internal static ICalculationProvider<T> TryGetCalculationProvider<T>()
            where T : notnull
        {
            Type providerType;

            try
            {
                providerType = _providers[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                throw new NotSupportedException("No calculation provider found for this type");
            }

            return (ICalculationProvider<T>)
                        (providerType.GetConstructor(Type.EmptyTypes)?.Invoke(null)
                        ?? throw new NotSupportedException("Calculation provider has no default constructor"));
        }
    }
}
