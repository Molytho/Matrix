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
            (Matrix<int>, Matrix<int>, Matrix<int>)[] inputs =
            {
                (
                    new int[,]{{1012374,1037197},{1050599,1112890}},
                    new int[,]{{1236093,72540},{845062,51057}},
                    new int[,]{{2248467, 1109737}, {1895661, 1163947}}
                ),
                (
                    new int[,]{{131305,1052834},{805299,669307}},
                    new int[,]{{842151,1593357},{1855646,1953061}},
                    new int[,]{{973456, 2646191}, {2660945, 2622368}}
                ),
                (
                    new int[,]{{1973460,235416},{1873653,1312046},{1092682,758346}},
                    new int[,]{{1840525,1498344},{1426583,1545091},{1260516,1989954}},
                    new int[,]{{3813985, 1733760}, {3300236, 2857137}, {2353198, 2748300}}
                ),
                (
                    new int[,]{{1090924,151525,1385267,1905711},{1720299,1919949,606945,986322},{1857376,1683467,1749295,685363}},
                    new int[,]{{1462523,125142,1989364,1727489},{1260039,22140,124655,972345},{1113811,1403809,160162,1896816}},
                    new int[,]{{2553447, 276667, 3374631, 3633200}, {2980338, 1942089, 731600, 1958667}, {2971187, 3087276, 1909457, 2582179}}
                ),
                (
                    new int[,]{{1841561,825681,528558,673743}},
                    new int[,]{{1259720,1368474,63463,477415}},
                    new int[,]{{3101281, 2194155, 592021, 1151158}}
                ),
                (
                    new int[,]{{1690320,1229662,1243656},{811996,1833176,826311},{885842,1886789,848343}},
                    new int[,]{{655256,1064783,1188089},{1397484,1125799,1610467},{953822,1901463,721735}},
                    new int[,]{{2345576, 2294445, 2431745}, {2209480, 2958975, 2436778}, {1839664, 3788252, 1570078}}
                ),
            };

            for (int i = 0; i < inputs.Length; i++)
                Assert.Equal(inputs[i].Item3,  inputs[i].Item2 + inputs[i].Item1);
        }

        [Fact]
        public void TestSubstract()
        {
            (Matrix<int>, Matrix<int>, Matrix<int>)[] inputs =
            {
                (
                    new int[,]{{1012374,1037197},{1050599,1112890}},
                    new int[,]{{1236093,72540},{845062,51057}},
                    new int[,]{{-223719, 964657}, {205537, 1061833}}
                ),
                (
                    new int[,]{{131305,1052834},{805299,669307}},
                    new int[,]{{842151,1593357},{1855646,1953061}},
                    new int[,]{{-710846, -540523}, {-1050347, -1283754}}
                ),
                (
                    new int[,]{{1973460,235416},{1873653,1312046},{1092682,758346}},
                    new int[,]{{1840525,1498344},{1426583,1545091},{1260516,1989954}},
                    new int[,]{{132935, -1262928}, {447070, -233045}, {-167834, -1231608}}
                ),
                (
                    new int[,]{{1090924,151525,1385267,1905711},{1720299,1919949,606945,986322},{1857376,1683467,1749295,685363}},
                    new int[,]{{1462523,125142,1989364,1727489},{1260039,22140,124655,972345},{1113811,1403809,160162,1896816}},
                    new int[,]{{-371599, 26383, -604097, 178222}, {460260, 1897809, 482290, 13977}, {743565, 279658, 1589133, -1211453}}
                ),
                (
                    new int[,]{{1841561,825681,528558,673743}},
                    new int[,]{{1259720,1368474,63463,477415}},
                    new int[,]{{581841, -542793, 465095, 196328}}
                ),
                (
                    new int[,]{{1690320,1229662,1243656},{811996,1833176,826311},{885842,1886789,848343}},
                    new int[,]{{655256,1064783,1188089},{1397484,1125799,1610467},{953822,1901463,721735}},
                    new int[,]{{1035064, 164879, 55567}, {-585488, 707377, -784156}, {-67980, -14674, 126608}}
                ),
            };

            for (int i = 0; i < inputs.Length; i++)
                Assert.Equal(inputs[i].Item3,  inputs[i].Item1 - inputs[i].Item2);
        }

        [Fact]
        public void TestMultipy()
        {
            (Matrix<int>, Matrix<int>, Matrix<int>)[] inputs1 =
            {
                (
                    new int[,]{{5909,6344,6621},{4122,3346,8485}},
                    new int[,]{{442,8726,9136,9441},{5746,5926,9679,8057},{3968,9453,7127,5437}},
                    new int[,]{{65336530, 151744791, 162576067, 142898854}, {54716520, 136005673, 130517121, 112007469}}
                ),
                (
                    new int[,]{{4404},{9577},{7956},{6386},{6270}},
                    new int[,]{{81,6684,8545,5082,5104,4421,7253,1382,2401,6054}},
                    new int[,]{{356724, 29436336, 37632180, 22381128, 22478016, 19470084, 31942212, 6086328, 10574004, 26661816}, {775737, 64012668, 81835465, 48670314, 48881008, 42339917, 69461981, 13235414, 22994377, 57979158}, {644436, 53177904, 67984020, 40432392, 40607424, 35173476, 57704868, 10995192, 19102356, 48165624}, {517266, 42684024, 54568370, 32453652, 32594144, 28232506, 46317658, 8825452, 15332786, 38660844}, {507870, 41908680, 53577150, 31864140, 32002080, 27719670, 45476310, 8665140, 15054270, 37958580}}
                ),
                (
                    new int[,]{{6458,200,328},{4816,9768,9058},{934,6617,6677}},
                    new int[,]{{2844,1785,2069},{9892,5651,8163},{8272,6779,5812}},
                    new int[,]{{23058168, 14881242, 16900538}, {185249536, 125199710, 142345584}, {123343804, 84323240, 94753741}}
                ),
                (
                    new int[,]{{3805}},
                    new int[,]{{8739,7630,798,6524,3029,3240,888}},
                    new int[,]{{33251895, 29032150, 3036390, 24823820, 11525345, 12328200, 3378840}}
                ),
                (
                    new int[,]{{1644,4009,1222,5873,8771,2281,421,2277,3306,3399,7293,9006,3224},{7739,8665,9438,6369,1897,8085,7444,262,9398,7831,9026,1423,6673},{4100,8541,7276,1686,5204,8579,8215,9932,7223,6575,4859,6811,6464}},
                    new int[,]{{4322,5500},{9369,3022},{8886,2124},{2803,3835},{1809,6061},{3297,9952},{1870,5481},{9390,2802},{7707,1332},{3994,1937},{4587,4583},{7324,1476},{5637,4693}},
                    new int[,]{{274183467, 203659241}, {455964511, 349185560}, {503981528, 345070663}}
                ),
                (
                    new int[,]{{5988,5627,4259,7683,2185},{4968,1434,9356,8236,734},{8015,4211,8403,5957,3818},{253,227,2388,1859,956},{5831,2453,4670,6163,6422},{4531,8505,3058,737,6779},{6416,8688,9570,6950,9118},{8645,2466,9251,5025,593},{1328,8582,974,5644,6439},{7762,8905,6827,1934,7897},{3343,4948,7216,6034,9764},{2344,6628,7283,2647,8802},{724,4515,2573,4949,10}},
                    new int[,]{{3933,4528,1750,4084,7567,8433,6029,6375,4502},{7297,6449,2402,3648,7016,3218,3112,4289,3995},{5747,2929,3130,5443,7889,885,162,1856,3680},{1955,6141,1296,8867,467,4041,6628,4149,42},{4597,1632,1607,9054,7539,7338,7676,588,5159}},
                    new int[,]{{114152206, 126624021, 50794187, 156072176, 138450155, 119454238, 121997818, 103373954, 76706062}, {103247552, 110921858, 53276142, 156119500, 130843122, 93457584, 96152744, 89788918, 66657464}, {139739984, 130873959, 64298260, 181221408, 188050969, 140666869, 131578317, 111713149, 103776771}, {24404381, 22580270, 12408000, 39998609, 30421452, 19504762, 22278325, 15293725, 15843793}, {109241953, 104228282, 49020858, 170973267, 149468634, 133228846, 133689011, 85707485, 86626441}, {130059632, 99911240, 49778805, 134087583, 169532779, 121007862, 111200795, 78082583, 100633292}, {199131950, 170671616, 85710502, 254166300, 256988862, 185526396, 183323828, 130123686, 146142654}, {117710580, 113970274, 57473063, 144581038, 162516612, 113683467, 99151127, 104055814, 86085477}, {114077559, 109379600, 43648681, 150376224, 129123543, 109734676, 121705680, 74285030, 77303915}, {174825979, 137335562, 71538763, 189993025, 235509398, 165918611, 149051356, 113014809, 136465210}, {147405505, 121172062, 63832238, 212886538, 193367347, 146531969, 151663931, 86702791, 111998230}, {145076548, 105309602, 60393572, 176557466, 189289110, 122826914, 121046626, 73045719, 109353680}, {60301743, 70339953, 26595494, 77405698, 59840718, 42984156, 51711234, 49295104, 31024961}}
                )
            };
            for (int i = 0; i < inputs1.Length; i++)
                Assert.Equal(inputs1[i].Item3, inputs1[i].Item1 * inputs1[i].Item2);

            (Matrix<int>, int, Matrix<int>)[] input2 =
            {
                (
                    new int[,]{{553,4658,3201,8491,6715,4279},{9996,3733,7144,5266,6622,3716},{2211,1266,508,3062,595,8133},{8146,2853,5360,2610,1489,9662},{5633,7589,9894,4103,3522,3318},{7678,5847,7260,1338,2572,7161},{2034,5231,4486,5981,1229,3135},{9107,7018,1546,9637,3876,588}},
                    9,
                    new int[,]{{4977, 41922, 28809, 76419, 60435, 38511}, {89964, 33597, 64296, 47394, 59598, 33444}, {19899, 11394, 4572, 27558, 5355, 73197}, {73314, 25677, 48240, 23490, 13401, 86958}, {50697, 68301, 89046, 36927, 31698, 29862}, {69102, 52623, 65340, 12042, 23148, 64449}, {18306, 47079, 40374, 53829, 11061, 28215}, {81963, 63162, 13914, 86733, 34884, 5292}}
                ),
                (
                    new int[,]{{3119,2113,8344,6898},{8932,1805,5696,8132},{6223,7911,7789,9284},{1496,7288,7319,4534}},
                    3814,
                    new int[,]{{11895866, 8058982, 31824016, 26308972}, {34066648, 6884270, 21724544, 31015448}, {23734522, 30172554, 29707246, 35409176}, {5705744, 27796432, 27914666, 17292676}}
                ),
                (
                    new int[,]{{9164,3138,9588},{139,6508,3538}},
                    9260,
                    new int[,]{{84858640, 29057880, 88784880}, {1287140, 60264080, 32761880}}
                ),
                (
                    new int[,]{{6571}},
                    9073,
                    new int[,]{{59618683}}
                ),
                (
                    new int[,]{{7155,3289,321,8537,6583,5953,5718,674}},
                    1940,
                    new int[,]{{13880700, 6380660, 622740, 16561780, 12771020, 11548820, 11092920, 1307560}}
                ),
            };
            for (int i = 0; i < input2.Length; i++)
                Assert.Equal(input2[i].Item3, input2[i].Item1 * input2[i].Item2);
        }
    }
}