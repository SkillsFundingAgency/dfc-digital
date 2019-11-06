using DFC.Digital.Data.Model;
using Telerik.Sitefinity.Data.Events;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface IDataEventActions
    {
        MicroServicesDataEventAction GetEventAction(IDataEvent dataEvent);

        bool ShouldExportPage(IDataEvent dataEvent);
    }
}
