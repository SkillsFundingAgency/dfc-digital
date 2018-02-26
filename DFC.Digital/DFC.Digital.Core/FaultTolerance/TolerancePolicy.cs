using DFC.Digital.Data.Interfaces;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public class TolerancePolicy : ITolerancePolicy
    {
        private readonly IApplicationLogger logger;
        private ConcurrentDictionary<string, Policy> policies = new ConcurrentDictionary<string, Policy>();

        public TolerancePolicy(IApplicationLogger logger)
        {
            this.logger = logger;
        }

        public Task<T> ExecuteAsync<T>(Func<Task<T>> action, string dependencyName, FaultToleranceType toleranceType)
        {
            Policy policy;

            switch (toleranceType)
            {
                case FaultToleranceType.Timeout:
                    policy = policies.GetOrAdd(dependencyName, Policy.Timeout(TimeSpan.FromSeconds(2), TimeoutStrategy.Pessimistic));
                    break;
                case FaultToleranceType.Retry:
                    policy = policies.GetOrAdd(dependencyName, GetLimitedRetryPolicy(dependencyName));
                    break;
                case FaultToleranceType.RetryWithCircuitBreaker:
                    policy = policies.GetOrAdd(dependencyName, GetRetryWithCircuitBreaker(dependencyName));
                    break;
                case FaultToleranceType.CircuitBreaker:
                default:
                    policy = policies.GetOrAdd(dependencyName, GetCircuitBreaker(dependencyName));
                    break;
            }

            return policy.ExecuteAsync(action);
        }

        public Task<T> ExecuteAndRetryWithCircuitBreaker<T>(Func<Task<T>> action, string dependencyName)
        {
            var policy = policies.GetOrAdd(dependencyName, GetRetryWithCircuitBreaker(dependencyName));
            return policy.ExecuteAsync(action);
        }

        private CircuitBreakerPolicy GetCircuitBreaker(string dependency)
        {
            //Define our CircuitBreaker policy: Break if the action fails 4 times in a row.
            return Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 4,
                    durationOfBreak: TimeSpan.FromSeconds(60),
                    onBreak: (ex, breakDelay) => logger.Warn($"Circuit-Breaker logging: {dependency}: Breaking the circuit for {breakDelay}, failure-{ex.Message}"),
                    onReset: () => logger.Info($"Circuit-Breaker logging: {dependency}: Call succeeded. Closed the circuit again!"),
                    onHalfOpen: () => logger.Warn($"Circuit-Breaker logging: {dependency}: Half-open: Next call is a trial!"));
        }

        private PolicyWrap GetRetryWithCircuitBreaker(string dependency)
        {
            var waitAndRetryPolicy = Policy
                .Handle<Exception>(e => !(e is BrokenCircuitException)) // Exception filtering!  We don't retry if the inner circuit-breaker judges the underlying system is out of commission!
                .WaitAndRetryAsync(
                    3,
                    wait => TimeSpan.FromMilliseconds(200),
                    (ex, wait, attempt, ctx) => logger.Warn($"Wait and Retry: {dependency}: Attempt-{attempt}, Waiting for-{wait}, failure-{ex.Message}"));

            return Policy.WrapAsync(waitAndRetryPolicy, GetCircuitBreaker(dependency));
        }

        private RetryPolicy GetLimitedRetryPolicy(string dependency)
        {
            return Policy
                .Handle<Exception>()
                .Retry(3, (ex, attempt) => logger.Warn($"Retry policy: {dependency}: Attempt-{attempt}, failure-{ex.Message}"));
        }
    }
}