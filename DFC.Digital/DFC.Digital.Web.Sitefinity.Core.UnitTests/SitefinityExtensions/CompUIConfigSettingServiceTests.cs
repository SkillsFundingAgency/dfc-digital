using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Net.Http;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.UnitTests
{
    public class CompUIConfigSettingServiceTests
    {
        private readonly IConfigurationProvider fakeConfigurationProvider;

        public CompUIConfigSettingServiceTests()
        {
            fakeConfigurationProvider = A.Fake<IConfigurationProvider>(ops => ops.Strict());
        }

        [Fact]
        public void GetUrlTest()
        {
            var dummyUrl = "dummyUrl";

            var compUIConfig = new CompUIConfigSettingService(fakeConfigurationProvider);
            A.CallTo(() => fakeConfigurationProvider.GetConfig<string>(A<string>._)).Returns(dummyUrl);

            var result = compUIConfig.GetUrl();

            result.Should().Be(dummyUrl);
        }
    }
}