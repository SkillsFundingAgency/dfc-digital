using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SitefinityPageDataProxy : ISitefinityPageDataProxy
    {
        public string GetDescription(PageData pageData)
        {
            return pageData.Description.Value;
        }

        public string GetHtmlTitle(PageData pageData)
        {
            return pageData.HtmlTitle.Value;
        }

        public string GetKeywords(PageData pageData)
        {
            return pageData.Keywords.Value;
        }

        public string GetTitle(PageData pageData)
        {
            return pageData.NavigationNode.Title.Value;
        }
    }
}