using System;
using System.Diagnostics;

namespace Molytho.Matrix
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Vector<T> : MatrixBase<T>
    {
        private readonly T[] _data;

        public Vector(int size) : base(size, 1)
        {
            _data = new T[size];
        }
        public Vector(Dimension dimension) : base(dimension)
        {
            if (dimension.Width != 1)
                throw new DimensionMismatchException();

            _data = new T[dimension.Height];
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

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}
