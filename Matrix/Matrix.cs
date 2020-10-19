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

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
