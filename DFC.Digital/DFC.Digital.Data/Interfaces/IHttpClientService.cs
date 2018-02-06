using System.Net.Http;

namespace DFC.Digital.Data.Interfaces
{
    public interface IHttpClientService
    {
        HttpClient GetHttpClient();
    }
}