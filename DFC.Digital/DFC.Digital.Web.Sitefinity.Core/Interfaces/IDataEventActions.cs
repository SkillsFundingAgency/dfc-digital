using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Data.Events;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface IDataEventActions
    {
        MicroServicesDataEventAction GetEventAction(IDataEvent dataEvent);

        bool ShouldExportPage(IDataEvent dataEvent);
    }
}
