using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.OpenAccess;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.ContentLinks;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Events;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.GenericContent;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class DataEventProcessor : IDataEventProcessor
    {
        private const string ParentType = "Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile";

        private readonly IApplicationLogger applicationLogger;
        private readonly ICompositePageBuilder compositePageBuilder;
        private readonly IMicroServicesPublishingService compositeUIService;
        private readonly IAsyncHelper asyncHelper;
        private readonly IDataEventActions dataEventActions;
        private readonly IServiceBusMessageProcessor serviceBusMessageProcessor;
        private readonly IDynamicModuleConverter<JobProfileMessage> dynamicContentConverter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public DataEventProcessor(IApplicationLogger applicationLogger, ICompositePageBuilder compositePageBuilder, IMicroServicesPublishingService compositeUIService, IAsyncHelper asyncHelper, IDataEventActions dataEventActions, IDynamicModuleConverter<JobProfileMessage> dynamicContentConverter, IServiceBusMessageProcessor serviceBusMessageProcessor, IDynamicContentExtensions dynamicContentExtensions)
        {
            this.applicationLogger = applicationLogger;
            this.compositePageBuilder = compositePageBuilder;
            this.compositeUIService = compositeUIService;
            this.asyncHelper = asyncHelper;
            this.dataEventActions = dataEventActions;
            this.dynamicContentConverter = dynamicContentConverter;
            this.serviceBusMessageProcessor = serviceBusMessageProcessor;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public void PublishDynamicContent(IDynamicContentUpdatedEvent eventInfo)
        {
            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            GenerateRelatedJobProfilesByTaxon(eventInfo.Item);
            try
            {
                switch (eventInfo.Item.GetType().FullName)
                {
                    case ParentType:
                        if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished &&
                     eventInfo.Item.Status.ToString() == Constants.ItemStatusLive)
                        {
                            GenerateServiceBusMessage(eventInfo);
                        }
                        else if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusUnpublished &&
                    eventInfo.Item.Status.ToString() == Constants.ItemActionDeleted)
                        {
                        }

                        break;
                    default:
                        if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished &&
                    eventInfo.Item.Status.ToString() == Constants.ItemStatusMaster)
                        {
                            GenerateServiceBusMessage(eventInfo);
                        }
                        else if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusUnpublished &&
                    eventInfo.Item.Status.ToString() == Constants.ItemActionDeleted)
                        {
                        }

                        break;
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
            var itemId = eventInfo.Item.Id;
            var providerName = eventInfo.Item.ProviderName;
            var contentType = eventInfo.Item.GetType();
            var dynamicContent = eventInfo.Item;

            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();

            //IOrganizableProvider contentProvider = dynamicModuleManager.Provider as IOrganizableProvider;
            //var items = contentProvider.GetItemsByTaxon(itemId,true,);
            if (contentType.FullName == ParentType)
            {
                JobProfileMessage jobprofileData = dynamicContentConverter.ConvertFrom(dynamicContent);
                serviceBusMessageProcessor.SendJobProfileMessage(jobprofileData, contentType.Name, dynamicContent.ApprovalWorkflowState.Value);
            }
            else
            {
                // Here you get the IDs of items which has as related data that Author
                var parentItemContentLinks = contentLinksManager.GetContentLinks()
                    .Where(c => c.ParentItemType == ParentType && c.ChildItemId == itemId)
                    .Select(c => c.ParentItemId).ToList();

                var relatedWYDTypes = GetWYDRelatedItems(eventInfo.Item, parentItemContentLinks, dynamicModuleManager, ParentType);
                serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedWYDTypes, contentType.Name, dynamicContent.ApprovalWorkflowState.Value);
            }
        }

        private IEnumerable<RelatedContentItem> GetWYDRelatedItems(DynamicContent childItem, List<Guid> parentItemLinks, DynamicModuleManager dynamicModuleManager, string parentName)
        {
            var relatedContentItems = new List<RelatedContentItem>();
            var parentType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile");
            foreach (var contentId in parentItemLinks)
            {
                var parentItem = dynamicModuleManager.GetDataItem(parentType, contentId);

                relatedContentItems.Add(new RelatedContentItem
                {
                    JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(parentItem, nameof(RelatedContentItem.Id)),
                    JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(parentItem, nameof(RelatedContentItem.Title)),
                    Id = dynamicContentExtensions.GetFieldValue<Guid>(childItem, nameof(RelatedContentItem.Id)),
                    Title = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(RelatedContentItem.Title)),
                    Description = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(RelatedContentItem.Description)),
                    Url = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, Constants.Url),
                    IsNegative = dynamicContentExtensions.GetFieldValue<bool>(childItem, nameof(RelatedContentItem.IsNegative))
                });
            }

            return relatedContentItems;
        }

        private void GenerateRelatedJobProfilesByTaxon(DynamicContent content)
        {
            var taxonomyManager = TaxonomyManager.GetManager();
            var taxon = taxonomyManager.GetTaxa<FlatTaxon>()
                .Where(t => t.Title == "philosopher")
                .FirstOrDefault();

            var relatedContent = GetRelatedDataByTaxon<DynamicContent>(content, "Job Profile Specialism", "philosopher", taxon.Id);
            var relatedContent2 = GetRelatedDataByTaxon2<DynamicContent>(content, "Job Profile Specialism", "philosopher", taxon.Id);
        }

        private List<T> GetRelatedDataByTaxon<T>(object item, string relatedDataFieldName, string taxonFieldName, Guid taxonId)
            where T : IDataItem, IDynamicFieldsContainer
        {
            var relatedItems = item.GetRelatedItems<T>(relatedDataFieldName)
                .Where(p => p.GetValue<TrackedList<Guid>>(taxonFieldName).Contains(taxonId))
                .ToList();

            return relatedItems;
        }

        private List<T> GetRelatedDataByTaxon2<T>(object item, string relatedDataFieldName, string taxonFieldName, Guid taxonId)
            where T : IDataItem, IDynamicFieldsContainer
        {
            var manager = ManagerBase.GetMappedManager(typeof(T));
            IOrganizableProvider contentProvider = manager.Provider as IOrganizableProvider;
            int? totalCount = -1;
            var items = contentProvider.GetItemsByTaxon(taxonId, false, taxonFieldName, typeof(T), null, null, 0, 0, ref totalCount)
                .Cast<T>()
                .Select(p => p.Id)
                .ToList();

            var relatedItems = item.GetRelatedItems<T>(relatedDataFieldName)
                .Where(ri => items.Contains(ri.Id))
                .ToList();

            return relatedItems;
        }
    }
}
