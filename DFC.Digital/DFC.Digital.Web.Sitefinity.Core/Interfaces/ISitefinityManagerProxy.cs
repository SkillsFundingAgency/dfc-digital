using DFC.Digital.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ISitefinityManagerProxy
    {
        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        PageNode GetPageNode(string providerName, Type contentType, Guid itemId);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        PageData GetPageData(string providerName, Type contentType, Guid itemId);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        string GetControlContent(string providerName, PageControl pageControl);
    }
}
