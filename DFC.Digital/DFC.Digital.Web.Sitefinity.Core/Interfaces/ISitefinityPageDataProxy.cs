using DFC.Digital.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ISitefinityPageDataProxy
    {
        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        string GetHtmlTitle(PageData pageData);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        string GetDescription(PageData pageData);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        string GetKeywords(PageData pageData);
    }
}
