using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public class HttpClientService<TService> : IHttpClientService<TService>
    {
        private readonly ITolerancePolicy policy;
        private readonly HttpClient httpClient = new HttpClient();

        public HttpClientService(ITolerancePolicy policy)
        {
            this.policy = policy;
        }

        public bool AddHeader(string key, string value)
        {
            if (!httpClient.DefaultRequestHeaders.Contains(key))
            {
                httpClient.DefaultRequestHeaders.Add(key, value);
                return true;
            }

            return false;
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri, FaultToleranceType toleranceType = FaultToleranceType.RetryWithCircuitBreaker)
        {
            return await policy.ExecuteAsync(() => httpClient.GetAsync(new Uri(requestUri)), response => !response.IsSuccessStatusCode, $"{typeof(TService).Name}-{nameof(GetAsync)}", toleranceType);
        }

        public async Task<HttpResponseMessage> GetWhereAsync(string requestUri, Func<HttpResponseMessage, bool> predicate, FaultToleranceType toleranceType = FaultToleranceType.RetryWithCircuitBreaker)
        {
            return await policy.ExecuteAsync(() => httpClient.GetAsync(new Uri(requestUri)), predicate, $"{typeof(TService).Name}-{nameof(GetWhereAsync)}", toleranceType);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, string content, FaultToleranceType toleranceType = FaultToleranceType.RetryWithCircuitBreaker)
        {
            return await policy.ExecuteAsync(() => httpClient.PostAsync(new Uri(requestUri), new StringContent(content, Encoding.UTF8, "application/json")), response => !response.IsSuccessStatusCode, $"{typeof(TService).Name}-{nameof(PostAsync)}", toleranceType);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri, Func<HttpResponseMessage, bool> predicate = null, FaultToleranceType toleranceType = FaultToleranceType.RetryWithCircuitBreaker)
        {
            return await policy.ExecuteAsync(() => httpClient.DeleteAsync(new Uri(requestUri)), predicate ?? (response => !response.IsSuccessStatusCode), $"{typeof(TService).Name}-{nameof(DeleteAsync)}", toleranceType);
        }
    }
}