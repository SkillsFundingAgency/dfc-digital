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
        private const string SocSkillsMatrixType = "Telerik.Sitefinity.DynamicTypes.Model.JobProfile.SocSkillsMatrix";
        private const string JobProfileSocType = "Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfileSoc";

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

        public List<Guid> SkillsMatrixParentItems { get; set; }

        public void PublishDynamicContent(IDynamicContentUpdatedEvent eventInfo)
        {
            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            try
            {
                switch (eventInfo.Item.GetType().Name)
                {
                    case Constants.JobProfile:
                        if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished &&
                     eventInfo.Item.Status.ToString() == Constants.ItemStatusLive)
                        {
                            GenerateServiceBusMessageForJobProfile(eventInfo);
                        }
                        else if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusUnpublished &&
                    eventInfo.Item.Status.ToString() == Constants.ItemActionDeleted)
                        {
                            // Add delete function here
                        }

                        break;
                    case Constants.Restriction:
                    case Constants.Registration:
                    case Constants.ApprenticeshipRequirement:
                    case Constants.CollegeRequirement:
                    case Constants.UniversityRequirement:
                        if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished &&
                     eventInfo.Item.Status.ToString() == Constants.ItemStatusMaster)
                        {
                            GenerateServiceBusMessageForInfoTypes(eventInfo);
                        }
                        else if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusUnpublished &&
                    eventInfo.Item.Status.ToString() == Constants.ItemActionDeleted)
                        {
                            // Add delete function here
                        }

                        break;

                    case Constants.Uniform:
                    case Constants.Location:
                    case Constants.Environment:
                        if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished &&
                     eventInfo.Item.Status.ToString() == Constants.ItemStatusMaster)
                        {
                            GenerateServiceBusMessageForWYDTypes(eventInfo);
                        }
                        else if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusUnpublished &&
                    eventInfo.Item.Status.ToString() == Constants.ItemActionDeleted)
                        {
                            // Add delete function here
                        }

                        break;

                    case Constants.UniversityLink:
                    case Constants.CollegeLink:
                    case Constants.ApprenticeshipLink:
                        if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished &&
                     eventInfo.Item.Status.ToString() == Constants.ItemStatusMaster)
                        {
                            GenerateServiceBusMessageForTextFieldTypes(eventInfo);
                        }
                        else if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusUnpublished &&
                    eventInfo.Item.Status.ToString() == Constants.ItemActionDeleted)
                        {
                            // Add delete function here
                        }

                        break;

                    case Constants.Skill:
                        if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished &&
                     eventInfo.Item.Status.ToString() == Constants.ItemStatusMaster)
                        {
                            GenerateServiceBusMessageForSkillTypes(eventInfo);
                        }
                        else if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusUnpublished &&
                    eventInfo.Item.Status.ToString() == Constants.ItemActionDeleted)
                        {
                            // Add delete function here
                        }

                        break;

                    case Constants.JobProfileSoc:
                        if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished &&
                     eventInfo.Item.Status.ToString() == Constants.ItemStatusMaster)
                        {
                            GenerateServiceBusMessageForSocCodeType(eventInfo);
                        }
                        else if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusUnpublished &&
                    eventInfo.Item.Status.ToString() == Constants.ItemActionDeleted)
                        {
                            // Add delete function here
                        }

                        break;

                    case Constants.SOCSkillsMatrix:

                        //Get all the parentitem links when the status is Master and then get related data when the status is LIVE
                        if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished &&
                     eventInfo.Item.Status.ToString() == Constants.ItemStatusMaster)
                        {
                            SkillsMatrixParentItems = GetParentItemsForSocSkillsMatrix(eventInfo);
                        }

                        if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished &&
                   eventInfo.Item.Status.ToString() == Constants.ItemStatusLive)
                        {
                            GenerateServiceBusMessageForSocSkillsMatrixType(eventInfo);
                        }
                        else if (eventInfo.Item.ApprovalWorkflowState.Value == Constants.WorkflowStatusUnpublished &&
                    eventInfo.Item.Status.ToString() == Constants.ItemActionDeleted)
                        {
                            // Add delete function here
                        }

                        break;

                    default:
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

            if (eventInfo.ItemType != typeof(PageNode) && eventInfo.ItemType != typeof(FlatTaxon))
            {
                return;
            }

            try
            {
                var microServicesDataEventAction = dataEventActions.GetEventAction(eventInfo);

                var itemId = eventInfo.ItemId;
                var providerName = eventInfo.ProviderName;
                var contentType = eventInfo.ItemType;

                if (eventInfo.ItemType.Name == "FlatTaxon")
                {
                    GetClassificationRelatedItems(eventInfo);
                }

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

        private void GetClassificationRelatedItems(IDataEvent eventInfo)
        {
            DynamicModuleManager manager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            Type dynamicType = TypeResolutionService.ResolveType(ParentType);
            Type jobProfileSocype = TypeResolutionService.ResolveType(JobProfileSocType);

            var taxonomyManager = TaxonomyManager.GetManager();

            var taxon = taxonomyManager.GetTaxa<FlatTaxon>()
                .Where(t => t.Id == eventInfo.ItemId)
                .FirstOrDefault();
            var relatedPropertyName = "jobprofile";
            switch (taxon.FlatTaxonomy.Name)
            {
                case Constants.TaxonApprenticeshipFrameworks:

                    //Sitefinity equivalent
                    relatedPropertyName = "apprenticeshipframeworks";
                    GetIndividualClassificationsForSocCodeData(manager, jobProfileSocype, taxon, relatedPropertyName);
                    break;
                case Constants.TaxonApprenticeshipStandards:

                    //Sitefinity equivalent
                    relatedPropertyName = "apprenticeshipstandards";
                    GetIndividualClassificationsForSocCodeData(manager, jobProfileSocype, taxon, relatedPropertyName);
                    break;
                case Constants.TaxonCollegeEntryRequirements:

                    //Sitefinity equivalent
                    GetIndividualClassifications(manager, dynamicType, taxon, Constants.TaxonCollegeEntryRequirements);
                    break;
                case Constants.TaxonHiddenAlternativeTitle:

                    //Sitefinity equivalent
                    GetIndividualClassifications(manager, dynamicType, taxon, Constants.TaxonHiddenAlternativeTitle);
                    break;
                case Constants.TaxonJobProfileSpecialism:

                    //Sitefinity equivalent
                    GetIndividualClassifications(manager, dynamicType, taxon, Constants.TaxonJobProfileSpecialism);
                    break;
                case Constants.TaxonUniversityEntryRequirements:

                    //Sitefinity equivalent
                    GetIndividualClassifications(manager, dynamicType, taxon, Constants.TaxonUniversityEntryRequirements);
                    break;
                case Constants.TaxonWorkingHoursDetails:

                    //Sitefinity equivalent
                    GetIndividualClassifications(manager, dynamicType, taxon, Constants.TaxonWorkingHoursDetails);
                    break;
                case Constants.TaxonWorkingPattern:

                    //Sitefinity equivalent
                    GetIndividualClassifications(manager, dynamicType, taxon, Constants.TaxonWorkingPattern);
                    break;
                case Constants.TaxonWorkingPatternDetails:

                    //Sitefinity equivalent
                    GetIndividualClassifications(manager, dynamicType, taxon, Constants.TaxonWorkingPatternDetails);
                    break;
                case Constants.TaxonApprenticeshipEntryRequirements:
                    GetIndividualClassifications(manager, dynamicType, taxon, Constants.TaxonApprenticeshipEntryRequirements);
                    break;
                default:
                    break;
            }
        }

        private void GetIndividualClassificationsForSocCodeData(DynamicModuleManager manager, Type dynamicType, FlatTaxon taxon, string relatedPropertyName)
        {
            IOrganizableProvider contentProvider = manager.Provider as IOrganizableProvider;
            int? totalCount = -1;

            var socCodeIds = contentProvider.GetItemsByTaxon(taxon.Id, false, relatedPropertyName, dynamicType, null, null, 0, 0, ref totalCount)
                .Cast<DynamicContent>()
                .Select(p => p.Id)
                .ToList();

            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var classificationData = new List<SOCCodeClassificationItem>();
            var contentLinksManager = ContentLinksManager.GetManager();
            var parentType = TypeResolutionService.ResolveType(ParentType);
            foreach (var socCodeDataId in socCodeIds)
            {
                //Get JobProfile Item
                var relatedSocData = dynamicModuleManager.GetDataItem(dynamicType, socCodeDataId);

                if (relatedSocData.Status.ToString() == Constants.ItemStatusMaster)
                {
                    var socCodeClassificationItem = new SOCCodeClassificationItem
                    {
                        SOCCodeClassificationId = dynamicContentExtensions.GetFieldValue<Guid>(relatedSocData, nameof(SOCCodeClassificationItem.Id)),
                        SOCCode = dynamicContentExtensions.GetFieldValue<Lstring>(relatedSocData, nameof(SOCCodeClassificationItem.SOCCode)),
                        Id = taxon.Id,
                        Title = taxon.Title,
                        Url = taxon.UrlName,
                        Description = taxon.Description
                    };
                    var jobProfileId = contentLinksManager.GetContentLinks()
                .Where(c => c.ParentItemType == ParentType && c.ChildItemId == dynamicContentExtensions.GetFieldValue<Guid>(relatedSocData, nameof(SOCCodeClassificationItem.Id)))
                .Select(c => c.ParentItemId).FirstOrDefault();
                    var jobProfileItem = dynamicModuleManager.GetDataItem(parentType, jobProfileId);
                    socCodeClassificationItem.JobProfileId = jobProfileItem.Id;
                    socCodeClassificationItem.JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(jobProfileItem, nameof(SOCCodeClassificationItem.Title));
                    classificationData.Add(socCodeClassificationItem);
                    serviceBusMessageProcessor.SendOtherRelatedTypeMessages(classificationData, taxon.FlatTaxonomy.Name, taxon.Status.ToString());
                }
            }
        }

        private void GetIndividualClassifications(DynamicModuleManager manager, Type dynamicType, FlatTaxon taxon, string relatedPropertyName)
        {
            IOrganizableProvider contentProvider = manager.Provider as IOrganizableProvider;
            int? totalCount = -1;

            var jobProfileIds = contentProvider.GetItemsByTaxon(taxon.Id, false, relatedPropertyName, dynamicType, null, null, 0, 0, ref totalCount)
                .Cast<DynamicContent>()
                .Select(p => p.Id)
                .ToList();

            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var classificationData = new List<ClassificationItem>();
            foreach (var jobProfileId in jobProfileIds)
            {
                //Get JobProfile Item
                var relatedJobprofile = dynamicModuleManager.GetDataItem(dynamicType, jobProfileId);

                if (relatedJobprofile.Status.ToString() == Constants.ItemStatusLive)
                {
                    classificationData.Add(new ClassificationItem
                    {
                        JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(relatedJobprofile, nameof(ClassificationItem.Id)),
                        JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(relatedJobprofile, nameof(ClassificationItem.Title)),
                        Id = taxon.Id,
                        Title = taxon.Title,
                        Url = taxon.GetDefaultUrl(),
                        Description = taxon.Description
                    });
                    serviceBusMessageProcessor.SendOtherRelatedTypeMessages(classificationData, taxon.FlatTaxonomy.Name, taxon.Status.ToString());
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

        private void ExportPageNode(string providerName, Type contentType, Guid itemId)
        {
            var microServiceEndPointConfigKey = compositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(contentType, itemId, providerName);
            if (!microServiceEndPointConfigKey.IsNullOrEmpty())
            {
                var compositePageData = compositePageBuilder.GetPublishedPage(contentType, itemId, providerName);
                asyncHelper.Synchronise(() => compositeUIService.PostPageDataAsync(microServiceEndPointConfigKey, compositePageData));
            }
        }

        private void GenerateServiceBusMessageForJobProfile(IDynamicContentUpdatedEvent eventInfo)
        {
            JobProfileMessage jobprofileData = dynamicContentConverter.ConvertFrom(eventInfo.Item);
            serviceBusMessageProcessor.SendJobProfileMessage(jobprofileData, eventInfo.Item.GetType().Name, eventInfo.Item.ApprovalWorkflowState.Value);
        }

        private void GenerateServiceBusMessageForInfoTypes(IDynamicContentUpdatedEvent eventInfo)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();
            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == ParentType && c.ChildItemId == eventInfo.Item.Id)
                   .Select(c => c.ParentItemId).ToList();

            var relatedInfoTypes = GetInfoRelatedItems(eventInfo.Item, parentItemContentLinks, dynamicModuleManager, ParentType);
            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedInfoTypes, eventInfo.Item.GetType().Name, eventInfo.Item.ApprovalWorkflowState.Value);
        }

        private void GenerateServiceBusMessageForSocCodeType(IDynamicContentUpdatedEvent eventInfo)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();
            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == ParentType && c.ChildItemId == eventInfo.Item.Id)
                   .Select(c => c.ParentItemId).ToList();

            var relatedSocContentTypes = GetSocRelatedItems(eventInfo.Item, parentItemContentLinks, dynamicModuleManager, ParentType);
            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedSocContentTypes, eventInfo.Item.GetType().Name, eventInfo.Item.ApprovalWorkflowState.Value);
        }

        private List<Guid> GetParentItemsForSocSkillsMatrix(IDynamicContentUpdatedEvent eventInfo)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();
            return contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == ParentType && c.ChildItemId == eventInfo.Item.Id)
                   .Select(c => c.ParentItemId).ToList();
        }

        private void GenerateServiceBusMessageForSocSkillsMatrixType(IDynamicContentUpdatedEvent eventInfo)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();
            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == ParentType && c.ChildItemId == eventInfo.Item.Id)
                   .Select(c => c.ParentItemId).ToList();

            var relatedSocSkillsMatrixContentTypes = GetSocSkillMatrixRelatedItems(eventInfo.Item, parentItemContentLinks, dynamicModuleManager, ParentType);
            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedSocSkillsMatrixContentTypes, eventInfo.Item.GetType().Name, eventInfo.Item.ApprovalWorkflowState.Value);
        }

        private void GenerateServiceBusMessageForWYDTypes(IDynamicContentUpdatedEvent eventInfo)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();

            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == ParentType && c.ChildItemId == eventInfo.Item.Id)
                   .Select(c => c.ParentItemId).ToList();

            var relatedWYDTypes = GetWYDRelatedItems(eventInfo.Item, parentItemContentLinks, dynamicModuleManager, ParentType);
            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedWYDTypes, eventInfo.Item.GetType().Name, eventInfo.Item.ApprovalWorkflowState.Value);
        }

        private void GenerateServiceBusMessageForTextFieldTypes(IDynamicContentUpdatedEvent eventInfo)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();
            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == ParentType && c.ChildItemId == eventInfo.Item.Id)
                   .Select(c => c.ParentItemId).ToList();

            var relatedTextFieldTypes = GetTextFieldRelatedItems(eventInfo.Item, parentItemContentLinks, dynamicModuleManager, ParentType);
            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedTextFieldTypes, eventInfo.Item.GetType().Name, eventInfo.Item.ApprovalWorkflowState.Value);
        }

        private void GenerateServiceBusMessageForSkillTypes(IDynamicContentUpdatedEvent eventInfo)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();
            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == SocSkillsMatrixType && c.ChildItemId == eventInfo.Item.Id)
                   .Select(c => c.ParentItemId).ToList();

            var relatedSkillTypes = GetRelatedSkillTypeItems(eventInfo.Item, parentItemContentLinks, dynamicModuleManager, SocSkillsMatrixType);
            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedSkillTypes, eventInfo.Item.GetType().Name, eventInfo.Item.ApprovalWorkflowState.Value);
        }

        private IEnumerable<SocCodeContentItem> GetSocRelatedItems(DynamicContent childItem, List<Guid> parentItemLinks, DynamicModuleManager dynamicModuleManager, string parentName)
        {
            var relatedSocContentItems = new List<SocCodeContentItem>();
            var parentType = TypeResolutionService.ResolveType(ParentType);
            var apprenticeshipStandardsData = childItem.GetValue<TrackedList<Guid>>(Constants.ApprenticeshipStandards.ToLower());
            var apprenticeshipFrameworkData = childItem.GetValue<TrackedList<Guid>>(Constants.ApprenticeshipFramework.ToLower());

            foreach (var contentId in parentItemLinks)
            {
                var parentItem = dynamicModuleManager.GetDataItem(parentType, contentId);

                relatedSocContentItems.Add(new SocCodeContentItem
                {
                    Id = childItem.Id,
                    Title = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(SocCodeContentItem.SOCCode)),
                    SOCCode = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(SocCodeContentItem.SOCCode)),
                    Description = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(SocCodeContentItem.Description)),
                    UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(SocCodeContentItem.UrlName)),
                    ONetOccupationalCode = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(SocCodeContentItem.ONetOccupationalCode)),
                    ApprenticeshipFramework = MapClassificationData(apprenticeshipFrameworkData),
                    ApprenticeshipStandards = MapClassificationData(apprenticeshipStandardsData),
                    JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(parentItem, nameof(SocCodeContentItem.Id)),
                    JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(parentItem, nameof(SocCodeContentItem.Title))
                });
            }

            return relatedSocContentItems;
        }

        private FrameworkSkillItem GetRelatedSkillsData(DynamicContent content, string relatedField)
        {
            var relatedSkillsData = new FrameworkSkillItem();
            content.ProviderName = string.Empty;
            var relatedItem = dynamicContentExtensions.GetRelatedItems(content, relatedField).FirstOrDefault();

            return new FrameworkSkillItem
            {
                Id = dynamicContentExtensions.GetFieldValue<Guid>(relatedItem, nameof(FrameworkSkillItem.Id)),
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(FrameworkSkillItem.Title)),
                Description = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(FrameworkSkillItem.Description)),
                ONetElementId = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(FrameworkSkillItem.ONetElementId))
            };
        }

        private RelatedSocCodeItem GenerateSocData(DynamicContent content)
        {
            var socCodes = new RelatedSocCodeItem
            {
                Id = content.Id,
                SOCCode = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocCode.SOCCode))
            };

            return socCodes;
        }

        private IEnumerable<RelatedSocCodeItem> GetRelatedSocsData(DynamicContent content, string relatedField)
        {
            var relatedSocsData = new List<RelatedSocCodeItem>();
            var relatedItems = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    relatedSocsData.Add(GenerateSocData(relatedItem));
                }
            }

            return relatedSocsData;
        }

        private IEnumerable<SocSkillMatrixContentItem> GetSocSkillMatrixRelatedItems(DynamicContent childItem, List<Guid> parentItemLinks, DynamicModuleManager dynamicModuleManager, string parentName)
        {
            var relatedSocSkillMatrixContentItems = new List<SocSkillMatrixContentItem>();
            var parentType = TypeResolutionService.ResolveType(ParentType);

            var socSkillsMatrixContent = new SocSkillMatrixContentItem
            {
                Id = childItem.Id,
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(SocSkillMatrixContentItem.Title)),
                Contextualised = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(SocSkillMatrixContentItem.Contextualised)),
                ONetAttributeType = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(SocSkillMatrixContentItem.ONetAttributeType)),
                ONetRank = dynamicContentExtensions.GetFieldValue<decimal>(childItem, nameof(SocSkillMatrixContentItem.ONetRank)),
                Rank = dynamicContentExtensions.GetFieldValue<decimal>(childItem, nameof(SocSkillMatrixContentItem.Rank)),
                RelatedSkill = GetRelatedSkillsData(childItem, nameof(SocSkillMatrixContentItem.RelatedSkill)),
                RelatedSOC = GetRelatedSocsData(childItem, nameof(SocSkillMatrixContentItem.RelatedSOC))
            };

            foreach (var contentId in SkillsMatrixParentItems)
            {
                var parentItem = dynamicModuleManager.GetDataItem(parentType, contentId);
                relatedSocSkillMatrixContentItems.Add(new SocSkillMatrixContentItem
                {
                    Id = childItem.Id,
                    Title = socSkillsMatrixContent.Title,
                    Contextualised = socSkillsMatrixContent.Title,
                    ONetAttributeType = socSkillsMatrixContent.ONetAttributeType,
                    ONetRank = socSkillsMatrixContent.ONetRank,
                    Rank = socSkillsMatrixContent.Rank,
                    RelatedSkill = socSkillsMatrixContent.RelatedSkill,
                    RelatedSOC = socSkillsMatrixContent.RelatedSOC,
                    JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(parentItem, nameof(SocCodeContentItem.Id)),
                    JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(parentItem, nameof(SocCodeContentItem.Title))
                });
            }

            return relatedSocSkillMatrixContentItems;
        }

        private IEnumerable<Classification> MapClassificationData(TrackedList<Guid> classifications)
        {
            var classificationData = new List<Classification>();
            TaxonomyManager taxonomyManager = TaxonomyManager.GetManager();
            foreach (var cat in classifications)
            {
                classificationData.Add(new Classification
                {
                    Id = taxonomyManager.GetTaxon(cat).Id,
                    Title = taxonomyManager.GetTaxon(cat).Title,
                    Url = taxonomyManager.GetTaxon(cat).UrlName
                });
            }

            return classificationData;
        }

        private IEnumerable<WYDContentItem> GetWYDRelatedItems(DynamicContent childItem, List<Guid> parentItemLinks, DynamicModuleManager dynamicModuleManager, string parentName)
        {
            var relatedContentItems = new List<WYDContentItem>();
            var parentType = TypeResolutionService.ResolveType(ParentType);
            foreach (var contentId in parentItemLinks)
            {
                var parentItem = dynamicModuleManager.GetDataItem(parentType, contentId);

                relatedContentItems.Add(new WYDContentItem
                {
                    JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(parentItem, nameof(WYDContentItem.Id)),
                    JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(parentItem, nameof(WYDContentItem.Title)),
                    Id = dynamicContentExtensions.GetFieldValue<Guid>(childItem, nameof(WYDContentItem.Id)),
                    Title = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(WYDContentItem.Title)),
                    Description = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(WYDContentItem.Description)),
                    IsNegative = dynamicContentExtensions.GetFieldValue<bool>(childItem, nameof(WYDContentItem.IsNegative))
                });
            }

            return relatedContentItems;
        }

        private IEnumerable<InfoContentItem> GetInfoRelatedItems(DynamicContent childItem, List<Guid> parentItemLinks, DynamicModuleManager dynamicModuleManager, string parentName)
        {
            var relatedContentItems = new List<InfoContentItem>();
            var parentType = TypeResolutionService.ResolveType(ParentType);
            foreach (var contentId in parentItemLinks)
            {
                var parentItem = dynamicModuleManager.GetDataItem(parentType, contentId);

                relatedContentItems.Add(new InfoContentItem
                {
                    JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(parentItem, nameof(InfoContentItem.Id)),
                    JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(parentItem, nameof(InfoContentItem.Title)),
                    Id = dynamicContentExtensions.GetFieldValue<Guid>(childItem, nameof(InfoContentItem.Id)),
                    Title = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(InfoContentItem.Title)),
                    Info = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(InfoContentItem.Info))
                });
            }

            return relatedContentItems;
        }

        private IEnumerable<TextFieldContentItem> GetTextFieldRelatedItems(DynamicContent childItem, List<Guid> parentItemLinks, DynamicModuleManager dynamicModuleManager, string parentName)
        {
            var relatedContentItems = new List<TextFieldContentItem>();
            var parentType = TypeResolutionService.ResolveType(ParentType);
            foreach (var contentId in parentItemLinks)
            {
                var parentItem = dynamicModuleManager.GetDataItem(parentType, contentId);

                relatedContentItems.Add(new TextFieldContentItem
                {
                    JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(parentItem, nameof(TextFieldContentItem.Id)),
                    JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(parentItem, nameof(TextFieldContentItem.Title)),
                    Id = dynamicContentExtensions.GetFieldValue<Guid>(childItem, nameof(TextFieldContentItem.Id)),
                    Title = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(TextFieldContentItem.Title)),
                    Url = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(TextFieldContentItem.Url)),
                    Text = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(TextFieldContentItem.Text))
                });
            }

            return relatedContentItems;
        }

        private IEnumerable<SkillContentItem> GetRelatedSkillTypeItems(DynamicContent childItem, List<Guid> parentItemLinks, DynamicModuleManager dynamicModuleManager, string parentName)
        {
            //When you update a skill get all the socskill matrixes that have this skill
            var relatedContentItems = new List<SkillContentItem>();
            var socSkillsMatrixType = TypeResolutionService.ResolveType(SocSkillsMatrixType);
            var parentType = TypeResolutionService.ResolveType(ParentType);
            dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();
            foreach (var contentId in parentItemLinks)
            {
                var parentItem = dynamicModuleManager.GetDataItem(socSkillsMatrixType, contentId);

                var jobProfileId = contentLinksManager.GetContentLinks()
                 .Where(c => c.ParentItemType == ParentType && c.ChildItemId == parentItem.Id)
                 .Select(c => c.ParentItemId).FirstOrDefault();

                var jobProfileItem = dynamicModuleManager.GetDataItem(parentType, jobProfileId);

                relatedContentItems.Add(new SkillContentItem
                {
                    JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(jobProfileItem, nameof(SkillContentItem.Id)),
                    JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(jobProfileItem, nameof(SkillContentItem.Title)),
                    Id = dynamicContentExtensions.GetFieldValue<Guid>(childItem, nameof(SkillContentItem.Id)),
                    Title = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(SkillContentItem.Title)),
                    ONetElementId = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(SkillContentItem.ONetElementId)),
                    SocSkillMatrixId = dynamicContentExtensions.GetFieldValue<Guid>(parentItem, nameof(SkillContentItem.Id)),
                    SocSkillMatrixTitle = dynamicContentExtensions.GetFieldValue<Lstring>(parentItem, nameof(SkillContentItem.Title)),
                    Description = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(SkillContentItem.Description))
                });
            }

            return relatedContentItems;
        }
    }
}
