using System.Text;
using System.Threading;

namespace Molytho.Matrix
{
    public interface ICalculationProvider<T>
    {
        public Matrix<T> Inverse(Matrix<T> a);
        public Matrix<T> Add(Matrix<T> a, Matrix<T> b);
        public Matrix<T> Substract(Matrix<T> a, Matrix<T> b);
        public Matrix<T> Multipy(Matrix<T> a, Matrix<T> b);
        public Matrix<T> Multipy(Matrix<T> a, T b);

        public void InverseThis(Matrix<T> ret, Matrix<T> a);
        public void AddThis(Matrix<T> ret, Matrix<T> a, Matrix<T> b);
        public void SubstractThis(Matrix<T> ret, Matrix<T> a, Matrix<T> b);
        public void MultipyThis(Matrix<T> ret, Matrix<T> a, Matrix<T> b);
        public void MultipyThis(Matrix<T> ret, Matrix<T> a, T b);
    }
}
