using Telerik.Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Telerik.Sitefinity.Logging;

namespace DFC.Digital.Web.Sitefinity.Logging
{
    public class DFCConfigurator : ISitefinityLogCategoryConfigurator
    {
        public void Configure(SitefinityLogCategory category)
        {
            category.Configuration
                    .WithOptions
                    .SendTo
                    .Custom<DFCLogListener>("Custom");
        }
    }
}