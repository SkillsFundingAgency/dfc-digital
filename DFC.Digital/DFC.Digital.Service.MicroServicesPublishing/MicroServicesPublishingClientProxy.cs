using DFC.Digital.Core;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Service.MicroServicesPublishing
{
    public class MicroServicesPublishingClientProxy : IMicroServicesPublishingClientProxy
    {
        private readonly IHttpClientService<IMicroServicesPublishingClientProxy> httpClient;

        public MicroServicesPublishingClientProxy(IHttpClientService<IMicroServicesPublishingClientProxy> httpClient)
        {
           this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> PostDataAsync(string postEndPoint, string pageDataJson)
        {
            return await httpClient.PostAsync(postEndPoint, pageDataJson);
        }
      }
}
