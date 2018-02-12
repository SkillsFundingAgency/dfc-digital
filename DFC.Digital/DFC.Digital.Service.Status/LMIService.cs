using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.Status
{
    class LMIService : ServiceStatusBase
    {
        public override string ServiceName => "LMI";

        public override ServiceStatus GetCurrentStatus()
        {
            return new ServiceStatus { Name = ServiceName, Status = ServiceState.Amber, Notes = "A bit slow today" };
        }

    }
}
