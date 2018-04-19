using DFC.Digital.Data.Interfaces;
using FakeItEasy;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Web.Core.UnitTests
{
    public class DependencyHealthCheckServiceTests
    {
        [Fact]
        public async Task GetServiceStatusTestAsync()
        {
            var fakeServiceStatus = A.Fake<IServiceStatus>();
            var healthCheckService = new DependencyHealthCheckService(fakeServiceStatus);
            await healthCheckService.GetServiceStatus();

            A.CallTo(() => fakeServiceStatus.GetCurrentStatusAsync()).MustHaveHappened();
        }
    }
}