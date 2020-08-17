using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IServiceStatusCUIApp
    {
        Task<ServiceStatus> GetCurrentCUIAppStatusAsync(string serviceName, string serviceEndPoint);
    }
}
