using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Service.CUIStatusChecks;
using FakeItEasy;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Services.CUIStatusHealthChecks.Tests
{
    public class CUIJobProfilesStatusCheckTests
    {
        private readonly IServiceStatusCUIApp fakeServiceStatusCUIApp;
        private readonly IConfigurationProvider fakeConfigurationProvider;

        public CUIJobProfilesStatusCheckTests()
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
            var jobprofilesHealthCheck = new CUIJobprofilesHealthCheck(fakeConfigurationProvider, fakeServiceStatusCUIApp);
            var serviceStatus = await jobprofilesHealthCheck.GetCurrentStatusAsync();

            //Asserts
            A.CallTo(() => fakeServiceStatusCUIApp.GetCurrentCUIAppStatusAsync(A<string>._, A<string>._)).MustHaveHappenedOnceExactly();
        }
    }
}