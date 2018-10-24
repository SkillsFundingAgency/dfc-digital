namespace DFC.Digital.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Constants
    {
        public const string KeysSearchServiceName = "DFC.Digital.SearchServiceName";
        public const string KeysSearchServiceAdminApiKey = "DFC.Digital.SearchServiceAdminAPIKey";
        public const string AssetCacheExpiryTimeMins = "DFC.Digital.AssetCacheExpiryTimeMins";
        public const string GovUkNotifyApiKey = "DFC.Digital.GovUkNotifyAPIKey";
        public const string GovUkNotifyTemplateId = "DFC.Digital.GovUkNotifyTemplateId";
        public const string ContentTypeKey = "ContentType";
        public const string ProviderKey = "ProviderName";
        public const string DefaultSuggesterName = "sg";
        public const string CourseSearchApiKey = "DFC.Digital.CourseSearchApiKey";
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
        public const string ServiceStatusFailedCheckLogsMessage = "Service status check failed, check logs with activity id";
        public const int DefaultMaxRelatedItems = 100;
        public const string UrlNameRegexPattern = @"[^\w\-\!\$\'\(\)\=\@\d_]+";
        public const string Technology = "Technology";
        public const string SocField = "SOC";
    }
}