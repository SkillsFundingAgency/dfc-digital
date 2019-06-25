using System.Collections.Generic;

namespace DFC.Digital.Repository.SitefinityCMS.Helpers
{
    public class MemberDataHelper
    {
        public static IEnumerable<object[]> Dfc9493GetByJobProfileUrlNameTestsInput()
        {
            yield return new object[]
            {
                "plumber",
                true,
                true
            };
            yield return new object[]
            {
                "plumber",
                false,
                false
            };

            yield return new object[]
            {
                "plumber",
                true,
                false
            };
        }
    }
}
