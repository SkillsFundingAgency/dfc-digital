using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Service.CUIStatusChecks
{
    public class CUIContentPagesHealthCheck : IServiceStatus
    {
        private readonly IServiceStatusCUIApp serviceStatusCUIApp;
        private readonly IConfigurationProvider configurationProvider;

        #region ctor
        public CUIContentPagesHealthCheck(IConfigurationProvider configurationProvider, IServiceStatusCUIApp serviceStatusCUIApp)
        {
            this.configurationProvider = configurationProvider;
            this.serviceStatusCUIApp = serviceStatusCUIApp;
        }
        #endregion

        #region Implement of IServiceStatus
        private static string ServiceName => "CUI: Content pages";

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            return await serviceStatusCUIApp.GetCurrentCUIAppStatusAsync(ServiceName, configurationProvider.GetConfig<string>(Constants.CUIAppContentPagesHealthEndPoint));
        }
        #endregion

    }
}
