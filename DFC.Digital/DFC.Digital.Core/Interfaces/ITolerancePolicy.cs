using System;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public interface ITolerancePolicy
    {
        Task<T> ExecuteAsync<T>(Func<Task<T>> action, string dependencyName, FaultToleranceType toleranceType);

        Task<T> ExecuteAsync<T>(Func<Task<T>> action, Func<T, bool> predicate, string dependencyName, FaultToleranceType toleranceType);

        T Execute<T>(Func<T> action, string dependencyName, FaultToleranceType toleranceType);
    }
}