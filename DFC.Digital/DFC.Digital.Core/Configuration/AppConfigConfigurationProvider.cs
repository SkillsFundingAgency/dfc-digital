using System;
using System.Configuration;

namespace DFC.Digital.Core.Configuration
{
    public class AppConfigConfigurationProvider : IConfigurationProvider
    {
        public void Add<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public T GetConfig<T>(string key)
        {
            var value = ConfigurationManager.AppSettings.Get(key);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public T GetConfig<T>(string key, T defaultValue)
        {
            var value = ConfigurationManager.AppSettings.Get(key);
            return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
        }
    }
}