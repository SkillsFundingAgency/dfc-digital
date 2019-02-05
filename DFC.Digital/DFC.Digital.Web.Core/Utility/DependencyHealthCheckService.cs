using DFC.Digital.Data.Interfaces;
using System.Threading.Tasks;

namespace DFC.Digital.Web.Core
{
    public class DependencyHealthCheckService
    {
        private readonly IServiceStatus serviceStatus;

        public DependencyHealthCheckService(IServiceStatus serviceStatus)
        {
            this.serviceStatus = serviceStatus;
        }

        public Task<ServiceStatus> GetServiceStatus()
        {
            return serviceStatus.GetCurrentStatusAsync();
        }
    }
}