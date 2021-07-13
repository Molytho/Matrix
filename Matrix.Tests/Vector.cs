using Xunit;
using Molytho.Matrix;
using System;

namespace Molytho.UnitTests.Matrix
{
    public class VectorTests
    {
        const int dimensionBoundries = 20;
        const int randomMin = int.MinValue;
        const int randomMax = int.MaxValue;
        static Random random = new();

        [Fact]
        public void TestIndexer()
        {
            int size;
            Vector<int> vector;
            for (size = 1; size <= dimensionBoundries; size++)
            {

                int[] testStorage = new int[size];
                vector = new Vector<int>(size);

                for (int i = 0; i < size; i++)
                {
                    vector[i] = testStorage[i] = random.Next(randomMin, randomMax);
                }

                for (int i = 0; i < size; i++)
                {
                    Assert.Equal(testStorage[i], vector[0, i]);
                    Assert.Equal(testStorage[i], vector[i]);
                }

            }

            size = random.Next(1, 41);
            vector = new Vector<int>(size);

            Assert.Throws<IndexOutOfRangeException>(() => vector[-1, 0]);
            Assert.Throws<IndexOutOfRangeException>(() => vector[0, -51]);
            Assert.Throws<IndexOutOfRangeException>(() => vector[-1, -1]);
            Assert.Throws<IndexOutOfRangeException>(() => vector[0, size]);
            Assert.Throws<IndexOutOfRangeException>(() => vector[1, 0]);
        }

        [Fact]
        public void TestDimension()
        {
            int size = random.Next(1, 41);
            Vector<int> vector = new Vector<int>(size);

            Assert.True(vector.Width == vector.Dimension.Width);
            Assert.True(vector.Height == vector.Dimension.Height);

            Assert.True(vector.Height == size);
            Assert.True(vector.Width == 1);
        }

        [Fact]
        public void TestConstructor()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector<int>(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector<int>(-19));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector<int>(new Dimension(1, 0)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector<int>(new Dimension(0, 0)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector<int>(new Dimension(1, -100)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector<int>(new Dimension(0, 1)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Vector<int>(new Dimension(-19, 1)));

            Assert.Throws<DimensionMismatchException>(() => new Vector<int>(new Dimension(7, 2)));
            Assert.Throws<DimensionMismatchException>(() => new Vector<int>(new Dimension(7, 100)));

            Vector<int> m1, m2;
            int size = random.Next(1, 41);
            m1 = new Vector<int>(size);
            m2 = new Vector<int>(new Dimension(size, 1));
            Assert.True(m1.Height == m2.Height);
            Assert.True(m1.Width == m2.Width);
            Assert.True(m1.Dimension == m2.Dimension);

            int[] initValues = new int[size];
            for (int i = 0; i < size; i++)
                initValues[i] = random.Next();
            Vector<int> m3 = new Vector<int>(initValues);
            Assert.True(m3.Height == size);
            Assert.True(m3.Width == 1);
            for (int i = 0; i < size; i++)
                Assert.True(m3[i] == initValues[i]);
            Assert.Same((int[])m3, initValues);
            Vector<int> m4 = initValues;
            Assert.Same((int[])m4, initValues);
        }

        [Fact]
        public void TestToString()
        {
            Vector<int> vector;
            int size = random.Next(1, 41);
            vector = new Vector<int>(size);

            for (int i = 0; i < size; i++)
                    vector[i] = random.Next();

            string str = vector.ToString();
            Assert.NotEmpty(str);
            Assert.False(str.Length < 5); // Has to be at least {{0}}
        }
    }
}