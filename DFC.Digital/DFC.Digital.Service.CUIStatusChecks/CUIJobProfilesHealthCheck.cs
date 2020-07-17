

using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Digital.Service.CUIStatusChecks
{
    public class CUIJobprofilesHealthCheck : IServiceStatus
    {
        private readonly IApplicationLogger applicationLogger;
        private readonly IConfigurationProvider configurationProvider;
        private readonly IHttpClientService<IServiceStatus> httpClient;

        #region ctor
        public CUIJobprofilesHealthCheck(IApplicationLogger applicationLogger, IConfigurationProvider configurationProvider, IHttpClientService<IServiceStatus> httpPublishingClient)
        {
            this.applicationLogger = applicationLogger;
            this.httpClient = httpPublishingClient;
            this.configurationProvider = configurationProvider;
        }
        #endregion

        #region Implement of IServiceStatus
        private static string ServiceName => "CUI: Jobprofiles";

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, CheckCorrelationId = Guid.NewGuid() };

            try
            {
                this.httpClient.AddHeader("Accept", "application/json");
                var response = await this.httpClient.GetAsync(configurationProvider.GetConfig<string>(Constants.CUIAppJobProfilesHealthEndPoint));

                if (response.IsSuccessStatusCode)
                {
                    serviceStatus.Status = ServiceState.Amber;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var result = JsonConvert.DeserializeObject<IList<ServiceStatusCUIApps>>(responseString);
                        serviceStatus.Status = ServiceState.Green;
                        serviceStatus.CheckCorrelationId = Guid.Empty;
                    }
                }
                else
                {
                    applicationLogger.Warn($"{nameof(CUIJobprofilesHealthCheck)}.{nameof(GetCurrentStatusAsync)} : {Constants.ServiceStatusWarnLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}] - Non Success Response StatusCode [{response.StatusCode}] Reason [{response.ReasonPhrase}]");
                }
            }
            catch (Exception ex)
            {
                applicationLogger.ErrorJustLogIt($"{nameof(CUIJobprofilesHealthCheck)}.{nameof(GetCurrentStatusAsync)} : {Constants.ServiceStatusFailedLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}]", ex);
            }

            return serviceStatus;
        }
        #endregion

    }
}
