using DFC.Digital.Data.Interfaces; using DFC.Digital.Core;
using FakeItEasy;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.Utility.Tests
{
    public class WebAppContextSitefinityCoreTest
    {
        [Theory]
        [InlineData("Nurse", true)]
        [InlineData("ZAP%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s%n%s", false)]
        [InlineData("<invalid>", false)]
        public void CheckIsValidAndFormattedUrlShouldReturnTrue(string jobprofileUrl, bool expectation)
        {
            string jobProfileDetailspage = "/job-profile/";
            var webAppContext = A.Fake<IWebAppContext>();

            A.CallTo(() => webAppContext.IsValidAndFormattedUrl($"{jobProfileDetailspage}{jobprofileUrl}"))
                .Returns(expectation);
        }
    }
}