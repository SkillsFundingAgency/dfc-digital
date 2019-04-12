using System;

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

        public const string ServiceStatusFailedCheckLogsMessage =
            "Service status check failed, check logs with activity id";

        public const int DefaultMaxRelatedItems = 100;
        public const string UrlNameRegexPattern = @"[^\w\-\!\$\'\(\)\=\@\d_]+";
        public const string Technology = "Technology";
        public const string SearchScoringProfileName = "jp";
        public const string SocField = "SOC";
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
    }
}