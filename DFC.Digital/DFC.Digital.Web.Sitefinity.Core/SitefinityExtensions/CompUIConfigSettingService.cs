using DFC.Digital.Core;
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
        public string GetUrl()
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings[Constants.MicroServiceHelpPreviewEndPoint];
        }
    }
}