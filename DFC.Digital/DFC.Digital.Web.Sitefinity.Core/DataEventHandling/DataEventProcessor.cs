using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class DataEventProcessor : IDataEventProcessor
    {
        private readonly IApplicationLogger applicationLogger;
        private readonly ICompositePageBuilder compositePageBuilder;
        private readonly IMicroServicesPublishingService compositeUIService;
        private readonly ISitefinityDataEventProxy sitefinityDataEventProxy;
        private readonly IAsyncHelper asyncHelper;

        public DataEventProcessor(IApplicationLogger applicationLogger, ICompositePageBuilder compositePageBuilder, ISitefinityDataEventProxy sitefinityDataEventProxy, IMicroServicesPublishingService compositeUIService, IAsyncHelper asyncHelper)
        {
            this.applicationLogger = applicationLogger;
            this.compositePageBuilder = compositePageBuilder;
            this.compositeUIService = compositeUIService;
            this.sitefinityDataEventProxy = sitefinityDataEventProxy;
            this.asyncHelper = asyncHelper;
        }

        public void ExportCompositePage(IDataEvent eventInfo)
        {
            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            try
            {
                var microServicesDataEventAction = GetEventAction(eventInfo);

                var itemId = eventInfo.ItemId;
                var providerName = eventInfo.ProviderName;
                var contentType = eventInfo.ItemType;

                //Ignore any workflow property changes
                var changedProperties = sitefinityDataEventProxy.GetPropertyValue<IDictionary<string, PropertyChange>>(eventInfo, Constants.ChangedProperties);
                var filteredProperties = changedProperties.Where(p => p.Key != Constants.ApprovalWorkflowState).Count();

                var hasPageChanged = sitefinityDataEventProxy.GetPropertyValue<bool>(eventInfo, Constants.HasPageDataChanged);

                if (microServicesDataEventAction == MicroServicesDataEventAction.PublishedOrUpdated)
                {
                    if (contentType == typeof(PageNode) && (hasPageChanged || filteredProperties > 0))
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
                else if (microServicesDataEventAction == MicroServicesDataEventAction.UnpublishedOrDeleted)
                {
                    DeletePage(providerName, contentType, itemId);
                }
            }
            catch (Exception ex)
            {
                applicationLogger.ErrorJustLogIt($"Failed to export page data for item id {eventInfo.ItemId}", ex);
                throw;
            }
        }

        public MicroServicesDataEventAction GetEventAction(IDataEvent eventInfo)
        {
            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            var itemAction = eventInfo.Action;
            var workFlowStatus = sitefinityDataEventProxy.GetPropertyValue<string>(eventInfo, Constants.ApprovalWorkflowState);
            var itemStatus = sitefinityDataEventProxy.GetPropertyValue<string>(eventInfo, Constants.ItemStatus);
            var recycleBinAction = sitefinityDataEventProxy.GetPropertyValue<RecycleBinAction>(eventInfo, Constants.RecycleBinAction);

            if (itemAction == Constants.ItemActionDeleted || (workFlowStatus == Constants.WorkFlowStatusUnPublished && recycleBinAction != RecycleBinAction.RestoreFromRecycleBin))
            {
                //Unpublished or deleted Check for this first
                return MicroServicesDataEventAction.UnpublishedOrDeleted;
            }
            else if (workFlowStatus == Constants.WorkFlowStatusPublished && itemStatus == Constants.ItemStatusLive)
            {
                //Published
                return MicroServicesDataEventAction.PublishedOrUpdated;
            }

            return MicroServicesDataEventAction.Ignored;
        }

        private void DeletePage(string providerName, Type contentType, Guid itemId)
        {
            var microServiceEndPointConfigKey = compositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(providerName, contentType, itemId);
            if (!microServiceEndPointConfigKey.IsNullOrEmpty())
            {
                asyncHelper.Synchronise(() => compositeUIService.DeletePageAsync(microServiceEndPointConfigKey, itemId));
            }
        }

        private void ExportPageNode(string providerName, Type contentType, Guid itemId)
        {
            var microServiceEndPointConfigKey = compositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(providerName, contentType, itemId);
            if (!microServiceEndPointConfigKey.IsNullOrEmpty())
            {
                var compositePageData = compositePageBuilder.GetCompositePageForPageNode(providerName, contentType, itemId);
                asyncHelper.Synchronise(() => compositeUIService.PostPageDataAsync(microServiceEndPointConfigKey, compositePageData));
            }
        }
    }
}
