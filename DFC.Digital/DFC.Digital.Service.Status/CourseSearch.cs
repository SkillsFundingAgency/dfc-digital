using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Service.Status
{
    class CourseSearch : ServiceStatusBase
    {
        public override string ServiceName => "Course Search";

        public override ServiceStatus GetCurrentStatus()
        {
            return new ServiceStatus { Name = ServiceName, Status = ServiceState.Green, Notes = "slow today" };
        }

    }
}
