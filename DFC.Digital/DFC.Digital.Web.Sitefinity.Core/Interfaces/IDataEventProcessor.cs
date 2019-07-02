using Telerik.Sitefinity.Data.Events;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface IDataEventProcessor
    {
        void ExportCompositePage(IDataEvent eventInfo);
    }
}
