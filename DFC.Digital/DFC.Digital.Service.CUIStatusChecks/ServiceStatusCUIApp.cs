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
    public class ServiceStatusCUIApp : IServiceStatusCUIApp
    {
        private readonly IApplicationLogger applicationLogger;
        private readonly IHttpClientService<IServiceStatus> httpClient;

        #region ctor
        public ServiceStatusCUIApp(IApplicationLogger applicationLogger, IHttpClientService<IServiceStatus> httpClient)
        {
            this.applicationLogger = applicationLogger;
            this.httpClient = httpClient;
        }
        #endregion

        #region Implement of IServiceStatusCUIApp

        public async Task<ServiceStatus> GetCurrentCUIAppStatusAsync(string serviceName, string serviceEndPoint)
        {
            var serviceStatus = new ServiceStatus { Name = serviceName, Status = ServiceState.Red, CheckCorrelationId = Guid.NewGuid() };

            try
            {
                this.httpClient.AddHeader("Accept", "application/json");
                var response = await this.httpClient.GetAsync(serviceEndPoint);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    serviceStatus.Status = ServiceState.Amber;
                    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var cuiHealthStatuses = JsonConvert.DeserializeObject<IList<ServiceStatusCUIResponse>>(responseString);
                    serviceStatus.ChildAppStatuses = new List<ServiceStatusChildApp>();
                    foreach (ServiceStatusCUIResponse s in cuiHealthStatuses)
                    {
                        serviceStatus.ChildAppStatuses.Add(new ServiceStatusChildApp() { Name = s.Service, Status = s.Message.Contains("is available") ? ServiceState.Green : ServiceState.Red });
                    }

                    if (serviceStatus.ChildAppStatuses.All(s => s.Status == ServiceState.Green))
                    {
                        serviceStatus.Status = ServiceState.Green;
                        serviceStatus.CheckCorrelationId = Guid.Empty;
                    }
                }
                else
                {
                    applicationLogger.Warn($"{nameof(CUIJobprofilesHealthCheck)}.{nameof(GetCurrentCUIAppStatusAsync)} : {Constants.ServiceStatusWarnLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}] - Non Success Response StatusCode [{response.StatusCode}] Reason [{response.ReasonPhrase}]");
                }
            }
            catch (Exception ex)
            {
                applicationLogger.ErrorJustLogIt($"{nameof(CUIJobprofilesHealthCheck)}.{nameof(GetCurrentCUIAppStatusAsync)} : {Constants.ServiceStatusFailedLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}]", ex);
            }

            return serviceStatus;
        }
        #endregion
    }
}