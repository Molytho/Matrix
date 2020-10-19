using Molytho.Matrix.Calculation;
using System.Diagnostics;
using System.Text;

namespace Molytho.Matrix
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Matrix<T> : MatrixBase<T>
    {
        private readonly T[,] _data;

        public Matrix(int height, int width) : base(height, width)
        {
            _data = new T[height, width];
        }
        public Matrix(Dimension dimension) : base(dimension)
        {
            _data = new T[dimension.Height, dimension.Width];
        }

        public override ref T this[int x, int y]
            => ref _data[y, x];

        public static MatrixBase<T> operator +(Matrix<T> a, Matrix<T> b) => CalculationProvider<T>.Provider.Add(a, b);
        public static MatrixBase<T> operator -(Matrix<T> a, Matrix<T> b) => CalculationProvider<T>.Provider.Substract(a, b);
        public static MatrixBase<T> operator *(Matrix<T> a, Matrix<T> b) => CalculationProvider<T>.Provider.Multipy(a, b);
        public static MatrixBase<T> operator *(Matrix<T> a, T b) => CalculationProvider<T>.Provider.Multipy(a, b);
        public static MatrixBase<T> operator *(T a, Matrix<T> b) => CalculationProvider<T>.Provider.Multipy(b, a);

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
