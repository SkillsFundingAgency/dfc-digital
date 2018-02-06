using DFC.Digital.Data.Model;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Web;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ISitefinityCurrentContext
    {
        PageData CurrentPage { get; set; }

        PageSiteNode CurrentNode { get; set; }

        PageManager CurrentPageManager { get; set; }

        DfcPageSiteNode GetCurrentDfcPageNode();
    }
}