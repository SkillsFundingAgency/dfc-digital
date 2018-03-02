using System.Net.Http;

namespace DFC.Digital.Core
{
    public interface IHttpClientService
    {
        HttpClient GetHttpClient();
    }
}