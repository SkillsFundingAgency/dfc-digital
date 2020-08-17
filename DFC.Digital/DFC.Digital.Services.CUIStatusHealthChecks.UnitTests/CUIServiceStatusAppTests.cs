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
    public class CUIServiceStatusAppTests
    {
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IHttpClientService<IServiceStatus> fakeHttpClientService;

        public CUIServiceStatusAppTests()
        {
            fakeApplicationLogger = A.Fake<IApplicationLogger>();
            fakeHttpClientService = A.Fake<IHttpClientService<IServiceStatus>>(ops => ops.Strict());
            A.CallTo(() => fakeHttpClientService.AddHeader(A<string>.Ignored, A<string>.Ignored)).Returns(true);
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, ServiceState.Green, "is available")]
        [InlineData(HttpStatusCode.OK, ServiceState.Amber, "not available")]
        [InlineData(HttpStatusCode.BadRequest, ServiceState.Red, "")]
        public async Task GetServiceStatusAsync(HttpStatusCode returnHttpStatusCode, ServiceState expectedServiceStatus, string serviceStatusResponse)
        {
            //Arrange
            var responseJson = "[{\"service\": \"DFC.App.JobProfile\",\"message\": \"Document store" + serviceStatusResponse + "\"}]";

            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json"),
                StatusCode = returnHttpStatusCode
            };

            A.CallTo(() => fakeHttpClientService.GetAsync(A<string>._, A<FaultToleranceType>._)).Returns(httpResponseMessage);

            //Act
            var serviceStatusCUIApp = new ServiceStatusCUIApp(fakeApplicationLogger, fakeHttpClientService);
            var serviceStatus = await serviceStatusCUIApp.GetCurrentCUIAppStatusAsync(A.Dummy<string>(), A.Dummy<string>());

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
            var serviceStatusCUIApp = new ServiceStatusCUIApp(fakeApplicationLogger, fakeHttpClientService);
            var serviceStatus = await serviceStatusCUIApp.GetCurrentCUIAppStatusAsync(A.Dummy<string>(), A.Dummy<string>());

            //Asserts
            serviceStatus.Status.Should().NotBe(ServiceState.Green);
            A.CallTo(() => fakeApplicationLogger.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappened();
        }
    }
}