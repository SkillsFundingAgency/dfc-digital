using Polly;
using System;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public interface ITolerancePolicy
    {
        Task<T> ExecuteAsync<T>(Func<Task<T>> action, string dependencyName, FaultToleranceType toleranceType);
    }
}