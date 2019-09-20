using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public interface IHttpClientService<TService>
    {
        bool AddHeader(string key, string value);

        Task<HttpResponseMessage> GetAsync(string requestUri, FaultToleranceType toleranceType = FaultToleranceType.RetryWithCircuitBreaker);

        Task<HttpResponseMessage> PostAsync(string requestUri, string content, FaultToleranceType toleranceType = FaultToleranceType.RetryWithCircuitBreaker);

        Task<HttpResponseMessage> DeleteAsync(string requestUri, Func<HttpResponseMessage, bool> predicate = null, FaultToleranceType toleranceType = FaultToleranceType.RetryWithCircuitBreaker);
    }
}