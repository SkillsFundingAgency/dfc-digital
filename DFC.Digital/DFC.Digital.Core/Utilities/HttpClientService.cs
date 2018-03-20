﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public class HttpClientService<TService> : IHttpClientService<TService>
    {
        private readonly ITolerancePolicy policy;
        private HttpClient httpClient = new HttpClient();

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

        public Task<HttpResponseMessage> GetAsync(string requestUri, FaultToleranceType toleranceType = FaultToleranceType.RetryWithCircuitBreaker)
        {
            return policy.ExecuteAsync(() => httpClient.GetAsync(new Uri(requestUri)), this.GetType().Name, toleranceType);
        }
    }
}