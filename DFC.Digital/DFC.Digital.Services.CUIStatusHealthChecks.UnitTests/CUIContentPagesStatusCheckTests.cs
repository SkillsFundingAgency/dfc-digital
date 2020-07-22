using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Service.CUIStatusChecks;
using FakeItEasy;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Services.CUIStatusHealthChecks.Tests
{
    public class CUIContentPagesStatusCheckTests
    {
        private readonly IServiceStatusCUIApp fakeServiceStatusCUIApp;
        private readonly IConfigurationProvider fakeConfigurationProvider;

        public CUIContentPagesStatusCheckTests()
        {
            fakeServiceStatusCUIApp = A.Fake<IServiceStatusCUIApp>(ops => ops.Strict());
            fakeConfigurationProvider = A.Fake<IConfigurationProvider>(ops => ops.Strict());
            A.CallTo(() => fakeConfigurationProvider.GetConfig<string>(A<string>._)).Returns(A.Dummy<string>());
        }

        [Fact]
        public async Task GetServiceStatusExceptionAsync()
        {
            //arrange
            A.CallTo(() => fakeServiceStatusCUIApp.GetCurrentCUIAppStatusAsync(A<string>._, A<string>._)).Returns(A.Dummy<ServiceStatus>());

            //Act
            var contentPagesHealthCheck = new CUIContentPagesHealthCheck(fakeConfigurationProvider, fakeServiceStatusCUIApp);
            var serviceStatus = await contentPagesHealthCheck.GetCurrentStatusAsync();

            //Asserts
            A.CallTo(() => fakeServiceStatusCUIApp.GetCurrentCUIAppStatusAsync(A<string>._, A<string>._)).MustHaveHappenedOnceExactly();
        }
    }
}