using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ISitefinityPageNodeProxy
    {
        string GetURLName(PageNode pageNode);
    }
}
