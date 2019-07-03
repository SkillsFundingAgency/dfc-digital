using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DFC.Digital.Data.Interfaces
{
    public interface IWebAppContext
    {
        bool IsContentAuthoringSite { get; }

        bool IsPreviewMode { get; }

        bool IsContentPreviewMode { get; }

        // If we are in backend design mode but not backend preview mode
        bool IsContentAuthoringAndNotPreviewMode { get; }

        bool IsJobProfilePage { get; }

        bool IsCategoryPage { get; }

        bool IsSearchResultsPage { get; }

        bool IsUserAdministrator { get; }

        NameValueCollection RequestQueryString { get; }

        bool IsValidAndFormattedUrl(string urlToBeValidated);

        void SetVocCookie(string cookieName, string cookieValue);

        VocSurveyPersonalisation GetVocCookie(string cookieName);

        string GetGAClientId();

        void SetResponseStatusCode(int statusCode);

        void SetMetaDescription(string description);

        string GetCurrentUrlWithQueryString(Dictionary<string, object> additionalQueryStrings = null);

        string GetQueryStringExcluding(IEnumerable<string> keysToExclude);

        void SetupCanonicalUrlEventHandler();

        void CheckAuthenticationByAuthCookie();

        string ServerMapPath(string fileName);

        string GetUrlEncodedPathAndQuery();
    }
}