using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public class HttpClientService<TService> : IHttpClientService<TService>
    {
        private HttpClient httpClient = new HttpClient();

        public bool AddHeader(string key, string value)
        {
            if (!httpClient.DefaultRequestHeaders.Contains(key))
            {
                httpClient.DefaultRequestHeaders.Add(key, value);
                return true;
            }

            return false;
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return httpClient.GetAsync(new Uri(requestUri));
        }
    }
}