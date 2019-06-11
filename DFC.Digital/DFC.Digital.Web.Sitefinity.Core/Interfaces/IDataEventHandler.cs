using DFC.Digital.Data.Model;
using System.Collections.Generic;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface IDataEventHandler
    {
        void ExportCompositePage(IDataEvent eventInfo);

        CompositePageData GetCompositePageForPageNode(PageNode node);

        IList<string> GetPageControlsData(PageData pageData);
    }
}
