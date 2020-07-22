using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Service.CUIStatusChecks;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Services.CUIStatusHealthChecks.Tests
{
    public class CUIShellStatusCheckTests
    {
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IHttpClientService<IServiceStatus> fakeHttpClientService;
        private readonly IConfigurationProvider fakeConfigurationProvider;

        public CUIShellStatusCheckTests()
        {
            fakeApplicationLogger = A.Fake<IApplicationLogger>();
            fakeHttpClientService = A.Fake<IHttpClientService<IServiceStatus>>(ops => ops.Strict());
            fakeConfigurationProvider = A.Fake<IConfigurationProvider>(ops => ops.Strict());
            A.CallTo(() => fakeHttpClientService.AddHeader(A<string>.Ignored, A<string>.Ignored)).Returns(true);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, ServiceState.Green)]
        [InlineData(HttpStatusCode.BadRequest, ServiceState.Red)]
        public async Task GetServiceStatusAsync(HttpStatusCode returnHttpStatusCode, ServiceState expectedServiceStatus)
        {
            //Arrange
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = returnHttpStatusCode
            };
            A.CallTo(() => fakeConfigurationProvider.GetConfig<string>(A<string>._)).Returns(A.Dummy<string>());
            A.CallTo(() => fakeHttpClientService.GetAsync(A<string>._, A<FaultToleranceType>._)).Returns(httpResponseMessage);

            //Act
            var shellHealthCheck = new CUIShellHealthCheck(fakeApplicationLogger, fakeConfigurationProvider, fakeHttpClientService);
            var serviceStatus = await shellHealthCheck.GetCurrentStatusAsync();

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

            //Act
            var shellHealthCheck = new CUIShellHealthCheck(fakeApplicationLogger, fakeConfigurationProvider, fakeHttpClientService);
            var serviceStatus = await shellHealthCheck.GetCurrentStatusAsync();

            //Asserts
            serviceStatus.Status.Should().NotBe(ServiceState.Green);
            A.CallTo(() => fakeApplicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappened();
        }
    }
}