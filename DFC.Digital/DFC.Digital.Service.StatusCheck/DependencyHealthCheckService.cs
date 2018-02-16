
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Service.CourseSearchProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.ServiceStatusesToCheck
{
    public class DependencyHealthCheckService
    {
        private readonly IServiceStatus serviceStatus;

        public DependencyHealthCheckService(IServiceStatus serviceStatus)
        {
            this.serviceStatus = serviceStatus;
        }

        public ServiceStatus Status => serviceStatus.GetCurrentStatus();
    }
}
