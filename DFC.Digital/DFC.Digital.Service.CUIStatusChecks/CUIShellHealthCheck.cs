

using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using System;
using System.Threading.Tasks;

namespace DFC.Digital.Service.CUIStatusChecks
{
    public class CUIShellHealthCheck : IServiceStatus
    {
        private readonly IApplicationLogger applicationLogger;
        private readonly IConfigurationProvider configurationProvider;
        private readonly IHttpClientService<IServiceStatus> httpPublishingClient;

        #region ctor
        public CUIShellHealthCheck(IApplicationLogger applicationLogger, IConfigurationProvider configurationProvider, IHttpClientService<IServiceStatus> httpPublishingClient)
        {
            this.applicationLogger = applicationLogger;
            this.httpPublishingClient = httpPublishingClient;
            this.configurationProvider = configurationProvider;
        }
        #endregion

        #region Implement of IServiceStatus
        private static string ServiceName => "CUI: Shell";

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, CheckCorrelationId = Guid.NewGuid() };

            try
            {
                var response = await this.httpPublishingClient.GetAsync(configurationProvider.GetConfig<string>(Constants.CUIShellHealthEndPoint));
                if (response.IsSuccessStatusCode)
                {
                    // Got a response back
                    serviceStatus.Status = ServiceState.Green;
                    serviceStatus.CheckCorrelationId = Guid.Empty;
                }
                else
                {
                    applicationLogger.Warn($"{nameof(CUIShellHealthCheck)}.{nameof(GetCurrentStatusAsync)} : {Constants.ServiceStatusWarnLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}] - Non Success Response StatusCode [{response.StatusCode}] Reason [{response.ReasonPhrase}]");
                }
            }
            catch (Exception ex)
            {
                applicationLogger.ErrorJustLogIt($"{nameof(CUIShellHealthCheck)}.{nameof(GetCurrentStatusAsync)} : {Constants.ServiceStatusFailedLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}]", ex);
            }

            return serviceStatus;
        }
        #endregion

    }
}
