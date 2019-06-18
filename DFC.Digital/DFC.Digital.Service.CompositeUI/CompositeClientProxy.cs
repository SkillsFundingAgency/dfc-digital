using DFC.Digital.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.CompositeUI
{
  
    public class CompositeClientProxy : ICompositeClientProxy
    {
        private readonly IHttpClientService<ICompositeClientProxy> httpClient;

        public CompositeClientProxy(IHttpClientService<ICompositeClientProxy> httpClient)
        {
            this.httpClient = httpClient;
        }

        #region Implementation of ICompositeClientProxy

        public async Task<HttpResponseMessage> PostDataAsync(string pageDataJSon)
        {
            return await httpClient.PostAsync(ConfigurationManager.AppSettings[Constants.CompositeUIPublishEndPoint], pageDataJSon);
        }

        #endregion Implementation of ICompositeClientProxy
    }
}
