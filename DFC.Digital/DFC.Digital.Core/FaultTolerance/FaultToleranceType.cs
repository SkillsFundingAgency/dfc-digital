namespace DFC.Digital.Core
{
    public enum FaultToleranceType
    {
        NoPolicy,
        Timeout,
        Retry,
        WaitRetry,
        CircuitBreaker,
        RetryWithCircuitBreaker
    }
}