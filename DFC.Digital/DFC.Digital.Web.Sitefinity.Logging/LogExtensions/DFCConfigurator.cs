using Telerik.Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Telerik.Sitefinity.Logging;

namespace DFC.Digital.Web.Sitefinity.Logging
{
    public class DfcConfigurator : ISitefinityLogCategoryConfigurator
    {
        public void Configure(SitefinityLogCategory category)
        {
            category.Configuration
                    .WithOptions
                    .SendTo
                    .Custom<DfcLogListener>("Custom");
        }
    }
}