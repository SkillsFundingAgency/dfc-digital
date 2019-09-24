using DFC.Digital.Core;
using System;
using System.Linq;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.DynamicTypes.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;

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

        public DynamicContent GetDynamicContentTypeNode(Type contentType, Guid itemId, string providerName)
        {
            var item = GetDynamicContentManagerForProvider(itemId, providerName);
            var contentNode = (DynamicContent)item;
            return contentNode;
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

        public PageDraft GetPreviewPageDataById(Guid id)
        {
            return GetPageManagerForProvider().GetPreview(id);
        }

        private PageManager GetPageManagerForProvider(string providerName = null)
        {
            return providerName == null ? PageManager.GetManager() : PageManager.GetManager(providerName);
        }

        //private DynamicContent RetrieveJobProfileByID(string providerName, Guid jobprofileID)
        //{
        //    // Set the provider name for the DynamicModuleManager here. All available providers are listed in
        //    // Administration -> Settings -> Advanced -> DynamicModules -> Providers
        //    // Set a transaction name

        //}
        private DynamicContent GetDynamicContentManagerForProvider(Guid itemId, string providerName = null)
        {
            var transactionName = DateTime.Today.ToString("ddMMYYYYmmsstt");

            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(providerName, transactionName);
            Type jobProfileType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile");
            Guid jobProfileID = itemId;

            // This is how we get the jobProfile item by ID
            DynamicContent jobProfileItem = dynamicModuleManager.GetDataItem(jobProfileType, jobProfileID);
            return jobProfileItem;
        }
    }
}