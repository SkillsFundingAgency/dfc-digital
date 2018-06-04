using DFC.Digital.Data.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Core.Tests
{
    public class HttpClientServiceTests
    {

        [Fact]
        public void HttpClientServiceTest()
        {
            var tolerancePolicyFake = A.Fake<ITolerancePolicy>();
            var httpClientService = new HttpClientService<ISpellcheckService>(tolerancePolicyFake);

            httpClientService.AddHeader("dummyKey", "dummyValue").Should().Be(true);

            //This should already have been added an so now return false
            httpClientService.AddHeader("dummyKey", "dummyValue").Should().Be(false);
        }
    }
}
