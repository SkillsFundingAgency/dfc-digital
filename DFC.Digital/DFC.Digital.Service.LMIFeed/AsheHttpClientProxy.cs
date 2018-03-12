using DFC.Digital.Core;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Service.LMIFeed.Interfaces;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Service.LMIFeed
{
    public class AsheHttpClientProxy : IAsheHttpClientProxy
    {
        private readonly IHttpClientService httpClient;
        private readonly ITolerancePolicy policy;

        public AsheHttpClientProxy(IHttpClientService httpClient, ITolerancePolicy policy)
        {
            this.httpClient = httpClient;
            this.policy = policy;
        }

        #region Implementation of IAsheHttpClientProxy

        public async Task<HttpResponseMessage> EstimatePayMdAsync(string socCode)
        {
            var url = ConfigurationManager.AppSettings[Constants.AsheEstimateMDApiGateway];
            var accessKey = ConfigurationManager.AppSettings[Constants.AsheAccessKey];

            return await policy.ExecuteAsync(
                () => httpClient.GetHttpClient().GetAsync(string.Format(url, socCode, accessKey)), 
                r => !r.IsSuccessStatusCode,
                Constants.Ashe, 
                FaultToleranceType.RetryWithCircuitBreaker);
        }

        #endregion Implementation of IAsheHttpClientProxy
    }
}