namespace Molytho.Matrix.Calculation
{
    public interface ICalculationProvider<T>
        where T : notnull
    {
        public MatrixBase<T> Inverse(MatrixBase<T> a);
        public MatrixBase<T> Add(MatrixBase<T> a, MatrixBase<T> b);
        public MatrixBase<T> Substract(MatrixBase<T> a, MatrixBase<T> b);
        public MatrixBase<T> Multiply(MatrixBase<T> a, MatrixBase<T> b);
        public MatrixBase<T> Multiply(MatrixBase<T> a, T b);
        public Vector<T> Multiply(Vector<T> a, Vector<T> b);
        public MatrixBase<T> Transpose(MatrixBase<T> a);

        public void InverseThis(MatrixBase<T> ret, MatrixBase<T> a);
        public void AddThis(MatrixBase<T> ret, MatrixBase<T> a, MatrixBase<T> b);
        public void SubstractThis(MatrixBase<T> ret, MatrixBase<T> a, MatrixBase<T> b);
        public void MultiplyThis(MatrixBase<T> ret, MatrixBase<T> a, MatrixBase<T> b);
        public void MultiplyThis(MatrixBase<T> ret, MatrixBase<T> a, T b);
        public void MultiplyThis(Vector<T> ret, Vector<T> a, Vector<T> b);

    }
}
