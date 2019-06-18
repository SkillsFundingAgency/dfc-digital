using DFC.Digital.Core.Interceptors;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ISitefinityPageNodeProxy
    {
        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        string GetURLName(PageNode pageNode);
    }
}
