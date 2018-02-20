using DFC.Digital.Data.Model;
using System.Collections.Specialized;
using System.Web;

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

        NameValueCollection RequestQueryString { get; }

        bool IsValidAndFormattedUrl(string urlTobeValidated);

        void SetVocCookie(string cookieName, string cookieValue);

        VocSurveyPersonalisation GetVocCookie(string cookieName);

        string GetGAClientId();
    }
}