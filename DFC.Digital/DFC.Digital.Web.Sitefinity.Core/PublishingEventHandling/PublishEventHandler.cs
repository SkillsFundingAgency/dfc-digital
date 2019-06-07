using DFC.Digital.Core;
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

namespace DFC.Digital.Web.Sitefinity.Core.SitefinityExtensions
{
    public class PublishEventHandler
    {
        private readonly IApplicationLogger applicationLogger;

        public PublishEventHandler(IApplicationLogger applicationLogger)
        {
            this.applicationLogger = applicationLogger;
        }

        public void Content_Action(IDataEvent eventInfo)
        {
            try
            {
                var action = eventInfo.Action;
                var contentType = eventInfo.ItemType;
                var itemId = eventInfo.ItemId;
                var providerName = eventInfo.ProviderName;
                var manager = ManagerBase.GetMappedManager(contentType, providerName);

                var hasPageChanged = eventInfo.GetPropertyValue<bool>("HasPageDataChanged");
                var workFlowStatus = eventInfo.GetPropertyValue<string>("ApprovalWorkflowState");
                var status = eventInfo.GetPropertyValue<string>("Status");

                if (action == "Updated" && workFlowStatus == "Published")
                {
                    //Checking by type did not work
                    if (contentType.Name == "JobProfile" && status == "Live")
                    {
                        var item = manager.GetItemOrDefault(contentType, itemId);
                        var dynamicContent = (Telerik.Sitefinity.DynamicModules.Model.DynamicContent)item;
                    }
                    else if (contentType == typeof(PageNode))
                    {
                        var item = (PageNode)manager.GetItemOrDefault(contentType, itemId);

                        var contentData = GetPageContentBlocks(item.Title.ToString());
                        var jsonString = JsonConvert.SerializeObject(contentData);

                        var htmlContent = RenderPageToString(item.Title.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                applicationLogger.Error("Failed to export page data", ex);
            }
        }

        private string RenderPageToString(string pageNodeTitle)
        {
            var manager = PageManager.GetManager();
            var node = manager.GetPageNodes()
            .Where(t => t.Title == pageNodeTitle)
            .FirstOrDefault();

            if (node != null)
            {
                InMemoryPageRender renderer = new InMemoryPageRender();
                return renderer.RenderPage(node, false, true);
            }

            return string.Empty;
        }

        private List<CompositePageData> GetPageContentBlocks(string pageNodeTitle)
        {
            var contentData = new List<CompositePageData>();

            var manager = PageManager.GetManager();
            var node = manager.GetPageNodes().Where(t => t.Title == pageNodeTitle).FirstOrDefault();

            var pageData = node.GetPageData();

            PageManager pManager = PageManager.GetManager();

            //This bit came from Sitefinity Support.
            var pageDraftControls = pageData.Controls.Where(c => c.Caption == "Content block");
            foreach (var pageDraftControl in pageDraftControls)
            {
                pageDraftControl.SetPersistanceStrategy();
                Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy control =
                    pManager.LoadControl(pageDraftControl) as Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy;
                contentData.Add(new CompositePageData() { Name = pageDraftControl.Caption, Content = control.Settings.Values["Content"] });
            }

            return contentData;
        }
    }
}
