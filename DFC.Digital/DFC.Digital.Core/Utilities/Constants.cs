namespace DFC.Digital.Core
{
    //Reason to exlude is because these are constants not code with logic
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Constants
    {
        public const string KeysSearchServiceName = "DFC.Digital.SearchServiceName";
        public const string AssetCacheExpiryTimeMins = "DFC.Digital.AssetCacheExpiryTimeMins";
        public const string GovUkNotifyApiKey = "DFC.Digital.GovUkNotifyAPIKey";
        public const string GovUkNotifyTemplateId = "DFC.Digital.GovUkNotifyTemplateId";
        public const string ContentTypeKey = "ContentType";
        public const string ProviderKey = "ProviderName";
        public const string DefaultSuggesterName = "sg";
        public const string CourseSearchApiKey = "DFC.Digital.CourseSearchApiKey";
        public const string SendGridApiKey = "DFC.Digital.SendGridApiKey";
        public const string CourseSearchPageSize = "DFC.Digital.CourseSearchPageSize";
        public const string CourseSearchAttendanceModes = "DFC.Digital.CourseSearchAttendanceModes";
        public const string CourseSearchEndpointConfigName = "CourseSearch";
        public const string BingSpellcheckApiKey = "DFC.Cognitive.BingSpellCheck.ApiKey";
        public const string BingSpellcheckRequestEndPoint = "DFC.Cognitive.BingSpellCheck.RequestEndPoint";
        public const string OcpApimSubscriptionKey = "Ocp-Apim-Subscription-Key";
        public const string ValidBAUSearchCharacters = @"[^a-zA-Z0-9& \(\)\+:'’,\./]";
        public const string VocPersonalisationCookieName = "vocPersonalisation";
        public const string VocPersonalisationCookieContent = "LastVisitedJobProfile";
        public const string LastVisitedJobProfileKey = "jpprofile";
        public const string GoogleAnalyticsCookie = "_ga";
        public const string GoogleClientIdKey = "clientid";
        public const string Unknown = "Unknown";
        public const string AsheAccessKey = "DFC.Digital.AsheFeedAccessKey";
        public const string AsheEstimateMDApiGateway = "DFC.Digital.AsheEstimateMdApiGateway";
        public const decimal Multiplier = 52;
        public const string Ashe = "LMI-ASHE";
        public const string ServiceStatusFailedLogMessage = "Service status check failed for activity id";
        public const string ServiceStatusWarnLogMessage = "Service status check is degraded for activity id";

        public const int DefaultMaxRelatedItems = 100;
        public const string UrlNameRegexPattern = @"[^\w\-\!\$\'\(\)\=\@\d_]+";
        public const string Technology = "Technology";
        public const string SearchScoringProfileName = "jp";
        public const string SocField = "SOC";
        public const string ApprenticeshipFramework = "ApprenticeshipFrameworks";
        public const string ApprenticeshipStandards = "ApprenticeshipStandards";
        public const string WorkingPattern = "WorkingPattern";
        public const string WorkingPatternDetail = "WorkingPatternDetails";
        public const string WorkingHoursDetail = "WorkingHoursDetails";
        public const string HiddenAlternativeTitle = "HiddenAlternativeTitle";
        public const string JobProfileSpecialism = "JobProfileSpecialism";

        public const string RelatedLocations = "RelatedLocations";
        public const string RelatedEnvironments = "RelatedEnvironments";
        public const string RelatedUniforms = "RelatedUniforms";

        public const string RelatedCareerProfiles = "RelatedCareerProfiles";

        public const string RelatedSkills = "RelatedSkills";
        public const string Url = "ItemDefaultUrl";

        public const string JobProfileSearchIndex = "DFC.Digital.JobProfileSearchIndex";
        public const string SearchServiceQueryAPIKey = "DFC.Digital.SearchServiceQueryAPIKey";
        public const string SearchServiceAdminAPIKey = "DFC.Digital.SearchServiceAdminAPIKey";
        public const string BackEndDateTimeFormat = "dd MMM yyyy HH:mm:ss";
        public const string CDNLocation = "DFC.Digital.CDNLocation";
        public const string SimulationSuccessEmailAddress = "DFC.Digital.SimulationSuccessEmailAddress";
        public const string SimulationFailureEmailAddress = "DFC.Digital.SimulationFailureEmailAddress";
        public const string CosmosDbName = "DFC.Digital.CosmosDb.Name";
        public const string EmailDocumentCollection = "DFC.Digital.EmailAudit.Collection";
        public const string CourseSearchDocumentCollection = "DFC.Digital.CourseSearchAudit.Collection";
        public const string CosmosDbEndPointPrimaryKey = "DFC.Digital.CosmosAudit.PrimaryKey";
        public const string CosmosDbEndPoint = "DFC.Digital.CosmosAudit.EndpointUrl";
        public const string SendGridDefaultNumberOfRetries = "DFC.Digital.SendGridDefaultNumberOfRetries";
        public const string SendGridDefaultMinimumBackOff = "DFC.Digital.SendGridDefaultMinimumBackOff";
        public const string SendGridDeltaBackOff = "DFC.Digital.SendGridDeltaBackOff";
        public const string SendGridDefaultMaximumBackOff = "DFC.Digital.SendGridDefaultMaximumBackOff";

        public const string CUIShellHealthEndPoint = "DFC.Digital.CUIShell.HealthEndPoint";
        public const string CUIAppJobProfilesHealthEndPoint = "DFC.Digital.CUIAppJobProfiles.HealthEndPoint";
        public const string CUIAppContentPagesHealthEndPoint = "DFC.Digital.CUIAppContentPages.HealthEndPoint";

        public const string CourseSearchInvalidCharactersRegexPattern = "[^a-zA-Z0-9& ()+:'’,.]*";
        public const string CourseApiDateFormat = "yyyy-MM-dd";
        public const string CourseSearchFrontEndStartDateFormat = "dd MMMM yyyy";
        public const string CourseSearchQueryStringStartDateFormat = "yyyy-MM-dd";
        public const string CourseSearchLocationRegularExpression = @"^([bB][fF][pP][oO]\s{0,1}[0-9]{1,4}|[gG][iI][rR]\s{0,1}0[aA][aA]|[a-pr-uwyzA-PR-UWYZ]([0-9]{1,2}|([a-hk-yA-HK-Y][0-9]|[a-hk-yA-HK-Y][0-9]([0-9]|[abehmnprv-yABEHMNPRV-Y]))|[0-9][a-hjkps-uwA-HJKPS-UW])\s{0,1}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2})$";

        public const string WebChatEndPoint = "DFC.Digital.WebChat.EndPoint";

        //Sitefinity page properties
        public const string Pages = "Pages";

        public const string AlertPages = "Alerts";
        public const string AlertPagesTitle = "Error";

        //Sitefinity event properties
        public const string DynamicProvider = "dynamicProvider2";

        public const string HasPageDataChanged = "HasPageDataChanged";
        public const string ApprovalWorkflowState = "ApprovalWorkflowState";
        public const string ItemStatus = "Status";
        public const string ItemActionUpdated = "Updated";
        public const string ItemActionDeleted = "Deleted";
        public const string WorkflowStatusPublished = "Published";
        public const string WorkflowStatusDraft = "Draft";
        public const string WorkflowStatusUnpublished = "Unpublished";
        public const string ItemStatusLive = "Live";
        public const string ItemStatusTemp = "Temp";
        public const string ItemStatusMaster = "Master";
        public const string ChangedProperties = "ChangedProperties";
        public const string RecycleBinAction = "RecycleBinAction";
        public const string ContentBlock = "Content block";
        public const string Content = "Content";
        public const string SharedContent = "SharedContentID";
        public const string JobProfile = "JobProfile";

        //Fields with Info as field
        public const string Restriction = "Restriction";

        public const string Registration = "Registration";
        public const string ApprenticeshipRequirement = "ApprenticeshipRequirement";
        public const string CollegeRequirement = "CollegeRequirement";
        public const string UniversityRequirement = "UniversityRequirement";

        //Fields with Text as field
        public const string UniversityLink = "UniversityLink";

        public const string CollegeLink = "CollegeLink";
        public const string ApprenticeshipLink = "ApprenticeshipLink";

        //Fields with Description and IsNegative as fields
        public const string Location = "Location";

        public const string Uniform = "Uniform";
        public const string Environment = "Environment";

        //All Soc Code fields
        public const string SOCCode = "SOCCode";

        public const string JobProfileSoc = "JobProfileSoc";

        //Fields with Description and ONetElementId as fields
        public const string Skill = "Skill";

        //All Soc and Skill related fields with ONetAttributeType, ONetRank, Rank, RelatedSkill and RelatedSoc as fields
        public const string SOCSkillsMatrix = "SocSkillsMatrix";

        //Get all classifications
        public const string TaxonApprenticeshipFrameworks = "apprenticeship-frameworks";

        public const string TaxonApprenticeshipStandards = "apprenticeship-standards";

        public const string TaxonHiddenAlternativeTitle = "HiddenAlternativeTitle";
        public const string TaxonJobProfileSpecialism = "JobProfileSpecialism";

        public const string TaxonWorkingHoursDetails = "WorkingHoursDetails";
        public const string TaxonWorkingPattern = "WorkingPattern";
        public const string TaxonWorkingPatternDetails = "WorkingPatternDetails";

        public const string TaxonUniversityEntryRequirements = "UniversityEntryRequirements";
        public const string TaxonCollegeEntryRequirements = "CollegeEntryRequirements";
        public const string TaxonApprenticeshipEntryRequirements = "ApprenticeshipEntryRequirements";

        public const string WYDIntroduction = "WYDIntroduction";
        public const string WYDDayToDayTasks = "WYDDayToDayTasks";

        //Orginal Content Id is used where master version Id is needed
        public const string OriginalContentId = "OriginalContentId";
        public const string ContentId = "Id";

        public const string MicroServiceEndPointConfigKey = "MicroServiceEndPointConfigKey";
        public const string DFCDraftCustomEndpoint = "DFC.Digital.Draft.CustomEndPoint";

        // Check for Content Pages using Page Property
        public const string ShouldIncludeInDFCAppContentPages = "ShouldIncludeInDFCAppContentPages";

        // Area Routing Api settings for Contact Us
        public const string AreaRoutingApiServiceUrl = "DFC.Digital.AreaRoutingApi.ServiceUrl";

        public const string AreaRoutingApiSubscriptionKey = "DFC.Digital.AreaRoutingApi.SubscriptionKey";

        public const string SharedConfigStorageConnectionString = "DFC.SharedConfig.ConfigurationStorageConnectionString";
        public const string SharedConfigCloudStorageTableName = "DFC.SharedConfig.CloudStorageTableName";
        public const string SharedConfigInMemoryCacheTimeToLiveTimespan = "DFC.SharedConfig.InMemoryCacheTimeToLiveTimespan";
        public const string SharedConfigEnvironmentName = "DFC.SharedConfig.EnvironmentName";
    }
}