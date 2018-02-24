using Polly;
using System;
using System.Threading.Tasks;

namespace DFC.Digital.Core.FaultTolerance
{
    public interface ITolerancePolicy
    {
        Task<T> ExecuteWithCircuitBreaker<T>(Func<Task<T>> action, string dependencyName);
    }
}