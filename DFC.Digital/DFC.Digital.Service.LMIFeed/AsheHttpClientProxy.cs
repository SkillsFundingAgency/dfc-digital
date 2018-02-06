using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.Digital.Core.Utilities;
using DFC.Digital.Service.LMIFeed.Interfaces;
namespace DFC.Digital.Service.LMIFeed
{
    public class AsheHttpClientProxy : IAsheHttpClientProxy
    {
        #region Implementation of IAsheHttpClientProxy

        public async Task<HttpResponseMessage> EstimatePayMdAsync(string socCode)
        {
            using (var client = new HttpClient())
            {
                var url = ConfigurationManager.AppSettings[Constants.AsheEstimateMdApiGateway];
                var accessKey = ConfigurationManager.AppSettings[Constants.AsheAccessKey];

                return await client.GetAsync(string.Format(url, socCode, accessKey));
            }
        }

        #endregion Implementation of IAsheHttpClientProxy
    }
}