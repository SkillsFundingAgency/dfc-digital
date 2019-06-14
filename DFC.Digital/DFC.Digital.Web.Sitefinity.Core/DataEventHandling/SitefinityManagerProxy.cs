using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
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

        public MvcControllerProxy LoadControl(string providerName, PageControl pageControl)
        {
            return PageManager.GetManager(providerName).LoadControl(pageControl) as Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy;
        }
    }
}