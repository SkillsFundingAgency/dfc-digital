using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.StatusCheck
{
    public abstract class ServiceStatusBase : IServiceStatus
    {
        public string activityId = Guid.NewGuid().ToString();
        public string exceptionMessage => $"Exception: Check logs with activity id - {activityId}";
        public string logMessagePrefix => $"Service status check failed for activity id - {activityId}";

        public abstract Task<ServiceStatus> GetCurrentStatusAsync();

    }
}
