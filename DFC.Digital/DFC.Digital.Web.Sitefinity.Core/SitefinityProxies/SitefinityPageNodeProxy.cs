using DFC.Digital.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class SitefinityPageNodeProxy : ISitefinityPageNodeProxy
    {
        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        public string GetURLName(PageNode pageNode)
        {
            return pageNode.UrlName.Value;
        }
    }
}