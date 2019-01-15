using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Sitefinity.Frontend.Mvc.Helpers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure;
using Telerik.Sitefinity.Services;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class WebAppContext : IWebAppContext
    {
        private const string CanonicalAttrKey = "rel";
        private const string CanonicalAttrValue = "canonical";

        public Page CurrentPage => new HttpContextWrapper(HttpContext.Current).CurrentHandler?.GetPageHandler();

        public bool IsContentAuthoringSite => SitefinityContext.IsBackend;

        public bool IsPreviewMode => SitefinityContext.IsPreviewMode;

        public bool IsContentPreviewMode
        {
            get
            {
                //The SitefinityContext.IsPreviewMode only seems to get set to true, when the preview button for a page template is used.
                //When you come via the content type preview button this is aways false.
                if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString["sf-content-action"] == "preview")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsContentAuthoringAndNotPreviewMode => IsContentAuthoringSite && !IsPreviewMode;

        public bool IsSearchResultsPage => HttpContext.Current.Request.Url.ToString().ToLower().Contains("/search-results");

        public bool IsUserAdministrator
        {
            get
            {
                return Telerik.Sitefinity.Security.Claims.ClaimsManager.GetCurrentIdentity().Roles.ToList().Exists(x => x.Name == "Administrators");
            }
        }

        public bool IsCategoryPage => HttpContext.Current.Request.Url.ToString().ToLower().Contains("/job-categories");

        public bool IsJobProfilePage => HttpContext.Current.Request.Url.ToString().ToLower().Contains("/job-profiles");

        public NameValueCollection RequestQueryString => HttpContext.Current.Request.QueryString;

        public bool IsValidAndFormattedUrl(string urlToBeValidated) => Uri.IsWellFormedUriString($"{urlToBeValidated}", UriKind.RelativeOrAbsolute);

        public VocSurveyPersonalisation GetVocCookie(string cookieName)
        {
            var profile = new VocSurveyPersonalisation();
            var cookies = HttpContext.Current?.Request.Cookies;
            if (cookies != null)
            {
                var cookie = cookies.Get(cookieName)?.Value;
                if (!string.IsNullOrEmpty(cookie))
                {
                    profile.Personalisation.Add(Constants.LastVisitedJobProfileKey, HttpUtility.UrlDecode(cookie));
                }
            }

            if (!profile.Personalisation.ContainsKey(Constants.LastVisitedJobProfileKey))
            {
                profile.Personalisation.Add(Constants.LastVisitedJobProfileKey, Constants.Unknown);
            }

            profile.Personalisation.Add(Constants.GoogleClientIdKey, GetGAClientId());
            return profile;
        }

        public void SetVocCookie(string cookieName, string cookieValue)
        {
            HttpContext.Current.Response.Cookies[cookieName].Value = cookieValue;
        }

        public string GetGAClientId()
        {
            var cookie = HttpContext.Current?.Request.Cookies.Get(Constants.GoogleAnalyticsCookie)?.Value;
            if (!string.IsNullOrEmpty(cookie))
            {
                // The GA id has got 3 components seperated by "." we are Interested in the value following second "."
                var clientId = $"{cookie.Substring(cookie.IndexOf('.', cookie.IndexOf('.') + 1) + 1)}";
                return clientId;
            }

            return "Unknown";
        }

        public void SetResponseStatusCode(int statusCode)
        {
            HttpContext.Current.Response.StatusCode = statusCode;
        }

        public void SetMetaDescription(string description)
        {
            if (HttpContext.Current.CurrentHandler is Page page)
            {
                page.MetaDescription = description;
            }
        }

        public string GetCurrentQueryString(Dictionary<string, object> additionalQueryStrings)
        {
            var currentUrl = HttpContext.Current?.Request.RawUrl;
            return RequestQueryString?.Count > 0
                ? $"{currentUrl}&{string.Join("&", additionalQueryStrings.Select(a => $"{a.Key}={a.Value}"))}"
                : $"{currentUrl}?{string.Join("&", additionalQueryStrings.Select(a => $"{a.Key}={a.Value}"))}";
        }

        public void SetupCanonicalUrlEventHandler()
        {
            var page = CurrentPage;

            if (page != null)
            {
                page.PreRenderComplete += Page_PreRenderComplete;
            }
        }

        public void CheckAuthenticationByAuthCookie()
        {
            if (!SystemManager.CurrentHttpContext.Request.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("The current user is not allowed access");
            }
        }

        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            RemoveCanonicalTag();
            AddCanonicalTag();
        }

        private void AddCanonicalTag()
        {
            var page = CurrentPage;
            if (page != null)
            {
                var link = new HtmlLink();
                link.Attributes.Add(CanonicalAttrKey, CanonicalAttrValue);
                link.Href = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("?", StringComparison.InvariantCultureIgnoreCase) > 0 ? HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("?", StringComparison.InvariantCultureIgnoreCase) : HttpContext.Current.Request.Url.AbsoluteUri.Length);

                if (!string.IsNullOrWhiteSpace(link.Href))
                {
                    page.Header.Controls.Add(link);
                }
            }
        }

        private void RemoveCanonicalTag()
        {
            var page = CurrentPage;
            if (page != null)
            {
                var headerControls = page.Header.Controls;
                foreach (var control in headerControls)
                {
                    if (control is HtmlLink link)
                    {
                        if (link.Attributes[CanonicalAttrKey] == CanonicalAttrValue)
                        {
                            headerControls.Remove(link);
                            break;
                        }
                    }
                }
            }
        }
    }
}