using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
