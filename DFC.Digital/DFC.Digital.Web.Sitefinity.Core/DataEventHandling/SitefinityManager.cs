using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Modules.Pages;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class SitefinityManager : ISitefinityManager
    {
        public PageManager GetManager(string providerName)
        {
            return PageManager.GetManager(providerName);
        }

        public PageManager GetManager()
        {
            return PageManager.GetManager();
        }
    }
}