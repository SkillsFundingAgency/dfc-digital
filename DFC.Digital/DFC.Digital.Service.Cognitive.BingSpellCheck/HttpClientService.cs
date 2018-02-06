using DFC.Digital.Data.Interfaces;
using System.Net.Http;

namespace DFC.Digital.Service.Cognitive.BingSpellCheck
{
    public class HttpClientService : IHttpClientService
    {
        public HttpClient GetHttpClient()
        {
            return new HttpClient();
        }
    }
}