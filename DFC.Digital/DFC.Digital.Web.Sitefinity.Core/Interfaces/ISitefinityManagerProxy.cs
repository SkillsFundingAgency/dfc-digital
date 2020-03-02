using DFC.Digital.Core.Interceptors;
using DFC.Digital.Repository.SitefinityCMS;
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
        PageNode GetPageNode(Type contentType, Guid itemId, string providerName = null);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        PageData GetPageDataByName(string name);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        PageDraft GetPreviewPageDataById(Guid id);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        PageData GetPageData(Type contentType, Guid itemId, string providerName = null);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        string GetControlContent(PageControl pageControl, string providerName = null);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        string GetControlContent(PageDraftControl pageDraftControl);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        string GetControlSharedContent(PageControl pageControl, IDynamicContentExtensions dynamicContentExtensions, string providerName = null);
    }
}
