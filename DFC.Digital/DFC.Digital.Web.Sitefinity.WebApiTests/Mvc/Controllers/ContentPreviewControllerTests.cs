using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.WebApi.Mvc.Controllers;
using FakeItEasy;
using FluentAssertions;
using System.Net;
using System.Web.Http.Results;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.WebApiTests.Mvc.Controllers
{
    public class ContentPreviewControllerTests
    {
        [Theory]
        [InlineData(true, HttpStatusCode.OK)]
        [InlineData(false, HttpStatusCode.NotFound)]
        public void GetTests(bool contentExists, HttpStatusCode expectedResponseCode)
        {
            // Setup
            var fakeCompositePageBuilder = A.Fake<ICompositePageBuilder>();
            var dummymicroServicesPublishingPageData = A.Dummy<MicroServicesPublishingPageData>();

            if (contentExists)
            {
                A.CallTo(() => fakeCompositePageBuilder.GetPreviewPage(A<string>._)).Returns(dummymicroServicesPublishingPageData);
            }
            else
            {
                A.CallTo(() => fakeCompositePageBuilder.GetPreviewPage(A<string>._)).Returns(null);
            }

            // Act
            var api = new ContentPreviewController(fakeCompositePageBuilder);
            var actionResult = api.Get("dummyName");

            // Asserts
            if (expectedResponseCode == HttpStatusCode.NotFound)
            {
                actionResult.Should().BeOfType<NotFoundResult>();
            }
            else
            {
                var response = actionResult as JsonResult<MicroServicesPublishingPageData>;
                response.Content.Should().Be(dummymicroServicesPublishingPageData);
            }
        }
    }
}
