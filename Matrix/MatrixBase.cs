namespace Molytho.Matrix
{
    public abstract class MatrixBase<T>
    {
        public abstract ref T this[int x, int y] { get; }

        public abstract Dimension Dimension { get; }
        public abstract int Height { get; }
        public abstract MatrixBase<T> Transpose { get; }
        public abstract int Width { get; }

        public abstract void Add(MatrixBase<T> other);
        public abstract void Inverse();
        public abstract void Multipy(MatrixBase<T> other);
        public abstract void Multipy(T scalar);
        public abstract void Substract(MatrixBase<T> other);
    }
}