﻿using DFC.Digital.Data.Interfaces;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public class TolerancePolicy : ITolerancePolicy
    {
        private readonly IApplicationLogger logger;
        private readonly TransientFaultHandlingStrategy strategy;
        private ConcurrentDictionary<string, object> policies = new ConcurrentDictionary<string, object>();

        public TolerancePolicy(IApplicationLogger logger, TransientFaultHandlingStrategy strategy)
        {
            this.logger = logger;
            this.strategy = strategy;
        }

        public T Execute<T>(Func<T> action, string dependencyName, FaultToleranceType toleranceType)
        {
            Policy policy;

            switch (toleranceType)
            {
                case FaultToleranceType.Retry:
                    policy = (Policy)policies.GetOrAdd($"{dependencyName}-sync", GetLimitedRetryPolicy(dependencyName));
                    break;

                case FaultToleranceType.WaitRetry:
                    policy = (Policy)policies.GetOrAdd($"{dependencyName}-sync", GetWaitAndRetryPolicy(dependencyName));
                    break;

                case FaultToleranceType.RetryWithCircuitBreaker:
                    policy = (Policy)policies.GetOrAdd($"{dependencyName}-sync", GetRetryWithCircuitBreaker(dependencyName));
                    break;

                case FaultToleranceType.CircuitBreaker:
                    policy = (Policy)policies.GetOrAdd($"{dependencyName}-sync", GetCircuitBreaker(dependencyName));
                    break;

                case FaultToleranceType.Timeout:
                default:
                    policy = (Policy)policies.GetOrAdd($"{dependencyName}-sync", Policy.Timeout(strategy.Timeout, TimeoutStrategy.Pessimistic));
                    break;
            }

            return policy.Execute(action);
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, string dependencyName, FaultToleranceType toleranceType)
        {
            Policy policy;

            switch (toleranceType)
            {
                case FaultToleranceType.Retry:
                    policy = (Policy)policies.GetOrAdd(dependencyName, GetLimitedRetryPolicyAsync(dependencyName));
                    break;

                case FaultToleranceType.WaitRetry:
                    policy = (Policy)policies.GetOrAdd(dependencyName, GetWaitAndRetryPolicyAsync(dependencyName));
                    break;

                case FaultToleranceType.RetryWithCircuitBreaker:
                    policy = (Policy)policies.GetOrAdd(dependencyName, GetRetryWithCircuitBreakerAsync(dependencyName));
                    break;

                case FaultToleranceType.CircuitBreaker:
                    policy = (Policy)policies.GetOrAdd(dependencyName, GetCircuitBreakerAsync(dependencyName));
                    break;

                case FaultToleranceType.Timeout:
                default:
                    policy = (Policy)policies.GetOrAdd(dependencyName, Policy.TimeoutAsync(strategy.Timeout, TimeoutStrategy.Pessimistic));
                    break;
            }

            return await policy.ExecuteAsync(action);
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, Func<T, bool> predicate, string dependencyName, FaultToleranceType toleranceType)
        {
            Policy<T> policy;
            var key = $"{dependencyName}-predicate";
            switch (toleranceType)
            {
                case FaultToleranceType.WaitRetry:
                    policy = (Policy<T>)policies.GetOrAdd(key, GetWaitAndRetryPolicyAsync(dependencyName, predicate));
                    break;
                case FaultToleranceType.RetryWithCircuitBreaker:
                    policy = (Policy<T>)policies.GetOrAdd(key, GetRetryWithCircuitBreakerAsync(dependencyName, predicate));
                    break;
                case FaultToleranceType.Retry:
                case FaultToleranceType.CircuitBreaker:
                case FaultToleranceType.Timeout:
                default:
                    return await ExecuteAsync(action, dependencyName, toleranceType);
            }

            return await policy.ExecuteAsync(action);
        }

        private Policy GetWaitAndRetryPolicyAsync(string dependencyName)
        {
            return Policy
                .Handle<Exception>(e => !(e is BrokenCircuitException)) // Exception filtering!  We don't retry if the inner circuit-breaker judges the underlying system is out of commission!
                .WaitAndRetryAsync(
                    strategy.Retry,
                    wait => strategy.Wait,
                    (ex, wait, attempt, ctx) => logger.Warn($"{nameof(TolerancePolicy)}:Wait and Retry policy: {dependencyName}: Attempt-{attempt}, Waiting for-{wait}, failure-{ex}"));
        }

        private Policy GetWaitAndRetryPolicy(string dependencyName)
        {
            return Policy
                .Handle<Exception>(e => !(e is BrokenCircuitException)) // Exception filtering!  We don't retry if the inner circuit-breaker judges the underlying system is out of commission!
                .WaitAndRetry(
                    strategy.Retry,
                    wait => strategy.Wait,
                    (ex, wait, attempt, ctx) => logger.Warn($"{nameof(TolerancePolicy)}:Wait and Retry policy: {dependencyName}: Attempt-{attempt}, Waiting for-{wait}, failure-{ex}"));
        }

        private Policy<T> GetWaitAndRetryPolicyAsync<T>(string dependencyName, Func<T, bool> predicate)
        {
            var retry = GetWaitAndRetryPolicyAsync(dependencyName);
            return retry
                .WrapAsync(Policy
                .HandleResult(predicate)
                .WaitAndRetryAsync(
                    strategy.Retry,
                    wait => strategy.Wait,
                    (result, wait, attempt, ctx) => logger.Warn($"{nameof(TolerancePolicy)}:Wait and Retry policy: {dependencyName}: Attempt-{attempt}, Waiting for-{wait}, failure-{result.ToJson()}")));
        }

        private Policy GetLimitedRetryPolicy(string dependency)
        {
            return Policy
                .Handle<Exception>()
                .Retry(strategy.Retry, (ex, attempt) => logger.Warn($"{nameof(TolerancePolicy)}:Retry policy: {dependency}: Attempt-{attempt}, failure-{ex.Message}"));
        }

        private RetryPolicy GetLimitedRetryPolicyAsync(string dependency)
        {
            return Policy
                .Handle<Exception>()
                .RetryAsync(strategy.Retry, (ex, attempt) => logger.Warn($"{nameof(TolerancePolicy)}:Retry policy: {dependency}: Attempt-{attempt}, failure-{ex.Message}"));
        }

        private Policy GetCircuitBreaker(string dependency)
        {
            return Policy
                .Handle<Exception>()
                .CircuitBreaker(
                    exceptionsAllowedBeforeBreaking: strategy.AllowedFaults,
                    durationOfBreak: strategy.Breaktime,
                    onBreak: (ex, breakDelay) => logger.Warn($"{nameof(TolerancePolicy)}:Circuit-Breaker logging: Broken : {dependency}: Breaking the circuit for {breakDelay}, failure-{ex.Message}"),
                    onReset: () => logger.Info($"{nameof(TolerancePolicy)}:Circuit-Breaker logging: {dependency}: Call succeeded. Closed the circuit again!"),
                    onHalfOpen: () => logger.Warn($"{nameof(TolerancePolicy)}:Circuit-Breaker logging: {dependency}: Half-open: Next call is a trial!"));
        }

        private CircuitBreakerPolicy GetCircuitBreakerAsync(string dependency)
        {
            //Define our CircuitBreaker policy: Break if the action fails in a row.
            return Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: strategy.AllowedFaults,
                    durationOfBreak: strategy.Breaktime,
                    onBreak: (ex, breakDelay) => logger.Warn($"{nameof(TolerancePolicy)}:Circuit-Breaker logging: Broken : {dependency}: Breaking the circuit for {breakDelay}, failure-{ex.Message}"),
                    onReset: () => logger.Info($"{nameof(TolerancePolicy)}:Circuit-Breaker logging: {dependency}: Call succeeded. Closed the circuit again!"),
                    onHalfOpen: () => logger.Warn($"{nameof(TolerancePolicy)}:Circuit-Breaker logging: {dependency}: Half-open: Next call is a trial!"));
        }

        private Policy<T> GetCircuitBreakerAsync<T>(string dependency, Func<T, bool> predicate)
        {
            var circuitBreaker = GetCircuitBreakerAsync(dependency);
            return circuitBreaker.WrapAsync(
                Policy<T>
                    .HandleResult(predicate)
                    .CircuitBreakerAsync(
                        handledEventsAllowedBeforeBreaking: strategy.AllowedFaults,
                        durationOfBreak: strategy.Breaktime,
                        onBreak: (result, breakDelay) => logger.Warn($"{nameof(TolerancePolicy)}:Circuit-Breaker logging: Broken : {dependency}: Breaking the circuit for {breakDelay}, failure-{result.ToJson()}"),
                        onReset: () => logger.Info($"{nameof(TolerancePolicy)}:Circuit-Breaker logging: {dependency}: Call succeeded. Closed the circuit again!"),
                        onHalfOpen: () => logger.Warn($"{nameof(TolerancePolicy)}:Circuit-Breaker logging: {dependency}: Half-open: Next call is a trial!")));
        }

        private PolicyWrap GetRetryWithCircuitBreaker(string dependency)
        {
            return Policy.Wrap(GetCircuitBreaker(dependency), GetWaitAndRetryPolicy(dependency));
        }

        private PolicyWrap GetRetryWithCircuitBreakerAsync(string dependency)
        {
            return Policy.WrapAsync(GetCircuitBreakerAsync(dependency), GetWaitAndRetryPolicyAsync(dependency));
        }

        private Policy<T> GetRetryWithCircuitBreakerAsync<T>(string dependency, Func<T, bool> predicate)
        {
            return Policy.WrapAsync(GetCircuitBreakerAsync(dependency, predicate), GetWaitAndRetryPolicyAsync(dependency, predicate));
        }
    }
}