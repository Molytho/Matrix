using System;
using System.Runtime.Serialization;

namespace Molytho.Matrix
{
    public enum DimensionType
    {
        Row = 0,
        Column = 1,
        Unspecified = 2,
    }
    public class DimensionMismatchException : Exception
    {
        public DimensionType Dimension { get; }
        public DimensionMismatchException(DimensionType dimension, string message = "") : base(message)
        {
            Dimension = dimension;
        }
    }

    internal static class ThrowHelper
    {
        private static readonly DimensionMismatchException[] dimensionMismatchExceptionCache = new DimensionMismatchException[3];
        public static DimensionMismatchException ThrowDimensionMismatch(DimensionType dimension = DimensionType.Unspecified)
            => throw (dimensionMismatchExceptionCache[(int)dimension] ??= new DimensionMismatchException(dimension));
    }
}
