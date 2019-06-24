using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class DataEventHandler : IDataEventHandler
    {
        private readonly IApplicationLogger applicationLogger;
        private readonly ICompositePageBuilder compositePageBuilder;
        private readonly IMicroServicesPublishingService compositeUIService;
        private readonly ISitefinityDataEventProxy sitefinityDataEventProxy;
        private readonly IAsyncHelper asyncHelper;

        public DataEventHandler(IApplicationLogger applicationLogger, ICompositePageBuilder compositePageBuilder, ISitefinityDataEventProxy sitefinityDataEventProxy, IMicroServicesPublishingService compositeUIService, IAsyncHelper asyncHelper)
        {
            this.applicationLogger = applicationLogger;
            this.compositePageBuilder = compositePageBuilder;
            this.compositeUIService = compositeUIService;
            this.sitefinityDataEventProxy = sitefinityDataEventProxy;
            this.asyncHelper = asyncHelper;
        }

        public void ExportCompositePage(IDataEvent eventInfo)
        {
            try
            {
                var action = eventInfo.Action;
                var contentType = eventInfo.ItemType;
                var itemId = eventInfo.ItemId;
                var providerName = eventInfo.ProviderName;

                var hasPageChanged = sitefinityDataEventProxy.GetPropertyValue<bool>(eventInfo, Constants.HasPageDataChanged);
                var workFlowStatus = sitefinityDataEventProxy.GetPropertyValue<string>(eventInfo, Constants.ApprovalWorkflowState);
                var status = sitefinityDataEventProxy.GetPropertyValue<string>(eventInfo, Constants.ItemStatus);
                var changedProperties = sitefinityDataEventProxy.GetPropertyValue<IDictionary<string, PropertyChange>>(eventInfo, Constants.ChangedProperties);

                if (action == Constants.ItemActionUpdated && workFlowStatus == Constants.WorkFlowStatusPublished && status == Constants.ItemStatusLive)
                {
                    if (contentType == typeof(PageNode) && (hasPageChanged || changedProperties.Count > 0))
                    {
                        ExportPageNode(providerName, contentType, itemId);
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
                applicationLogger.ErrorJustLogIt($"Failed to export page data for item id {eventInfo.ItemId}", ex);
            }
        }

        private void ExportPageNode(string providerName, Type contentType, Guid itemId)
        {
            var microServiceEndPointConfigKey = compositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(providerName, contentType, itemId);
            if (!microServiceEndPointConfigKey.IsNullOrEmpty())
            {
                var compositePageData = compositePageBuilder.GetCompositePageForPageNode(providerName, contentType, itemId);
                var result = asyncHelper.Synchronise(() => compositeUIService.PostPageDataAsync(microServiceEndPointConfigKey, compositePageData));
            }
        }
    }
}
