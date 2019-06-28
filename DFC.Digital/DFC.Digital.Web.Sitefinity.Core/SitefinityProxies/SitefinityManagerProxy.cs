using DFC.Digital.Core;
using System;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SitefinityManagerProxy : ISitefinityManagerProxy
    {
        public PageNode GetPageNode(string providerName, Type contentType, Guid itemId)
        {
            var item = PageManager.GetManager(providerName).GetItemOrDefault(contentType, itemId);
            var pageNode = (PageNode)item;
            return pageNode;
        }

        public PageData GetPageData(string providerName, Type contentType, Guid itemId)
        {
            return GetPageNode(providerName, contentType, itemId).GetPageData();
        }

        public string GetControlContent(string providerName, PageControl pageControl)
        {
            var control = PageManager.GetManager(providerName).LoadControl(pageControl) as Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy;
            return control.Settings.Values[Constants.Content];
        }

        public string GetLstringValue(Lstring lstring)
        {
            return lstring?.Value;
        }
    }
}