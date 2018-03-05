using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Core.Utility
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