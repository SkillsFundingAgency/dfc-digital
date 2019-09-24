using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.DynamicModules.Events;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.DynamicTypes.Model;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class DataEventProcessor : IDataEventProcessor
    {
        private readonly IApplicationLogger applicationLogger;
        private readonly ICompositePageBuilder compositePageBuilder;
        private readonly IMicroServicesPublishingService compositeUIService;
        private readonly IAsyncHelper asyncHelper;
        private readonly IDataEventActions dataEventActions;

        public DataEventProcessor(IApplicationLogger applicationLogger, ICompositePageBuilder compositePageBuilder, IMicroServicesPublishingService compositeUIService, IAsyncHelper asyncHelper, IDataEventActions dataEventActions)
        {
            this.applicationLogger = applicationLogger;
            this.compositePageBuilder = compositePageBuilder;
            this.compositeUIService = compositeUIService;
            this.asyncHelper = asyncHelper;
            this.dataEventActions = dataEventActions;
        }

        public void ExportCompositeDynamicContentUpdated(IDynamicContentUpdatedEvent eventInfo)
        {
            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            Type jobProfileType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile");
            try
            {
                var microServicesDataEventAction = dataEventActions.GetDynamicContentEventAction(eventInfo);

                var itemId = eventInfo.Item.Id;
                var providerName = eventInfo.Item.ProviderName;
                var contentType = eventInfo.Item.GetType();

                if (microServicesDataEventAction == MicroServicesDataEventAction.PublishedOrUpdated)
                {
                    if (dataEventActions.ShouldExportDynamicModuleData(eventInfo))
                    {
                            ExportDynamicContentTypeNode(providerName, contentType, itemId);
                    }
                }
                else if (microServicesDataEventAction == MicroServicesDataEventAction.UnpublishedOrDeleted)
                {
                    DeleteDynamicContentType(providerName, contentType, itemId);
                }
            }
            catch (Exception ex)
            {
                applicationLogger.ErrorJustLogIt($"Failed to export page data for item id {eventInfo.Item.Id}", ex);
                throw;
            }
        }

        public void ExportCompositePage(IDataEvent eventInfo)
        {
            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            Type jobProfileType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile");
            if (eventInfo.ItemType != typeof(PageNode) && eventInfo.ItemType == jobProfileType)
            {
                return;
            }

            try
            {
                var microServicesDataEventAction = dataEventActions.GetEventAction(eventInfo);

                var itemId = eventInfo.ItemId;
                var providerName = eventInfo.ProviderName;
                var contentType = eventInfo.ItemType;

                if (microServicesDataEventAction == MicroServicesDataEventAction.PublishedOrUpdated)
                {
                    if (dataEventActions.ShouldExportPage(eventInfo))
                    {
                        ExportPageNode(providerName, contentType, itemId);
                    }
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

        private void DeletePage(string providerName, Type contentType, Guid itemId)
        {
            var microServiceEndPointConfigKey = compositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(contentType, itemId, providerName);
            if (!microServiceEndPointConfigKey.IsNullOrEmpty())
            {
                asyncHelper.Synchronise(() => compositeUIService.DeletePageAsync(microServiceEndPointConfigKey, itemId));
            }
        }

        private void DeleteDynamicContentType(string providerName, Type contentType, Guid itemId)
        {
            var microServiceEndPointConfigKey = compositePageBuilder.GetMicroServiceEndPointConfigKeyForDynamicContentNode(contentType, itemId, providerName);
            if (!microServiceEndPointConfigKey.IsNullOrEmpty())
            {
                //asyncHelper.Synchronise(() => compositeUIService.DeletePageAsync(microServiceEndPointConfigKey, itemId));
            }
        }

        private void ExportPageNode(string providerName, Type contentType, Guid itemId)
        {
            var microServiceEndPointConfigKey = compositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(contentType, itemId, providerName);

            if (!microServiceEndPointConfigKey.IsNullOrEmpty())
            {
                var compositePageData = compositePageBuilder.GetPublishedPage(contentType, itemId, providerName);
                asyncHelper.Synchronise(() => compositeUIService.PostPageDataAsync(microServiceEndPointConfigKey, compositePageData));
            }
        }

        private void ExportDynamicContentTypeNode(string providerName, Type contentType, Guid itemId)
        {
            //only for testing add the microServiceEndPointConfigKey here
            var microServiceEndPointConfigKey = "DFC.Digital.MicroService.HelpEndPoint";

            //var microServiceEndPointConfigKey = compositePageBuilder.GetMicroServiceEndPointConfigKeyForDynamicContentNode(contentType, itemId, providerName);
            if (!microServiceEndPointConfigKey.IsNullOrEmpty())
            {
                var compositePageData = compositePageBuilder.GetPublishedPage(contentType, itemId, providerName);
                asyncHelper.Synchronise(() => compositeUIService.PostPageDataAsync(microServiceEndPointConfigKey, compositePageData));
            }
        }
    }
}
