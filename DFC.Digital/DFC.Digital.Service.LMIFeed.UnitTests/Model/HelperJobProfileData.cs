using System.Collections.Generic;
using System.Net;

namespace DFC.Digital.Service.LMIFeed.UnitTests.Model
{
    public class HelperJobProfileData
    {
        private static readonly Dictionary<int, decimal> FirstSet = new Dictionary<int, decimal>
        {
            {10, 1000},
            {20, 2000},
            {30, 3000},
            {40, 4000}
        };

        private static readonly Dictionary<int, decimal> SecondSet = new Dictionary<int, decimal>
        {
            {20, 2000},
            {30, 3000},
            {80, 8000},
            {90, 9000}
        };

        private static readonly Dictionary<int, decimal> ThirdSet = new Dictionary<int, decimal>
        {
            {30, 3000},
            {40, 4000},
            {50, 5000},
            {60, 6000},
            {70, 7000}
        };

        private static readonly Dictionary<int, decimal> FourthSet = new Dictionary<int, decimal>
        {
            {40, 4000},
            {60, 6000},
            {80, 8000}
        };

        public static IEnumerable<object[]> StarterSalaryMedianDeciles()
        {
            yield return new object[]
            {
                FirstSet,
                (decimal) 52000
            };
            yield return new object[]
            {
                SecondSet,
                (decimal) 104000
            };

            yield return new object[]
            {
                ThirdSet,
                (decimal) 156000
            };
            yield return new object[]
            {
                FourthSet,
                (decimal) 208000
            };
        }

        public static IEnumerable<object[]> ExperiencedSalaryMedianDeciles()
        {
            yield return new object[]
            {
                FirstSet,
                (decimal) 208000
            };
            yield return new object[]
            {
                SecondSet,
                (decimal) 468000
            };

            yield return new object[]
            {
                ThirdSet,
                (decimal) 364000
            };
            yield return new object[]
            {
                FourthSet,
                (decimal) 416000
            };
        }

        public static IEnumerable<object[]> JobProfileAsheData()
        {
            yield return new object[]
            {
                "1115"
                ,"1115"
            };
            yield return new object[]
            {
                 "1115-a",
                 "1115"
            };
            yield return new object[]
            {
                "2216",
                "2216"
            };
            yield return new object[]
            {
               "6222",
               "6222"
            };
        }

        public static IEnumerable<object[]> ProxyData()
        {
            yield return new object[]
            {
               "1115",
               HttpStatusCode.OK
            };
            yield return new object[]
            {
                " ",
                HttpStatusCode.BadRequest
            };
        }
    }
}