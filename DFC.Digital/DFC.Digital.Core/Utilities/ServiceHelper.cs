using DFC.Digital.Data.Interfaces;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DFC.Digital.Core.Utilities
{
    public class ServiceHelper : IServiceHelper
    {
        public void Use<TService>(Action<TService> action, string endpointConfigName = null)
        {
            var factory = new ChannelFactory<TService>(endpointConfigName ?? typeof(TService).Name);
            var proxy = (IClientChannel)factory.CreateChannel();

            var success = false;
            try
            {
                action((TService)proxy);
                proxy.Close();
                success = true;
            }
            finally
            {
                if (!success)
                {
                    proxy.Abort();
                }
            }
        }

        public TReturn Use<TService, TReturn>(Func<TService, TReturn> action, string endpointConfigName = null)
        {
            var factory = new ChannelFactory<TService>(endpointConfigName ?? typeof(TService).Name);
            var proxy = (IClientChannel)factory.CreateChannel();

            bool success = false;
            try
            {
                var result = action((TService)proxy);
                proxy.Close();
                success = true;
                return result;
            }
            finally
            {
                if (!success)
                {
                    proxy.Abort();
                }
            }
        }

        public async Task<TReturn> UseAsync<TService, TReturn>(Func<TService, Task<TReturn>> asyncAction, string endpointConfigName = null)
        {
            var factory = new ChannelFactory<TService>(endpointConfigName ?? typeof(TService).Name);
            var proxy = (IClientChannel)factory.CreateChannel();

            bool success = false;
            try
            {
                var result = await asyncAction((TService)proxy);
                proxy.Close();
                success = true;
                return result;
            }
            finally
            {
                if (!success)
                {
                    proxy.Abort();
                }
            }
        }
    }
}