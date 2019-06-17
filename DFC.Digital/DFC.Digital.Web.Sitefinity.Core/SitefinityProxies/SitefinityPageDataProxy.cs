using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
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
    }
}