using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.MicroServicesPublishing;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Service.CompositeUIPublishing.UnitTests
{
    public class MicroServicesPublishingTests
    {
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IMicroServicesPublishingClientProxy fakeMicroServicesPublishingClientProxy;

        public MicroServicesPublishingTests()
        {
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            fakeMicroServicesPublishingClientProxy = A.Fake<IMicroServicesPublishingClientProxy>(ops => ops.Strict());
            A.CallTo(() => fakeApplicationLogger.Trace(A<string>._)).DoesNothing();
            A.CallTo(() => fakeApplicationLogger.Info(A<string>._)).DoesNothing();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task PostPageDataAsync(bool expectedResponse)
        {
            //Arrange
            HttpStatusCode expectedHttpStatusCode;
            if (expectedResponse)
            {
                expectedHttpStatusCode = HttpStatusCode.OK;
            }
            else
            {
                expectedHttpStatusCode = HttpStatusCode.BadRequest;
            }

            var httpResponseMessage = new HttpResponseMessage(expectedHttpStatusCode);

            A.CallTo(() => fakeMicroServicesPublishingClientProxy.PostDataAsync(A<string>._, A<string>._)).Returns(httpResponseMessage);
     
            //Act
            var microServicesPublishingService = new MicroServicesPublishingService(fakeApplicationLogger, fakeMicroServicesPublishingClientProxy);
            var result = await microServicesPublishingService.PostPageDataAsync("DummyEnd", new MicroServicesPublishingPageData());

            //Assert
            result.Should().Be(expectedResponse);
        }


        [Theory]
        [InlineData(true, ServiceState.Green)]
        [InlineData(false, ServiceState.Red)]
        public async Task GetServiceStatusAsync(bool returnValidHttpStatusCode, ServiceState expectedServiceStatus)
        {
            //Arrange
            HttpStatusCode expectedHttpStatusCode;
            if (returnValidHttpStatusCode)
            {
                expectedHttpStatusCode = HttpStatusCode.OK;
            }
            else
            {
                expectedHttpStatusCode = HttpStatusCode.BadRequest;
            }

            var httpResponseMessage = new HttpResponseMessage(expectedHttpStatusCode);

            A.CallTo(() => fakeMicroServicesPublishingClientProxy.PostDataAsync(A<string>._, A<string>._)).Returns(httpResponseMessage);
       
            //Act
            var microServicesPublishingService = new MicroServicesPublishingService(fakeApplicationLogger, fakeMicroServicesPublishingClientProxy);
            var serviceStatus = await microServicesPublishingService.GetCurrentStatusAsync();

            //Assert
            serviceStatus.Status.Should().Be(expectedServiceStatus);
        }

        [Fact]
        public async Task GetServiceStatusExceptionAsync()
        {
            //Arrange
            //add no content to cause an exception
            var httpResponseMessage = new HttpResponseMessage();

            A.CallTo(() => fakeMicroServicesPublishingClientProxy.PostDataAsync(A<string>._, A<string>._)).Throws(new HttpRequestException() );
            A.CallTo(() => fakeApplicationLogger.LogExceptionWithActivityId(A<string>._, A<Exception>._)).Returns("Exception logged");

            //Act
            var microServicesPublishingService = new MicroServicesPublishingService(fakeApplicationLogger, fakeMicroServicesPublishingClientProxy);
            var serviceStatus = await microServicesPublishingService.GetCurrentStatusAsync();

            //Asserts
            serviceStatus.Status.Should().NotBe(ServiceState.Green);
            serviceStatus.Notes.Should().Contain("Exception");
            A.CallTo(() => fakeApplicationLogger.LogExceptionWithActivityId(A<string>._, A<Exception>._)).MustHaveHappened();
        }
    }
}
