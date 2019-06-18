using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SitefinityPageNodeProxy : ISitefinityPageNodeProxy
    {
        public string GetURLName(PageNode pageNode)
        {
            return pageNode.UrlName.Value;
        }
    }
}