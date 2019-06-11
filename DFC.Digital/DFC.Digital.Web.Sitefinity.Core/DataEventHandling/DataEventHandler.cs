using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Publishing;
using Telerik.Sitefinity.Web.ResourceCombining;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class DataEventHandler : IDataEventHandler
    {
        private readonly IApplicationLogger applicationLogger;
        private readonly ISitefinityManager sitefinityManager;
        private readonly ICompositeUIService compositeUIService;

        public DataEventHandler(IApplicationLogger applicationLogger, ISitefinityManager sitefinityManager, ICompositeUIService compositeUIService)
        {
            this.applicationLogger = applicationLogger;
            this.sitefinityManager = sitefinityManager;
            this.compositeUIService = compositeUIService;
        }

        public void ExportCompositePage(IDataEvent eventInfo)
        {
            try
            {
                var action = eventInfo.Action;
                var contentType = eventInfo.ItemType;
                var itemId = eventInfo.ItemId;
                var providerName = eventInfo.ProviderName;

                var hasPageChanged = eventInfo.GetPropertyValue<bool>("HasPageDataChanged");
                var workFlowStatus = eventInfo.GetPropertyValue<string>("ApprovalWorkflowState");
                var status = eventInfo.GetPropertyValue<string>("Status");
                var changedProperties = eventInfo.GetPropertyValue<IDictionary<string, PropertyChange>>("ChangedProperties");

                if (action == "Updated" && workFlowStatus == "Published" && status == "Live")
                {
                    var manager = sitefinityManager.GetManager(providerName);
                    var item = manager.GetItemOrDefault(contentType, itemId);

                    if (contentType == typeof(PageNode) && (hasPageChanged || changedProperties.Count > 0))
                    {
                        var pageNodeItem = (PageNode)item;
                        var pageTitle = pageNodeItem.Title.Value;
                        var compositePageData = GetCompositePageForPageNode(pageNodeItem);
                        compositeUIService.PostPageDataAsync(compositePageData);
                    }

                    /*
                     //Checking by type did not work
                     if (contentType.Name == "JobProfile")
                     {
                        var dynamicContent = (Telerik.Sitefinity.DynamicModules.Model.DynamicContent)item;
                     }
                    */
                }
            }
            catch (Exception ex)
            {
                applicationLogger.Error("Failed to export page data", ex);
            }
        }

        public CompositePageData GetCompositePageForPageNode(PageNode node)
        {
            var compositePageData = new CompositePageData() { Name = node.UrlName.Value };

            compositePageData.IncludeInSitemap = node.Crawlable;
            var pageData = node.GetPageData();
            compositePageData.Title = pageData.HtmlTitle.Value;
            compositePageData.MetaDescription = pageData.Description.Value;
            compositePageData.MetaKeyWords = pageData.Keywords.Value;
            compositePageData.Content = GetPageControlsData(pageData);
            return compositePageData;
        }

        public IList<string> GetPageControlsData(PageData pageData)
        {
            var contentData = new List<string>();
            var pManager = sitefinityManager.GetManager();

            //This bit came from Sitefinity Support.
            var pageDraftControls = pageData.Controls.Where(c => c.Caption == "Content block");
            foreach (var pageDraftControl in pageDraftControls)
            {
                pageDraftControl.SetPersistanceStrategy();
                Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy control =
                    pManager.LoadControl(pageDraftControl) as Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy;
                contentData.Add(control.Settings.Values["Content"]);
            }

            return contentData;
        }
    }
}
