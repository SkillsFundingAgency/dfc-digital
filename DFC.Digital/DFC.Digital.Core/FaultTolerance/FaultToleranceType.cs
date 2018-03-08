namespace DFC.Digital.Core
{
    public enum FaultToleranceType
    {
        Timeout,
        Retry,
        WaitRetry,
        CircuitBreaker,
        RetryWithCircuitBreaker
    }
}