using System.Text;
using Molytho.Matrix.Calculation;

namespace Molytho.Matrix
{
    public abstract class MatrixBase<T>
    {
        public MatrixBase(int height, int width)
        {
            _dimension = new Dimension(height, width);
        }
        public MatrixBase(Dimension dimension)
        {
            _dimension = dimension;
        }

        public abstract ref T this[int x, int y] { get; }

        private readonly Dimension _dimension;
        public Dimension Dimension => _dimension;
        public int Height => Dimension.Height;
        public int Width => Dimension.Width;

        public virtual MatrixBase<T> Transpose => CalculationProvider<T>.Provider.Transpose(this);

        public virtual void Inverse() => CalculationProvider<T>.Provider.InverseThis(this, this);
        public virtual void Add(MatrixBase<T> other) => CalculationProvider<T>.Provider.AddThis(this, this, other);
        public virtual void Substract(MatrixBase<T> other) => CalculationProvider<T>.Provider.SubstractThis(this, this, other);
        public virtual void Multipy(MatrixBase<T> other) => CalculationProvider<T>.Provider.MultipyThis(this, this, other);
        public virtual void Multipy(T scalar) => CalculationProvider<T>.Provider.MultipyThis(this, this, scalar);

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
    }
}