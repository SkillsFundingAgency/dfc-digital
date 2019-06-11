using DFC.Digital.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Modules.Pages;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ISitefinityManager
    {
        PageManager GetManager(string providerName);

        PageManager GetManager();
    }
}
