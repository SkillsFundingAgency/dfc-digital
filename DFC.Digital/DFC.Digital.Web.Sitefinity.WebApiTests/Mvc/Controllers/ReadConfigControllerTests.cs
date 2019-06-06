using DFC.Digital.Core;
using DFC.Digital.Web.Sitefinity.WebApi;
using DFC.Digital.Web.Sitefinity.WebApi.Mvc.Controllers;
using FakeItEasy;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Results;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.WebApiTests.Mvc.Controllers
{
    public class ReadConfigControllerTests
    {
        public class AVFeedRecycleBinControllerTests
        {
            [Theory]
            [InlineData(null, HttpStatusCode.NotFound)]
            [InlineData("validConfig", HttpStatusCode.OK)]
            public void IndexTest(string configValue, HttpStatusCode expectedResponseCode)
            {
                // Setup
                var fakeConfigurationProvider = A.Fake<IConfigurationProvider>();
                var api = new ReadConfigController(fakeConfigurationProvider);
                A.CallTo(() => fakeConfigurationProvider.GetConfig<string>(A<string>._)).Returns(configValue);

                // Action
                var actionResult = api.Index("fakekey");

                // Asserts
                A.CallTo(() => fakeConfigurationProvider.GetConfig<string>(A<string>._)).MustHaveHappenedOnceExactly();

                if (expectedResponseCode == HttpStatusCode.NotFound)
                {
                    actionResult.Should().BeOfType<NotFoundResult>();
                }
                else
                {
                    actionResult.Should().BeOfType<OkNegotiatedContentResult<string>>();
                    var response = actionResult as OkNegotiatedContentResult<string>;
                    response.Content.Should().Be(configValue);
                }
            }
        }
    }
}
