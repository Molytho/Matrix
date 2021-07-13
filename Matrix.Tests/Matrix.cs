using Xunit;
using Molytho.Matrix;
using System;

namespace Molytho.UnitTests.Matrix
{
    public class MatrixTests
    {
        const int dimensionBoundries = 20;
        const int randomMin = int.MinValue;
        const int randomMax = int.MaxValue;
        static Random random = new();

        [Fact]
        public void TestIndexer()
        {
            int xSize, ySize;
            Matrix<int> matrix;
            for (xSize = 1; xSize <= dimensionBoundries; xSize++)
            {
                for (ySize = 1; ySize <= dimensionBoundries; ySize++)
                {
                    int[,] testStorage = new int[xSize, ySize];
                    matrix = new Matrix<int>(ySize, xSize);

                    for (int x = 0; x < xSize; x++)
                    {
                        for (int y = 0; y < ySize; y++)
                        {
                            matrix[x, y] = testStorage[x, y] = random.Next(randomMin, randomMax);
                        }
                    }

                    for (int x = 0; x < xSize; x++)
                    {
                        for (int y = 0; y < ySize; y++)
                        {
                            Assert.True(matrix[x, y] == testStorage[x, y]);
                        }
                    }
                }
            }

            xSize = random.Next(1, 41);
            ySize = random.Next(1, 41);
            matrix = new Matrix<int>(ySize, xSize);

            Assert.Throws<IndexOutOfRangeException>(() => matrix[-1, 0]);
            Assert.Throws<IndexOutOfRangeException>(() => matrix[0, -51]);
            Assert.Throws<IndexOutOfRangeException>(() => matrix[-1, -1]);
            Assert.Throws<IndexOutOfRangeException>(() => matrix[0, ySize]);
            Assert.Throws<IndexOutOfRangeException>(() => matrix[xSize, 0]);
            Assert.Throws<IndexOutOfRangeException>(() => matrix[xSize, ySize]);
            Assert.Throws<IndexOutOfRangeException>(() => matrix[xSize + 100, ySize]);
        }

        [Fact]
        public void TestDimension()
        {
            int xSize = random.Next(1, 41);
            int ySize = random.Next(1, 41);
            Matrix<int> matrix = new Matrix<int>(ySize, xSize);

            Assert.True(matrix.Width == matrix.Dimension.Width);
            Assert.True(matrix.Height == matrix.Dimension.Height);

            Assert.True(matrix.Height == ySize);
            Assert.True(matrix.Width == xSize);
        }

        [Fact]
        public void TestConstructor()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix<int>(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix<int>(1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix<int>(0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix<int>(1, -100));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix<int>(-19, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix<int>(-19, -100));

            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix<int>(new Dimension(0, 1)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix<int>(new Dimension(1, 0)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix<int>(new Dimension(0, 0)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix<int>(new Dimension(1, -100)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix<int>(new Dimension(-19, 1)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Matrix<int>(new Dimension(-19, -100)));

            Matrix<int> m1, m2;
            int xSize = random.Next(1, 41), ySize = random.Next(1, 41);
            m1 = new Matrix<int>(ySize, xSize);
            m2 = new Matrix<int>(new Dimension(ySize, xSize));
            Assert.True(m1.Height == m2.Height);
            Assert.True(m1.Width == m2.Width);
            Assert.True(m1.Dimension == m2.Dimension);

            int[,] initValues = new int[ySize, xSize];
            for (int x = 0; x < xSize; x++)
                for (int y = 0; y < ySize; y++)
                    initValues[y, x] = random.Next();
            Matrix<int> m3 = new Matrix<int>(initValues);
            Assert.True(m3.Height == ySize);
            Assert.True(m3.Width == xSize);
            for (int x = 0; x < xSize; x++)
                for (int y = 0; y < ySize; y++)
                    Assert.True(m3[x, y] == initValues[y, x]);
            Assert.Same((int[,])m3, initValues);
            Matrix<int> m4 = initValues;
            Assert.Same((int[,])m4, initValues);
        }

        [Fact]
        public void TestToString()
        {
            Matrix<int> matrix;
            int xSize = random.Next(1, 41), ySize = random.Next(1, 41);
            matrix = new Matrix<int>(ySize, xSize);

            for (int x = 0; x < xSize; x++)
                for (int y = 0; y < ySize; y++)
                    matrix[x, y] = random.Next();

            string str = matrix.ToString();
            Assert.NotEmpty(str);
            Assert.False(str.Length < 5); // Has to be at least {{0}}
        }
    }
}