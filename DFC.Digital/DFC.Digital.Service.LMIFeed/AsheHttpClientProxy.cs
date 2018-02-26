using DFC.Digital.Core.Utilities;
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

        public AsheHttpClientProxy(IHttpClientService httpClient)
        {
            this.httpClient = httpClient;
        }

        #region Implementation of IAsheHttpClientProxy

        public async Task<HttpResponseMessage> EstimatePayMdAsync(string socCode)
        {
            var url = ConfigurationManager.AppSettings[Constants.AsheEstimateMDApiGateway];
            var accessKey = ConfigurationManager.AppSettings[Constants.AsheAccessKey];

            return await httpClient.GetHttpClient().GetAsync(string.Format(url, socCode, accessKey));
        }

        #endregion Implementation of IAsheHttpClientProxy
    }
}