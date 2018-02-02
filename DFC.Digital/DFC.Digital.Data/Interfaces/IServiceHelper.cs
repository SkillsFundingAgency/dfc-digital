using System;

namespace DFC.Digital.Data.Interfaces
{
    public interface IServiceHelper
    {
        void Use<TService>(Action<TService> action, string endpointConfigName = null);

        TReturn Use<TService, TReturn>(Func<TService, TReturn> action, string endpointConfigName = null);
    }
}