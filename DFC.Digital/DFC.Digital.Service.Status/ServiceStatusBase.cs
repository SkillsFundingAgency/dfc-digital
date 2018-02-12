using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.Status
{
    public abstract class ServiceStatusBase : IServiceStatus
    {
        public abstract string ServiceName { get; }
        public abstract ServiceStatus GetCurrentStatus();
    }
}
