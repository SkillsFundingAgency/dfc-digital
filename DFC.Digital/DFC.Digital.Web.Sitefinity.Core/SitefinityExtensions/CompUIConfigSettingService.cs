﻿using DFC.Digital.Core;
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
        private readonly IConfigurationProvider configurationProvider;

        public CompUIConfigSettingService(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public string GetUrl()
        {
            var readConfig = this.configurationProvider.GetConfig<string>(Constants.MicroServiceHelpPreviewEndPoint);

            if (string.IsNullOrEmpty(readConfig))
            {
                return string.Empty;
            }

            return readConfig;
        }
    }
}