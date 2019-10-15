using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Web.Sitefinity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Data.ContentLinks;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Builder.Model;
using Telerik.Sitefinity.DynamicModules.Events;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.DynamicTypes.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Model.ContentLinks;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.RelatedData;
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

        public DataEventProcessor(IApplicationLogger applicationLogger, ICompositePageBuilder compositePageBuilder, IMicroServicesPublishingService compositeUIService, IAsyncHelper asyncHelper, IDataEventActions dataEventActions, IServiceBusMessageProcessor serviceBusMessageProcessor)
        {
            this.applicationLogger = applicationLogger;
            this.compositePageBuilder = compositePageBuilder;
            this.compositeUIService = compositeUIService;
            this.asyncHelper = asyncHelper;
            this.dataEventActions = dataEventActions;
            this.serviceBusMessageProcessor = serviceBusMessageProcessor;
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
                var dynamicContent = eventInfo.Item;
                var relatedData = eventInfo.GetRelatedItems("Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile");
                if (microServicesDataEventAction == MicroServicesDataEventAction.PublishedOrUpdated)
                {
                    ExportDynamicContentTypeNode(eventInfo.Item.ApprovalWorkflowState, providerName, contentType, itemId, dynamicContent, relatedData);
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

            if (eventInfo.ItemType == typeof(PageNode))
            {
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
            var compositePageData = compositePageBuilder.GetPublishedJobProfileDynamicContent(itemId);

            //serviceBusMessageProcessor.SendMessage(compositePageData, "Published");
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

        private IQueryable<IDataItem> GetRelationsByParent(Guid itemId, string parentItemProviderName, string parentItemTypeName, string propertyName)
        {
            var parentTitle = "Headteacher";
            var relatedTitle = "Prospects";

            // Set a transaction name
            var transactionName = "someTransactionName";
            var providerName = "dynamicProvider2";
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(providerName, transactionName);
            Type jobProfileType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile");

            // This is how we get the collection of Job Profile items
            var myCollection = dynamicModuleManager.GetDataItems(jobProfileType).Where(i => i.GetValue<string>("Title").Contains(parentTitle));
            ContentLinksManager contentLinksManager = ContentLinksManager.GetManager();

            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                    .Where(c => c.ParentItemType == jobProfileType.Name && c.ChildItemId == itemId)
                    .Select(c => c.ParentItemId).ToList();

            var result = dynamicModuleManager.GetDataItems(jobProfileType)
.Where(d => parentItemContentLinks.Contains(d.OriginalContentId)
&& d.Status == ContentLifecycleStatus.Live);

            // At this point myCollection contains the items from type jobProfileType
            // return myCollection;
            Type parentType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile");
            Type relatedType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.JobProfile.UniversityLink");

            // get the live version of all parent items
            var parentItems = dynamicModuleManager.GetDataItems(jobProfileType);

            //.Where(i => i.GetValue<string>("Title").Contains(parentTitle));

            //&& i.Status == ContentLifecycleStatus.Live && i.Visible);

            // get the ids of the related items.
            // We use the OriginalContentId property since we work with the live vesrions of the dynamic modules
            var parentItemIds = parentItems.Select(i => i.OriginalContentId).ToList();

            // get the live versions of all the schedules items
            var relatedItems = dynamicModuleManager.GetDataItems(relatedType).Where(i => i.Status == ContentLifecycleStatus.Live && i.Visible && i.GetValue<string>("Title").Contains(relatedTitle));

            // get the content links
            var contentLinks = contentLinksManager.GetContentLinks().Where(cl => cl.ParentItemType == parentType.FullName && cl.ComponentPropertyName == "RelatedField" && parentItemIds.Contains(cl.ParentItemId) && cl.AvailableForLive);

            // get the IDs of the desired parent items
            var filteredParentItemIds = contentLinks.Join<ContentLink, DynamicContent, Guid, Guid>(relatedItems, (cl) => cl.ChildItemId, (i) => i.OriginalContentId, (cl, i) => cl.ParentItemId).Distinct();

            // get the desired parent items by the filtered IDs
            var filteredParentItems = parentItems.Where(i => filteredParentItemIds.Contains(i.OriginalContentId)).ToList();

            ContentLinksManager contentLinksManager2 = ContentLinksManager.GetManager();

            var linksToRelatedItems = contentLinksManager2.GetContentLinks()
                .Where(cl => cl.ParentItemId == itemId &&
                    cl.ParentItemProviderName == parentItemProviderName &&
                    cl.ParentItemType == parentItemTypeName);

            //&& cl.ComponentPropertyName == propertyName);
            return linksToRelatedItems;
        }

        private IQueryable<IDataItem> GetRelationsByChild(Guid childItemId, string childItemType, string childItemProviderName, string parentItemTypeFullName)
        {
            ContentLinksManager contentLinksManager = ContentLinksManager.GetManager();

            var links = contentLinksManager.GetContentLinks()
                .Where(cl => cl.ChildItemId == childItemId &&
                    cl.ChildItemType == childItemType &&
                    cl.ChildItemProviderName == childItemProviderName &&
                    cl.ParentItemType == parentItemTypeFullName);

            return links;
        }

        private void ExportDynamicContentTypeNode(string eventStatus, string providerName, Type contentType, Guid itemId, DynamicContent dynamicContent, IQueryable<IDataItem> relatedData)
        {
            Type parentType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile");
            Type universityLinkType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.JobProfile.UniversityLink");
            if (contentType == parentType)
            {
                var jobprofileData = compositePageBuilder.GetPublishedJobProfileDynamicContent(itemId);

                jobprofileData.CType = contentType.Name;
                jobprofileData.ActionType = eventStatus;
                serviceBusMessageProcessor.SendMessage(jobprofileData, eventStatus);
            }
            else
            {
                //var relatedParentItems = GetRelationsByParent(itemId, "dynamicProvider2", contentType.Name, "JobProfile");
                //var relatedChildItems = GetRelationsByChild(itemId, "Telerik.Sitefinity.DynamicTypes.Model.JobProfile.UniversityLink", "dynamicProvider2", "Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile");
                // Set a transaction name
                var transactionName = "someTransactionName";
                var defaultProviderName = "dynamicProvider2";
                DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(defaultProviderName, transactionName);
                ContentLinksManager contentLinksManager = ContentLinksManager.GetManager();

                // This is how we get the collection of Job Profile items
                var myCollection = dynamicModuleManager.GetDataItems(universityLinkType).Where(i => i.GetValue<string>("Title").Contains("PGCE courses") && i.Status == ContentLifecycleStatus.Live);

                var universityLinkItem = dynamicModuleManager.GetDataItem(universityLinkType, myCollection.FirstOrDefault().Id);
                var relatedUItems = universityLinkItem.GetRelatedItems("JobProfile").ToList();
                var relatedParentUItems = universityLinkItem.GetRelatedParentItems(parentType.FullName).ToList();
                var relatedParentItemsList = universityLinkItem.GetRelatedParentItemsList(parentType.FullName);

                var parentItemContentLinks = contentLinksManager.GetContentLinks()
        .Where(c => c.ParentItemType == parentType.Name && c.ChildItemId == myCollection.FirstOrDefault().Id)
        .Select(c => c.ParentItemId).ToList();

                // here you get the filtered dynamic module items by their IDs
                var result = dynamicModuleManager.GetDataItems(parentType)
                    .Where(d => parentItemContentLinks.Contains(d.OriginalContentId)
                    && d.Status == ContentLifecycleStatus.Live);
            }
        }
    }
}
