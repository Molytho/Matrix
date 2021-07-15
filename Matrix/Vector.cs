using System;
using System.Diagnostics;
using Molytho.Matrix.Calculation;

namespace Molytho.Matrix
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Vector<T> : MatrixBase<T>
        where T : notnull
    {
        private readonly T[] _data;

        public Vector(int size) : base(size, 1)
        {
            _data = new T[size];
        }
        public Vector(Dimension dimension) : base(dimension)
        {
            if (dimension.Width != 1)
                ThrowHelper.ThrowDimensionMismatch(DimensionType.Column);

            _data = new T[dimension.Height];
        }
        public Vector(T[] initialValues) : base(initialValues.Length, 1)
        {
            if (initialValues == null)
                throw new ArgumentNullException(nameof(initialValues));

            _data = initialValues;
        }

        public int Size => base.Height;

        public override ref T this[int x, int y]
        {
            get
            {
                if (x != 0)
                    throw new IndexOutOfRangeException();

                return ref _data[y];
            }
        }
        public ref T this[int index]
            => ref _data[index];

        public static Vector<T> operator +(Vector<T> a, Vector<T> b) => (Vector<T>)CalculationProvider<T>.Provider.Add(a, b);
        public static Vector<T> operator -(Vector<T> a, Vector<T> b) => (Vector<T>)CalculationProvider<T>.Provider.Substract(a, b);
        public static Vector<T> operator *(MatrixBase<T> a, Vector<T> b) => (Vector<T>)CalculationProvider<T>.Provider.Multiply(a, b);
        public static Vector<T> operator *(Vector<T> a, Vector<T> b) => CalculationProvider<T>.Provider.Multiply(a, b);
        public static Vector<T> operator *(Vector<T> a, T b) => (Vector<T>)CalculationProvider<T>.Provider.Multiply(a, b);
        public static Vector<T> operator *(T a, Vector<T> b) => (Vector<T>)CalculationProvider<T>.Provider.Multiply(b, a);

        public static implicit operator Vector<T>(T[] values) => new Vector<T>(values);
        public static explicit operator T[](Vector<T> vector) => vector._data;

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
