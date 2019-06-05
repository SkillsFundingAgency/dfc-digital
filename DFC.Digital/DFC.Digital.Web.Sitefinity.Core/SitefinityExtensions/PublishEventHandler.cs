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

        private void Content_Action(IDataEvent eventInfo)
        {
            try
            {
                var dumpFolder = "SitefinityDataDump";
                var action = eventInfo.Action;
                var contentType = eventInfo.ItemType;
                var itemId = eventInfo.ItemId;
                var providerName = eventInfo.ProviderName;
                var manager = ManagerBase.GetMappedManager(contentType, providerName);

                var hasPageChanged = eventInfo.GetPropertyValue<bool>("HasPageDataChanged");
                var workFlowStatus = eventInfo.GetPropertyValue<string>("ApprovalWorkflowState");
                var status = eventInfo.GetPropertyValue<string>("Status");

                var serialzerSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new AllPropertiesResolver(),

                    Converters = new[] { new LstringJsonConverter() }
                };

                if (action == "Updated" && workFlowStatus == "Published")
                {
                    //Checking by type did not work
                    if (contentType.Name == "JobProfile" && status == "Live")
                    {
                        var item = manager.GetItemOrDefault(contentType, itemId);
                        var dynamicContent = (Telerik.Sitefinity.DynamicModules.Model.DynamicContent)item;
                        var jsonString = JsonConvert.SerializeObject(item, serialzerSettings);

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter($"C:\\{dumpFolder}\\{contentType.Name}-{dynamicContent.UrlName}.json", false))
                        {
                            file.Write(jsonString);
                            file.WriteLine("---------------------------------------------------------------------------");
                            file.Flush();
                        }
                    }
                    else if (contentType == typeof(PageNode))
                    {
                        var item = (PageNode)manager.GetItemOrDefault(contentType, itemId);

                        var contentData = GetPageContentBlocks(item.Title.ToString());
                        var jsonString = JsonConvert.SerializeObject(contentData);

                        var htmlContent = RenderPageToString(item.Title.ToString());

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter($"C:\\{dumpFolder}\\{item.Title.ToString()}.html", false))
                        {
                            file.Write(htmlContent);
                            file.Flush();
                        }

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter($"C:\\{dumpFolder}\\{item.Title.ToString()}.json", false))
                        {
                            file.Write(jsonString);
                            file.Flush();
                        }
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

        private List<ContentDataHTML> GetPageContentBlocks(string pageNodeTitle)
        {
            var contentData = new List<ContentDataHTML>();

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
                contentData.Add(new ContentDataHTML() { ContentName = pageDraftControl.Caption, Content = control.Settings.Values["Content"] });
            }

            return contentData;
        }
    }
}
