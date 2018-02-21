using System;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IServiceHelper
    {
        void Use<TService>(Action<TService> action, string endpointConfigName = null);

        TReturn Use<TService, TReturn>(Func<TService, TReturn> action, string endpointConfigName = null);

        Task<TReturn> UseAsync<TService, TReturn>(Func<TService, Task<TReturn>> action, string endpointConfigName = null);
    }
}