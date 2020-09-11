using System.ServiceModel;
using System.ServiceModel.Activation;

namespace DFC.Digital.Web.Sitefinity.Core
{
    /// <summary>
    /// Sitefinity web service.
    /// </summary>
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CompUIConfigSettingService : ICompUIConfigSettingService
    {
        public string GetFromKey(string key)
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings[key];
        }
    }
}