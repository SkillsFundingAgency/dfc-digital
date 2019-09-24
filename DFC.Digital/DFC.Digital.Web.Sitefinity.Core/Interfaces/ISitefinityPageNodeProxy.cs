using DFC.Digital.Core.Interceptors;
using System;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.DynamicTypes.Model;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ISitefinityPageNodeProxy
    {
        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        string GetPageName(PageNode pageNode);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        string GetCustomField(PageNode pageNode, string customFieldName);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        DateTime GetLastPublishedDate(PageNode pageNode);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        string GetCustomContentField(DynamicContent dynamicContentNode, string customFieldName);
    }
}
