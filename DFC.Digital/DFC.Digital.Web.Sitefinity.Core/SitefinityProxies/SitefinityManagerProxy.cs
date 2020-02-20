using DFC.Digital.Core;
using DFC.Digital.Repository.SitefinityCMS;
using System;
using System.Linq;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.GenericContent;
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
            if (control.Settings.Values[Constants.SharedContent] == Guid.Empty)
            {
                return control.Settings.Values[Constants.Content];
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetControlContent(PageDraftControl pageDraftControl)
        {
            var control = GetPageManagerForProvider().LoadControl(pageDraftControl) as Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy;
            return control.Settings.Values[Constants.Content];
        }

        public string GetControlSharedContent(PageControl pageControl, IDynamicContentExtensions dynamicContentExtensions, string providerName)
        {
            var control = GetPageManagerForProvider(providerName).LoadControl(pageControl) as Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy;
            var sharedContentId = control.Settings.Values[Constants.SharedContent];
            if (control.Settings.Values[Constants.SharedContent] != Guid.Empty)
            {
                ContentManager manager = ContentManager.GetManager();
                ContentItem sharedContent = manager.GetContent(sharedContentId);
                return sharedContent.Content.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public PageDraft GetPreviewPageDataById(Guid id)
        {
            return GetPageManagerForProvider().GetPreview(id);
        }

        private PageManager GetPageManagerForProvider(string providerName = null)
        {
            return providerName == null ? PageManager.GetManager() : PageManager.GetManager(providerName);
        }
    }
}