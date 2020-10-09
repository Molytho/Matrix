using System;
using Molytho.Matrix;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int e;
            MatrixBase<int> c, f, i;
            Matrix<int> a = new Matrix<int>(3, 3), b = new Matrix<int>(3, 3), d = new Matrix<int>(3, 3), g = new Matrix<int>(2, 3), h = new Matrix<int>(3, 2);

            a[0, 0] = 1;
            a[0, 1] = 4;
            a[1, 2] = 123;
            a[1, 1] = 11;
            a[0, 2] = 15;
            a[2, 0] = 191;
            a[2, 1] = 592;
            a[2, 2] = 72;

            b[0, 0] = 5;
            b[0, 1] = 12;
            b[1, 2] = 13123;
            b[1, 1] = 1;
            b[0, 2] = 121;
            b[2, 0] = 11;
            b[2, 1] = 502;
            b[2, 2] = 2;

            c = a + b;

            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(c);

            Console.WriteLine();

            d[0, 0] = 1;
            d[1, 1] = 2;
            d[2, 2] = 3;
            d[0, 1] = 11;
            d[1, 2] = 22;

            e = 7;

            f = d * e;

            Console.WriteLine(d);
            Console.WriteLine(e);
            Console.WriteLine(f);

            Console.WriteLine();

            g[0, 0] = 11;
            g[1, 0] = 22;
            g[2, 0] = 33;
            g[0, 1] = 44;
            g[1, 1] = 55;
            g[2, 1] = 66;

            h[0, 0] = 1;
            h[0, 1] = 2;
            h[0, 2] = 3;
            h[1, 0] = 4;
            h[1, 1] = 5;
            h[1, 2] = 6;

            i = g * h;

            Console.WriteLine(g);
            Console.WriteLine(h);
            Console.WriteLine(i);
        }
    }
}
