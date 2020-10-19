using Molytho.Matrix.Calculation;
using System.Diagnostics;
using System.Text;

namespace Molytho.Matrix
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Matrix<T> : MatrixBase<T>
    {
        private readonly Dimension _dimension;
        private readonly T[,] _data;

        public Matrix(int height, int width)
        {
            _dimension = new Dimension(height, width);
            _data = new T[height, width];
        }
        public Matrix(Dimension dimension)
        {
            _dimension = dimension;
            _data = new T[dimension.Height, dimension.Width];
        }

        public override Dimension Dimension => _dimension;
        public override int Height => _dimension.Height;
        public override int Width => _dimension.Width;

        public override ref T this[int x, int y]
            => ref _data[y, x];

        public override MatrixBase<T> Transpose => CalculationProvider<T>.Provider.Transpose(this);

        public static MatrixBase<T> operator +(Matrix<T> a, Matrix<T> b) => CalculationProvider<T>.Provider.Add(a, b);
        public static MatrixBase<T> operator -(Matrix<T> a, Matrix<T> b) => CalculationProvider<T>.Provider.Substract(a, b);
        public static MatrixBase<T> operator *(Matrix<T> a, Matrix<T> b) => CalculationProvider<T>.Provider.Multipy(a, b);
        public static MatrixBase<T> operator *(Matrix<T> a, T b) => CalculationProvider<T>.Provider.Multipy(a, b);
        public static MatrixBase<T> operator *(T a, Matrix<T> b) => CalculationProvider<T>.Provider.Multipy(b, a);

        public override void Inverse() => CalculationProvider<T>.Provider.InverseThis(this, this);
        public override void Add(MatrixBase<T> other) => CalculationProvider<T>.Provider.AddThis(this, this, other);
        public override void Substract(MatrixBase<T> other) => CalculationProvider<T>.Provider.SubstractThis(this, this, other);
        public override void Multipy(MatrixBase<T> other) => CalculationProvider<T>.Provider.MultipyThis(this, this, other);
        public override void Multipy(T scalar) => CalculationProvider<T>.Provider.MultipyThis(this, this, scalar);

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('{');
            for (int y = 0; y < Height; y++)
            {
                builder.Append('{');
                for (int x = 0; x < Width; x++)
                {
                    builder.Append(this[x, y]);
                    if (x < Width - 1)
                        builder.Append(", ");
                }
                builder.Append('}');
                if (y < Height - 1)
                    builder.Append(", ");
            }
            builder.Append('}');
            return builder.ToString();
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
