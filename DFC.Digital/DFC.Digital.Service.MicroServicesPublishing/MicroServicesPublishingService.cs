using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Service.MicroServicesPublishing
{
    public class MicroServicesPublishingService : IMicroServicesPublishingService
    {
        //Have removed this the IServiceStatus inteface from above to stop it been auto called for now.
        //Once the method of checking the status for each service is definied this can be implemented and the interface readded.
        private readonly IApplicationLogger applicationLogger;
        private readonly IHttpClientService<IMicroServicesPublishingService> httpPublishingClient;
        private readonly IConfigurationProvider configurationProvider;

        #region ctor

        public MicroServicesPublishingService(IApplicationLogger applicationLogger, IConfigurationProvider configurationProvider,  IHttpClientService<IMicroServicesPublishingService> httpPublishingClient)
        {
            this.applicationLogger = applicationLogger;
            this.httpPublishingClient = httpPublishingClient;
            this.configurationProvider = configurationProvider;
        }

        #endregion ctor

        private static string ServiceName => "Micro Service Publishing";

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, Notes = string.Empty };
            try
            {
                var compositePageData = new MicroServicesPublishingPageData() { CanonicalName = "ServiceStatusCheck", IncludeInSiteMap = false, BreadcrumbTitle = "Last updated = {DateTime.Now}" };

                // Use the key for help at the moment, this needs to be expanded to pick up all keys that are posing to a micro service.
                var response = await this.httpPublishingClient.PostAsync(configurationProvider.GetConfig<string>("DFC.Digital.MicroService.HelpEndPoint"), JsonConvert.SerializeObject(compositePageData));
                if (response.IsSuccessStatusCode)
                {
                    // Got a response back
                    serviceStatus.Status = ServiceState.Green;
                }
                else
                {
                    serviceStatus.Notes = $"Non Success Response StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                serviceStatus.Notes = $"{Constants.ServiceStatusFailedCheckLogsMessage} - {this.applicationLogger.LogExceptionWithActivityId(Constants.ServiceStatusFailedLogMessage, ex)}";
            }

            return serviceStatus;
        }

        public async Task<bool> PostPageDataAsync(string microServiceEndPointConfigKey, MicroServicesPublishingPageData compositePageData)
        {
            var pageDataJson = JsonConvert.SerializeObject(compositePageData);

            // Get the correct end point to send this request from configurations, Key to use is passed in as we go to diffrent endpoints depending on the page
            var response = await httpPublishingClient.PostAsync(configurationProvider.GetConfig<string>(microServiceEndPointConfigKey), pageDataJson);
            return response.IsSuccessStatusCode;

        }

        public async Task<bool> DeletePageAsync(string microServiceEndPointConfigKey, Guid pageId)
        {
            // Get the correct end point to send this request from configurations, Key to use is passed in as we go to diffrent endpoints depending on the page
            string deleteEndPoint = $"{configurationProvider.GetConfig<string>(microServiceEndPointConfigKey)?.TrimEnd('/')}/{pageId}";
            var response = await httpPublishingClient.DeleteAsync(deleteEndPoint, res => !res.IsSuccessStatusCode || res.StatusCode != System.Net.HttpStatusCode.NotFound);
            return response.IsSuccessStatusCode;
        }
    }
}
