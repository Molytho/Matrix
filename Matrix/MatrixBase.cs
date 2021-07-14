using System;
using System.Text;
using Molytho.Matrix.Calculation;

namespace Molytho.Matrix
{
    public abstract class MatrixBase<T> : IEquatable<MatrixBase<T>>
        where T : notnull
    {
        public MatrixBase(int height, int width)
        {
            if (height <= 0 || width <= 0)
                throw new ArgumentOutOfRangeException();

            Dimension = new Dimension(height, width);
        }
        public MatrixBase(Dimension dimension)
        {
            if (!dimension.IsValid)
                throw new ArgumentOutOfRangeException();

            Dimension = dimension;
        }

        public abstract ref T this[int x, int y] { get; }

        public Dimension Dimension { get; }
        public int Height => Dimension.Height;
        public int Width => Dimension.Width;

        public virtual MatrixBase<T> Transpose => CalculationProvider<T>.Provider.Transpose(this);

        public virtual void Inverse() => CalculationProvider<T>.Provider.InverseThis(this, this);
        public virtual void Add(MatrixBase<T> other) => CalculationProvider<T>.Provider.AddThis(this, this, other);
        public virtual void Substract(MatrixBase<T> other) => CalculationProvider<T>.Provider.SubstractThis(this, this, other);
        public virtual void Multiply(MatrixBase<T> other) => CalculationProvider<T>.Provider.MultiplyThis(this, this, other);
        public virtual void Multiply(T scalar) => CalculationProvider<T>.Provider.MultiplyThis(this, this, scalar);

        public static MatrixBase<T> operator +(MatrixBase<T> a, MatrixBase<T> b) => CalculationProvider<T>.Provider.Add(a, b);
        public static MatrixBase<T> operator -(MatrixBase<T> a, MatrixBase<T> b) => CalculationProvider<T>.Provider.Substract(a, b);
        public static MatrixBase<T> operator *(MatrixBase<T> a, MatrixBase<T> b) => CalculationProvider<T>.Provider.Multiply(a, b);
        public static MatrixBase<T> operator *(MatrixBase<T> a, T b) => CalculationProvider<T>.Provider.Multiply(a, b);
        public static MatrixBase<T> operator *(T a, MatrixBase<T> b) => CalculationProvider<T>.Provider.Multiply(b, a);

        private static StringBuilder? _sb = null;
        private static StringBuilder sb => _sb ??= new StringBuilder();
        public override string ToString()
        {
            sb.Clear();
            sb.Append('{');
            for (int y = 0; y < Height; y++)
            {
                sb.Append('{');
                for (int x = 0; x < Width; x++)
                {
                    sb.Append(this[x, y]);
                    if (x < Width - 1)
                        sb.Append(", ");
                }
                sb.Append('}');
                if (y < Height - 1)
                    sb.Append(", ");
            }
            sb.Append('}');
            return sb.ToString();
        }

        public virtual bool Equals(MatrixBase<T>? other)
        {
            if (other is null)
                return false;

            if (!Dimension.Equals(other.Dimension))
                return false;

            for (int y = 0; y < Dimension.Height; y++)
                for (int x = 0; x < Dimension.Width; x++)
                    if (!this[x, y].Equals(other[x, y]))
                        return false;

            return true;
        }
    }
}