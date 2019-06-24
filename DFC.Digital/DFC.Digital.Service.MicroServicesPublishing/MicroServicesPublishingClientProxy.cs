using DFC.Digital.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        #region Implementation of IMicroServicesPublishingClientProxy

        public async Task<HttpResponseMessage> PostDataAsync(string postEndPoint, string pageDataJSon)
        {
            return await httpClient.PostAsync(postEndPoint, pageDataJSon);
        }

        #endregion Implementation of IMicroServicesPublishingClientProxy
    }
}
