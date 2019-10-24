using DFC.Digital.Data.Model;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.DynamicModules.Events;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface IDataEventProcessor
    {
        void ExportCompositePage(IDataEvent eventInfo);

        void PublishDynamicContent(IDynamicContentUpdatedEvent eventInfo);
    }
}
