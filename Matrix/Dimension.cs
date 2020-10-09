using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.Matrix
{
    public readonly struct Dimension
    {
        public Dimension(int height, int width)
        {
            Height = height;
            Width = width;
        }

        public readonly int Height { get; }
        public readonly int Width { get; }
    }
}
