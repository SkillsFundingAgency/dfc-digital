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

                var hasPageChanged = eventInfo.GetPropertyValue<bool>(Constants.HasPageDataChanged);
                var workFlowStatus = eventInfo.GetPropertyValue<string>(Constants.ApprovalWorkflowState);
                var status = eventInfo.GetPropertyValue<string>(Constants.ItemStatus);
                var changedProperties = eventInfo.GetPropertyValue<IDictionary<string, PropertyChange>>(Constants.ChangedProperties);

                if (action == Constants.ItemActionUpdated && workFlowStatus == Constants.WorkFlowStatusPublished && status == Constants.ItemStatusLive)
                {
                    var manager = sitefinityManager.GetManager(providerName);
                    var item = manager.GetItemOrDefault(contentType, itemId);

                    if (contentType == typeof(PageNode) && (hasPageChanged || changedProperties.Count > 0))
                    {
                        var pageNodeItem = (PageNode)item;
                        var compositePageData = GetCompositePageForPageNode(pageNodeItem);
                        compositeUIService.PostPageDataAsync(compositePageData);
                    }

                    /*
                     //Checking by type did not work
                     else if (contentType.Name == Constants.JobProfile)
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

        public CompositePageData GetCompositePageForPageNode(PageNode pageNode)
        {
            var compositePageData = new CompositePageData() { Name = pageNode.UrlName.Value };

            compositePageData.IncludeInSitemap = pageNode.Crawlable;
            var pageData = pageNode.GetPageData();
            compositePageData.Title = pageData.HtmlTitle.Value;
            compositePageData.Description = pageData.Description.Value;
            compositePageData.KeyWords = pageData.Keywords.Value;
            compositePageData.Content = GetPageControlsData(pageData);
            return compositePageData;
        }

        public IList<string> GetPageControlsData(PageData pageData)
        {
            var contentData = new List<string>();
            var pManager = sitefinityManager.GetManager();

            //This bit came from Sitefinity Support.
            var pageDraftControls = pageData.Controls.Where(c => c.Caption == Constants.ContentBlock);
            foreach (var pageDraftControl in pageDraftControls)
            {
                Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy control =
                    pManager.LoadControl(pageDraftControl) as Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy;
                contentData.Add(control.Settings.Values[Constants.Content]);
            }

            return contentData;
        }
    }
}
