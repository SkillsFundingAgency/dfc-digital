using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Data.ContentLinks;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Events;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
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
        private readonly IServiceBusMessageProcessor serviceBusMessageProcessor;
        private readonly IDynamicModuleConverter<JobProfileMessage> dynamicContentConverter;

        public DataEventProcessor(IApplicationLogger applicationLogger, ICompositePageBuilder compositePageBuilder, IMicroServicesPublishingService compositeUIService, IAsyncHelper asyncHelper, IDataEventActions dataEventActions, IDynamicModuleConverter<JobProfileMessage> dynamicContentConverter, IServiceBusMessageProcessor serviceBusMessageProcessor)
        {
            this.applicationLogger = applicationLogger;
            this.compositePageBuilder = compositePageBuilder;
            this.compositeUIService = compositeUIService;
            this.asyncHelper = asyncHelper;
            this.dataEventActions = dataEventActions;
            this.dynamicContentConverter = dynamicContentConverter;
            this.serviceBusMessageProcessor = serviceBusMessageProcessor;
        }

        public void PublishDynamicContent(IDynamicContentUpdatedEvent eventInfo)
        {
            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            try
            {
                if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished &&
                    eventInfo.Item.Status.ToString() == Constants.ItemStatusLive)
                {
                    GenerateServiceBusMessage(eventInfo);
                }
                else if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusUnpublished &&
                    eventInfo.Item.Status.ToString() == Constants.ItemActionDeleted)
                {
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

            if (eventInfo.ItemType != typeof(PageNode))
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

        private void DeletePage(string providerName, Type contentType, Guid itemId)
        {
            var microServiceEndPointConfigKey = compositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(contentType, itemId, providerName);
            if (!microServiceEndPointConfigKey.IsNullOrEmpty())
            {
                asyncHelper.Synchronise(() => compositeUIService.DeletePageAsync(microServiceEndPointConfigKey, itemId));
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

        private void GenerateServiceBusMessage(IDynamicContentUpdatedEvent eventInfo)
        {
            Type parentType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile");
            var itemId = eventInfo.Item.Id;
            var providerName = eventInfo.Item.ProviderName;
            var contentType = eventInfo.Item.GetType();
            var dynamicContent = eventInfo.Item;

            if (contentType == parentType)
            {
                JobProfileMessage jobprofileData = dynamicContentConverter.ConvertFrom(dynamicContent);
                jobprofileData.CType = contentType.Name;
                jobprofileData.ActionType = dynamicContent.ApprovalWorkflowState.Value;
                serviceBusMessageProcessor.SendMessage(jobprofileData);
            }
            else
            {
                //Work in progress to PUBLISH related dynamic content and its parent items
            }
        }
    }
}
