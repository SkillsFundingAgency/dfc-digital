using Telerik.Sitefinity.Data.Events;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface IDataEventHandler
    {
        void ExportCompositePage(IDataEvent eventInfo);
    }
}
