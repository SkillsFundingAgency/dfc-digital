using DFC.Digital.Core;
using System;
using System.Linq;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SitefinityManagerProxy : ISitefinityManagerProxy
    {
        public static string GetLstringValue(Lstring lstring)
        {
            return lstring?.Value;
        }

        public PageNode GetPageNode(Type contentType, Guid itemId, string providerName)
        {
            var item = GetPageManagerForProvider(providerName).GetItemOrDefault(contentType, itemId);
            var pageNode = (PageNode)item;
            return pageNode;
        }

        public PageData GetPageData(Type contentType, Guid itemId, string providerName)
        {
            return GetPageNode(contentType, itemId, providerName).GetPageData();
        }

        public PageData GetPageDataByName(string name)
        {
            return GetPageManagerForProvider().GetPageDataList().FirstOrDefault(page => page.NavigationNode.UrlName == name);
        }

        public string GetControlContent(PageControl pageControl, string providerName)
        {
            var control = GetPageManagerForProvider(providerName).LoadControl(pageControl) as Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy;
            return control.Settings.Values[Constants.Content];
        }

        public string GetControlContent(PageDraftControl pageDraftControl)
        {
            var control = GetPageManagerForProvider().LoadControl(pageDraftControl) as Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy;
            return control.Settings.Values[Constants.Content];
        }

        public PageDraft GetPreViewPageDataById(Guid id)
        {
            return GetPageManagerForProvider().GetPreview(id);
        }

        private PageManager GetPageManagerForProvider(string providerName = null)
        {
            var pageManager = providerName == null ? PageManager.GetManager() : PageManager.GetManager(providerName);
            return pageManager;
        }
    }
}