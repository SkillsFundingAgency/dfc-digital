using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public interface IHttpClientService<TService>
    {
        bool AddHeader(string key, string value);

        Task<HttpResponseMessage> GetAsync(string requestUri, FaultToleranceType toleranceType = FaultToleranceType.RetryWithCircuitBreaker);

        void SetBearerToken(string accessToken);

        void Accept(MediaTypeWithQualityHeaderValue mediaTypeWithQualityHeaderValue);
    }
}