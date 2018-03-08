using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public interface IHttpClientService<TService>
    {
        bool AddHeader(string key, string value);

        Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}