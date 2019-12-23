using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using DFC.Digital.Web.Sitefinity.Core.Interfaces;
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
        private readonly IDynamicContentAction dynamicContentAction;
        private readonly ISitefinityManagerProxy sitefinityManagerProxy;

        public DataEventProcessor(IApplicationLogger applicationLogger, ICompositePageBuilder compositePageBuilder, IMicroServicesPublishingService compositeUIService, IAsyncHelper asyncHelper, IDataEventActions dataEventActions, IDynamicModuleConverter<JobProfileMessage> dynamicContentConverter, IServiceBusMessageProcessor serviceBusMessageProcessor, IDynamicContentExtensions dynamicContentExtensions, IDynamicContentAction dynamicContentAction, ISitefinityManagerProxy sitefinityManagerProxy)
        {
            this.applicationLogger = applicationLogger;
            this.compositePageBuilder = compositePageBuilder;
            this.compositeUIService = compositeUIService;
            this.asyncHelper = asyncHelper;
            this.dataEventActions = dataEventActions;
            this.dynamicContentConverter = dynamicContentConverter;
            this.serviceBusMessageProcessor = serviceBusMessageProcessor;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.dynamicContentAction = dynamicContentAction;
            this.sitefinityManagerProxy = sitefinityManagerProxy;
        }

        public List<Guid> SkillsMatrixParentItems { get; set; }

        public void PublishDynamicContent(DynamicContent item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var eventAction = dynamicContentAction.GetDynamicContentEventAction(item);

            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"\logs\logger.txt", true))
            //{
            //    //file.WriteLine($"\n| Dynamic Content Action Type - | {eventActionType.PadRight(10, ' ')}");
            //    file.WriteLine($" |Item Type -- {item.GetType().Name.PadRight(10, ' ')} |Item ID -- {item.Id.ToString().PadRight(15, ' ')} | Item ApprovalWorkflowState -- {item.ApprovalWorkflowState.Value.PadRight(15, ' ')} | Item Status -- {item.Status.ToString().PadRight(10, ' ')} | Item Triggered action - {eventActionType.PadRight(10, ' ')} |");
            //    file.WriteLine($"*********************************************************************************************************************************************************\n");
            //}
            applicationLogger.Trace($"Got event - |{item.GetType().Name.PadRight(15, ' ')} -- {item.Id.ToString().PadRight(15, ' ')} |{item.ApprovalWorkflowState.Value.PadRight(15, ' ')} | {item.Status.ToString().PadRight(15, ' ')} | Derived action - {eventAction.ToString().PadRight(15, ' ')}");

            try
            {
                if (eventAction == MessageAction.Ignored)
                {
                    return;
                }

                switch (item.GetType().Name)
                {
                    case Constants.JobProfile:
                        if (eventAction == MessageAction.Published)
                        {
                            GenerateServiceBusMessageForJobProfile(item, eventAction);
                        }
                        else
                        {
                            GenerateServiceBusMessageForJobProfileUnPublish(item, eventAction);
                        }

                        break;

                    case Constants.Restriction:
                    case Constants.Registration:
                    case Constants.ApprenticeshipRequirement:
                    case Constants.CollegeRequirement:
                    case Constants.UniversityRequirement:
                        GenerateServiceBusMessageForInfoTypes(item, eventAction);

                        break;

                    case Constants.Uniform:
                    case Constants.Location:
                    case Constants.Environment:
                        GenerateServiceBusMessageForWYDTypes(item, eventAction);

                        break;

                    case Constants.UniversityLink:
                    case Constants.CollegeLink:
                    case Constants.ApprenticeshipLink:
                        GenerateServiceBusMessageForTextFieldTypes(item, eventAction);

                        break;

                    case Constants.Skill:
                        GenerateServiceBusMessageForSkillTypes(item, eventAction);

                        break;

                    case Constants.JobProfileSoc:
                        GenerateServiceBusMessageForSocCodeType(item, eventAction);

                        break;

                    case Constants.SOCSkillsMatrix:

                        //For all the Dynamic content types we are using Jobprofile as Parent Type
                        //and for only Skills we are using SocSkillsMatrix Type as the Parent Type
                        var liveVersionItem = item;

                        if (item.Status.ToString() != Constants.ItemStatusLive)
                        {
                            var liveItem = dynamicModuleManager.Lifecycle.GetLive(item);
                            liveVersionItem = dynamicModuleManager.GetDataItem(item.GetType(), liveItem.Id);
                        }
                        else
                        {
                            var masterItem = dynamicModuleManager.Lifecycle.GetMaster(item);
                            item = dynamicModuleManager.GetDataItem(item.GetType(), masterItem.Id);
                        }

                        SkillsMatrixParentItems = GetParentItemsForSocSkillsMatrix(item);
                        GenerateServiceBusMessageForSocSkillsMatrixType(liveVersionItem, item, eventAction);

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                applicationLogger.ErrorJustLogIt($"Failed to export data for item id {item.Id}", ex);
                throw;
            }
        }

        public void ExportContentData(IDataEvent eventInfo)
        {
            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            if (eventInfo.ItemType == typeof(PageNode))
            {
                ExportCompositePage(eventInfo);
            }
            else if (eventInfo.ItemType == typeof(FlatTaxon))
            {
                //Don't do this for deletes as the TaxonItem will not exist by now, linked JPs will fire updated events.
                if (eventInfo.Action != Constants.ItemActionDeleted)
                {
                    GetClassificationRelatedItems(eventInfo);
                }
            }
        }

        public void ExportCompositePage(IDataEvent eventInfo)
        {
            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            try
            {
                var microServicesDataEventAction = dataEventActions.GetEventAction(eventInfo);

                var itemId = eventInfo.ItemId;
                var providerName = eventInfo.ProviderName;
                var contentType = eventInfo.ItemType;
                var isContentPage = compositePageBuilder.GetContentPageTypeFromPageNode(contentType, itemId, providerName);

                if (isContentPage && microServicesDataEventAction == MicroServicesDataEventAction.PublishedOrUpdated)
                {
                    ExportPageNode(providerName, contentType, itemId, Constants.WorkflowStatusPublished);
                }
                else if (isContentPage && microServicesDataEventAction == MicroServicesDataEventAction.UnpublishedOrDeleted)
                {
                    ExportPageNode(providerName, contentType, itemId, Constants.ItemActionDeleted);
                }
                else if (isContentPage && microServicesDataEventAction == MicroServicesDataEventAction.Draft)
                {
                    ExportPageNode(providerName, contentType, itemId, Constants.WorkflowStatusDraft);
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
            switch (taxon.FlatTaxonomy.Name.Trim())
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
                }
            }

            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(classificationData, taxon.FlatTaxonomy.Name.Trim(), GetActionType(taxon.Status.ToString()));
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

                if (relatedJobprofile.Status.ToString() == Constants.ItemStatusMaster)
                {
                    classificationData.Add(new ClassificationItem
                    {
                        JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(relatedJobprofile, nameof(ClassificationItem.Id)),
                        JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(relatedJobprofile, nameof(ClassificationItem.Title)),
                        Id = taxon.Id,
                        Title = taxon.Title,
                        Url = taxon.UrlName,
                        Description = taxon.Description
                    });
                }
            }

            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(classificationData, taxon.FlatTaxonomy.Name.Trim(), GetActionType(taxon.Status.ToString()));
        }

        private void DeletePage(string providerName, Type contentType, Guid itemId)
        {
            var microServiceEndPointConfigKey = compositePageBuilder.GetMicroServiceEndPointConfigKeyForPageNode(contentType, itemId, providerName);
            if (!microServiceEndPointConfigKey.IsNullOrEmpty())
            {
                asyncHelper.Synchronise(() => compositeUIService.DeletePageAsync(microServiceEndPointConfigKey, itemId));
            }
        }

        private void ExportPageNode(string providerName, Type contentType, Guid itemId, string eventAction)
        {
            var compositePageData = compositePageBuilder.GetPublishedPage(contentType, itemId, providerName);
            GenerateServiceBusMessageForContentPages(compositePageData, Constants.Pages, eventAction);
        }

        private void GenerateServiceBusMessageForContentPages(MicroServicesPublishingPageData item, string contentType, string eventAction)
        {
            serviceBusMessageProcessor.SendContentPageMessage(item, contentType, eventAction);
        }

        private void GenerateServiceBusMessageForJobProfile(DynamicContent item, MessageAction eventAction)
        {
            JobProfileMessage jobprofileData = dynamicContentConverter.ConvertFrom(item);
            serviceBusMessageProcessor.SendJobProfileMessage(jobprofileData, item.GetType().Name, eventAction.ToString());
        }

        private void GenerateServiceBusMessageForJobProfileUnPublish(DynamicContent item, MessageAction eventAction)
        {
            var jobProfileMessage = new JobProfileMessage();
            jobProfileMessage.JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(item, "Id");
            jobProfileMessage.Title = dynamicContentExtensions.GetFieldValue<Lstring>(item, nameof(JobProfileMessage.Title));
            serviceBusMessageProcessor.SendJobProfileMessage(jobProfileMessage, item.GetType().Name, eventAction.ToString());
        }

        private void GenerateServiceBusMessageForInfoTypes(DynamicContent item, MessageAction eventAction)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();
            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == ParentType && c.ChildItemId == item.Id)
                   .Select(c => c.ParentItemId).ToList();

            var relatedInfoTypes = GetInfoRelatedItems(item, parentItemContentLinks, dynamicModuleManager, ParentType);
            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedInfoTypes, item.GetType().Name, eventAction.ToString());
        }

        private void GenerateServiceBusMessageForSocCodeType(DynamicContent item, MessageAction eventAction)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();
            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == ParentType && c.ChildItemId == item.Id)
                   .Select(c => c.ParentItemId).ToList();

            var relatedSocContentTypes = GetSocRelatedItems(item, parentItemContentLinks, dynamicModuleManager, ParentType);
            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedSocContentTypes, item.GetType().Name, eventAction.ToString());
        }

        private List<Guid> GetParentItemsForSocSkillsMatrix(DynamicContent item)
        {
            var contentLinksManager = ContentLinksManager.GetManager();
            return contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == ParentType && c.ChildItemId == item.Id)
                   .Select(c => c.ParentItemId).ToList();
        }

        private void GenerateServiceBusMessageForWYDTypes(DynamicContent item, MessageAction eventAction)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();

            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == ParentType && c.ChildItemId == item.Id)
                   .Select(c => c.ParentItemId).ToList();

            var relatedWYDTypes = GetWYDRelatedItems(item, parentItemContentLinks, dynamicModuleManager, ParentType);
            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedWYDTypes, item.GetType().Name, eventAction.ToString());
        }

        private void GenerateServiceBusMessageForTextFieldTypes(DynamicContent item, MessageAction eventAction)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();
            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == ParentType && c.ChildItemId == item.Id)
                   .Select(c => c.ParentItemId).ToList();

            var relatedTextFieldTypes = GetTextFieldRelatedItems(item, parentItemContentLinks, dynamicModuleManager, ParentType);

            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedTextFieldTypes, item.GetType().Name, eventAction.ToString());
        }

        private void GenerateServiceBusMessageForSkillTypes(DynamicContent item, MessageAction eventAction)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();
            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == SocSkillsMatrixType && c.ChildItemId == item.Id)
                   .Select(c => c.ParentItemId).ToList();

            var relatedSkillTypes = GetRelatedSkillTypeItems(item, parentItemContentLinks, dynamicModuleManager, SocSkillsMatrixType);
            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedSkillTypes, item.GetType().Name, eventAction.ToString());
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
                if (parentItem.ApprovalWorkflowState == Constants.WorkflowStatusPublished && !parentItem.IsDeleted)
                {
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
            }

            return relatedSocContentItems;
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

        private void GenerateServiceBusMessageForSocSkillsMatrixType(DynamicContent liveItem, DynamicContent masterItem, MessageAction eventAction)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var contentLinksManager = ContentLinksManager.GetManager();
            var parentItemContentLinks = contentLinksManager.GetContentLinks()
                   .Where(c => c.ParentItemType == ParentType && c.ChildItemId == masterItem.Id)
                   .Select(c => c.ParentItemId).ToList();

            var relatedSocSkillsMatrixContentTypes = GetSocSkillMatrixRelatedItems(liveItem, parentItemContentLinks, dynamicModuleManager, ParentType);
            serviceBusMessageProcessor.SendOtherRelatedTypeMessages(relatedSocSkillsMatrixContentTypes, liveItem.GetType().Name, eventAction.ToString());
        }

        private IEnumerable<SocSkillMatrixContentItem> GetSocSkillMatrixRelatedItems(DynamicContent childLiveItem, List<Guid> parentItemLinks, DynamicModuleManager dynamicModuleManager, string parentName)
        {
            var relatedSocSkillMatrixContentItems = new List<SocSkillMatrixContentItem>();
            var parentType = TypeResolutionService.ResolveType(ParentType);

            var socSkillsMatrixContent = new SocSkillMatrixContentItem
            {
                Id = dynamicContentExtensions.GetFieldValue<Guid>(childLiveItem, Constants.OriginalContentId),
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(childLiveItem, nameof(SocSkillMatrixContentItem.Title)),
                Contextualised = dynamicContentExtensions.GetFieldValue<Lstring>(childLiveItem, nameof(SocSkillMatrixContentItem.Contextualised)),
                ONetAttributeType = dynamicContentExtensions.GetFieldValue<Lstring>(childLiveItem, nameof(SocSkillMatrixContentItem.ONetAttributeType)),
                ONetRank = dynamicContentExtensions.GetFieldValue<decimal?>(childLiveItem, nameof(SocSkillMatrixContentItem.ONetRank)).GetValueOrDefault(0),
                Rank = dynamicContentExtensions.GetFieldValue<decimal?>(childLiveItem, nameof(SocSkillMatrixContentItem.Rank)).GetValueOrDefault(0),
                RelatedSkill = GetRelatedSkillsData(childLiveItem, nameof(SocSkillMatrixContentItem.RelatedSkill)),
                RelatedSOC = GetRelatedSocsData(childLiveItem, nameof(SocSkillMatrixContentItem.RelatedSOC))
            };

            if (SkillsMatrixParentItems != null)
            {
                foreach (var contentId in SkillsMatrixParentItems)
                {
                    var parentItem = dynamicModuleManager.GetDataItem(parentType, contentId);
                    if (parentItem.ApprovalWorkflowState == Constants.WorkflowStatusPublished && !parentItem.IsDeleted)
                    {
                        relatedSocSkillMatrixContentItems.Add(new SocSkillMatrixContentItem
                        {
                            Id = dynamicContentExtensions.GetFieldValue<Guid>(childLiveItem, Constants.OriginalContentId),
                            Title = socSkillsMatrixContent.Title,
                            Contextualised = socSkillsMatrixContent.Contextualised,
                            ONetAttributeType = socSkillsMatrixContent.ONetAttributeType,
                            ONetRank = socSkillsMatrixContent.ONetRank,
                            Rank = socSkillsMatrixContent.Rank,
                            RelatedSkill = socSkillsMatrixContent.RelatedSkill,
                            RelatedSOC = socSkillsMatrixContent.RelatedSOC,
                            JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(parentItem, nameof(SocCodeContentItem.Id)),
                            JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(parentItem, nameof(SocCodeContentItem.Title))
                        });
                    }
                }
            }

            return relatedSocSkillMatrixContentItems;
        }

        private FrameworkSkillItem GetRelatedSkillsData(DynamicContent content, string relatedField)
        {
            var relatedSkillsData = new FrameworkSkillItem();
            content.ProviderName = string.Empty;
            var relatedItem = dynamicContentExtensions.GetRelatedItems(content, relatedField).FirstOrDefault();

            return new FrameworkSkillItem
            {
                Id = dynamicContentExtensions.GetFieldValue<Guid>(relatedItem, Constants.OriginalContentId),
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(FrameworkSkillItem.Title)),
                Description = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(FrameworkSkillItem.Description)),
                ONetElementId = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(FrameworkSkillItem.ONetElementId))
            };
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
                if (parentItem.ApprovalWorkflowState == Constants.WorkflowStatusPublished && !parentItem.IsDeleted)
                {
                    relatedContentItems.Add(new WYDContentItem
                    {
                        JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(parentItem, nameof(WYDContentItem.Id)),
                        JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(parentItem, nameof(WYDContentItem.Title)),
                        Id = dynamicContentExtensions.GetFieldValue<Guid>(childItem, nameof(WYDContentItem.Id)),
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(WYDContentItem.Title)),
                        Description = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(WYDContentItem.Description)),
                        IsNegative = dynamicContentExtensions.GetFieldValue<bool>(childItem, nameof(WYDContentItem.IsNegative)),
                        Url = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, Constants.Url)
                    });
                }
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
                if (parentItem.ApprovalWorkflowState == Constants.WorkflowStatusPublished && !parentItem.IsDeleted)
                {
                    relatedContentItems.Add(new InfoContentItem
                    {
                        JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(parentItem, nameof(InfoContentItem.Id)),
                        JobProfileTitle = dynamicContentExtensions.GetFieldValue<Lstring>(parentItem, nameof(InfoContentItem.Title)),
                        Id = dynamicContentExtensions.GetFieldValue<Guid>(childItem, nameof(InfoContentItem.Id)),
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(InfoContentItem.Title)),
                        Info = dynamicContentExtensions.GetFieldValue<Lstring>(childItem, nameof(InfoContentItem.Info))
                    });
                }
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
                if (parentItem.ApprovalWorkflowState == Constants.WorkflowStatusPublished && !parentItem.IsDeleted)
                {
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
                if (parentItem.ApprovalWorkflowState == Constants.WorkflowStatusPublished && !parentItem.IsDeleted)
                {
                    var jobProfileId = contentLinksManager.GetContentLinks()
                 .Where(c => c.ParentItemType == ParentType && c.ChildItemId == parentItem.Id)
                 .Select(c => c.ParentItemId).FirstOrDefault();
                    if (jobProfileId != Guid.Empty)
                    {
                        var jobProfileItem = dynamicModuleManager.GetDataItem(parentType, jobProfileId);
                        if (jobProfileItem.ApprovalWorkflowState == Constants.WorkflowStatusPublished && !jobProfileItem.IsDeleted)
                        {
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
                    }
                }
            }

            return relatedContentItems;
        }

        private string GetActionType(string status)
        {
            if (status == "Published" || status == "Active")
            {
                return nameof(MessageAction.Published);
            }
            else if (status == "UnPublished" || status == "Deleted")
            {
                return nameof(MessageAction.Deleted);
            }
            else
            {
                return status == "Draft" ? nameof(MessageAction.Draft) : null;
            }
        }
    }
}
