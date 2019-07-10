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

namespace DFC.Digital.Service.MicroServicesPublishing.UnitTests
{
    public class MicroServicesPublishingTests
    {
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IHttpClientService<IMicroServicesPublishingService> fakeHttpClientService;
        private readonly IConfigurationProvider fakeConfigurationProvider;

        public MicroServicesPublishingTests()
        {
            fakeApplicationLogger = A.Fake<IApplicationLogger>();
            fakeHttpClientService = A.Fake<IHttpClientService<IMicroServicesPublishingService>>(ops => ops.Strict());
            fakeConfigurationProvider = A.Fake<IConfigurationProvider>(ops => ops.Strict());

            A.CallTo(() => fakeConfigurationProvider.GetConfig<string>(A<string>._)).Returns(A.Dummy<string>());
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
            A.CallTo(() => fakeHttpClientService.PostAsync(A<string>._, A<string>._, A<FaultToleranceType>._)).Returns(httpResponseMessage);

            //Act
            var microServicesPublishingService = new MicroServicesPublishingService(fakeApplicationLogger, fakeConfigurationProvider, fakeHttpClientService);
            var result = await microServicesPublishingService.PostPageDataAsync(A.Dummy<string>(), new MicroServicesPublishingPageData());

            //Assert
            result.Should().Be(expectedResponse);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeletePageAsync(bool expectedResponse)
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
            A.CallTo(() => fakeHttpClientService.DeleteAsync(A<string>._, A<Func<HttpResponseMessage, bool>>._, A<FaultToleranceType>._)).Returns(httpResponseMessage);

            A.CallTo(() => fakeConfigurationProvider.GetConfig<string>(A<string>._)).Returns($"{A.Dummy<string>()}/");

            //Act
            var microServicesPublishingService = new MicroServicesPublishingService(fakeApplicationLogger, fakeConfigurationProvider, fakeHttpClientService);
            var result = await microServicesPublishingService.DeletePageAsync(A.Dummy<string>(), A.Dummy<Guid>());

            //Assert
            result.Should().Be(expectedResponse);
            A.CallTo(() => fakeHttpClientService.DeleteAsync($"{A.Dummy<string>()}/{A.Dummy<Guid>()}", A<Func<HttpResponseMessage, bool>>._, A<FaultToleranceType>._)).MustHaveHappened();
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
            A.CallTo(() => fakeHttpClientService.PostAsync(A<string>._, A<string>._, A<FaultToleranceType>._)).Returns(httpResponseMessage);

            //Act
            var microServicesPublishingService = new MicroServicesPublishingService(fakeApplicationLogger, fakeConfigurationProvider, fakeHttpClientService);
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

            A.CallTo(() => fakeHttpClientService.PostAsync(A<string>._, A<string>._, A<FaultToleranceType>._)).Throws(new HttpRequestException());
            A.CallTo(() => fakeApplicationLogger.LogExceptionWithActivityId(A<string>._, A<Exception>._)).Returns("Exception logged");

            //Act
            var microServicesPublishingService = new MicroServicesPublishingService(fakeApplicationLogger, fakeConfigurationProvider, fakeHttpClientService);
            var serviceStatus = await microServicesPublishingService.GetCurrentStatusAsync();

            //Asserts
            serviceStatus.Status.Should().NotBe(ServiceState.Green);
            serviceStatus.Notes.Should().Contain("Exception");
            A.CallTo(() => fakeApplicationLogger.LogExceptionWithActivityId(A<string>._, A<Exception>._)).MustHaveHappened();
        }
    }
}
