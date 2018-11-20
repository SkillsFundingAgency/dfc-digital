using System;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public interface ICachingPolicy
    {
        Task<T> ExecuteAsync<T>(Func<Task<T>> action, CachePolicyType cachePolicyType, string policyName, string cacheContext);

        T Execute<T>(Func<T> action, CachePolicyType cachePolicyType, string policyName, string cacheContext);
    }
}