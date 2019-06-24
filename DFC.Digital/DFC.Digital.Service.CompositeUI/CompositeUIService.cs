using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

namespace DFC.Digital.Service.CompositeUI
{
    public class CompositeUIService : ICompositeUIService, IServiceStatus
    {
        private readonly IApplicationLogger applicationLogger;
        private readonly ICompositeClientProxy compositeClientProxy;

        #region ctor

        public CompositeUIService(IApplicationLogger applicationLogger, ICompositeClientProxy compositeClientProxy)
        {
            this.applicationLogger = applicationLogger;
            this.compositeClientProxy = compositeClientProxy;
        }

        #endregion ctor

        #region Implement of IServiceStatus
        private static string ServiceName => "Composite UI Publish";

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, Notes = string.Empty };
            try
            {
                var compositePageData = new CompositePageData() { Name = "ServiceStatusCheck", IncludeInSitemap = false, Title = "Last updated = {DateTime.Now}" };

                //Use the key for help at the moment, this needs to be expanded to pick up all keys that are posing to a micro service.
                var response = await compositeClientProxy.PostDataAsync(ConfigurationManager.AppSettings["DFC.Digital.MicroService-Help-EndPoint"], JsonConvert.SerializeObject(compositePageData));
                if (response.IsSuccessStatusCode)
                {
                    //Got a response back
                    serviceStatus.Status = ServiceState.Green;
                }
                else
                {
                    serviceStatus.Notes = $"Non Success Response StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                serviceStatus.Notes = $"{Constants.ServiceStatusFailedCheckLogsMessage} - {applicationLogger.LogExceptionWithActivityId(Constants.ServiceStatusFailedLogMessage, ex)}";
            }
            return serviceStatus;
        }

        #endregion

        #region Implement of ICompositeUIService

        public async Task<bool> PostPageDataAsync(string microServiceEndPointConfigKey, CompositePageData compositePageData)
        {
            var pageDataJson = JsonConvert.SerializeObject(compositePageData);
            applicationLogger.Trace($"Posting page data to api - [{pageDataJson}]");

            //Get the correct end point to send this request from configurations, Key to use is passed in as we go to diffrent endpoints depending on the page
            var response = await compositeClientProxy.PostDataAsync(ConfigurationManager.AppSettings[microServiceEndPointConfigKey], pageDataJson);
            if (response.IsSuccessStatusCode)
            {
                applicationLogger.Info($"Posted page data for {compositePageData.Name}");
                return true;
            }
            applicationLogger.Info($"Failed to posted page data for {compositePageData.Name}");
            return false;
        }
        #endregion
    }
}
