using DFC.Digital.Data.Interfaces;
using System.Net.Http;

namespace DFC.Digital.Core.Utilities
{
    public class HttpClientService : IHttpClientService
    {
        private HttpClient httpClient = new HttpClient();

        public HttpClient GetHttpClient()
        {
            return httpClient;
        }
    }
}