using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Web;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ISitefinityCurrentContext
    {
        PageData CurrentPage
        {
            [IgnoreInputInInterception]
            [IgnoreOutputInInterception]
            get;

            [IgnoreInputInInterception]
            [IgnoreOutputInInterception]
            set;
        }

        PageSiteNode CurrentNode
        {
            [IgnoreInputInInterception]
            [IgnoreOutputInInterception]
            get;
            [IgnoreInputInInterception]
            [IgnoreOutputInInterception]
            set;
        }

        PageManager CurrentPageManager
        {
            [IgnoreInputInInterception]
            [IgnoreOutputInInterception]
            get;

            [IgnoreInputInInterception]
            [IgnoreOutputInInterception]
            set;
        }

        DfcPageSiteNode GetCurrentDfcPageNode();

        IList<BreadCrumbLink> BreadcrumbToParent();
    }
}