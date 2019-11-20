using DFC.Digital.Data.Model;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.DynamicModules.Events;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface IDataEventProcessor
    {
        void ExportCompositePage(IDataEvent eventInfo);

        void ExportContentData(IDataEvent eventInfo);

        void PublishDynamicContent(DynamicContent eventInfo, string eventActionType);
    }
}
