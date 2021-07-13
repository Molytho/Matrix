using System;
using Molytho.Matrix;
using Xunit;

namespace Molytho.UnitTests.Matrix
{
    public class IntCalculationProvider : CalculationProviderHelper<int>
    {
        [Fact]
        public void TestAdd()
        {
            MatrixInput3[] inputs =
            {
                new MatrixInput3
                (
                    new int[,]{{1012374,1037197},{1050599,1112890}},
                    new int[,]{{1236093,72540},{845062,51057}},
                    new int[,]{{2248467, 1109737}, {1895661, 1163947}}
                ),
                new MatrixInput3
                (
                    new int[,]{{131305,1052834},{805299,669307}},
                    new int[,]{{842151,1593357},{1855646,1953061}},
                    new int[,]{{973456, 2646191}, {2660945, 2622368}}
                ),
                new MatrixInput3
                (
                    new int[,]{{1973460,235416},{1873653,1312046},{1092682,758346}},
                    new int[,]{{1840525,1498344},{1426583,1545091},{1260516,1989954}},
                    new int[,]{{3813985, 1733760}, {3300236, 2857137}, {2353198, 2748300}}
                ),
                new MatrixInput3
                (
                    new int[,]{{1090924,151525,1385267,1905711},{1720299,1919949,606945,986322},{1857376,1683467,1749295,685363}},
                    new int[,]{{1462523,125142,1989364,1727489},{1260039,22140,124655,972345},{1113811,1403809,160162,1896816}},
                    new int[,]{{2553447, 276667, 3374631, 3633200}, {2980338, 1942089, 731600, 1958667}, {2971187, 3087276, 1909457, 2582179}}
                ),
                new MatrixInput3
                (
                    new int[,]{{1841561,825681,528558,673743}},
                    new int[,]{{1259720,1368474,63463,477415}},
                    new int[,]{{3101281, 2194155, 592021, 1151158}}
                ),
                new MatrixInput3
                (
                    new int[,]{{1690320,1229662,1243656},{811996,1833176,826311},{885842,1886789,848343}},
                    new int[,]{{655256,1064783,1188089},{1397484,1125799,1610467},{953822,1901463,721735}},
                    new int[,]{{2345576, 2294445, 2431745}, {2209480, 2958975, 2436778}, {1839664, 3788252, 1570078}}
                ),
            };

            for (int i = 0; i < inputs.Length; i++)
                Assert.Equal(inputs[i].M3, inputs[i].M2 + inputs[i].M1);
        }
    }
}