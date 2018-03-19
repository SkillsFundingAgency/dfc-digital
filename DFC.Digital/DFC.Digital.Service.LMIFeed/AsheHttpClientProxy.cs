using DFC.Digital.Core;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Service.LMIFeed
{
    public class AsheHttpClientProxy : IAsheHttpClientProxy
    {
        private readonly IHttpClientService<IAsheHttpClientProxy> httpClient;

        public AsheHttpClientProxy(IHttpClientService<IAsheHttpClientProxy> httpClient)
        {
            this.httpClient = httpClient;
        }

        #region Implementation of IAsheHttpClientProxy

        public async Task<HttpResponseMessage> EstimatePayMdAsync(string socCode)
        {
            var url = ConfigurationManager.AppSettings[Constants.AsheEstimateMDApiGateway];
            var accessKey = ConfigurationManager.AppSettings[Constants.AsheAccessKey];

            return await httpClient.GetAsync(string.Format(url, socCode, accessKey));
        }

        #endregion Implementation of IAsheHttpClientProxy
    }
}