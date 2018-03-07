using System;
using System.Collections.Generic;

namespace DFC.Digital.Core.Configuration
{
    public class InMemoryConfigurationProvider : IConfigurationProvider
    {
        private Dictionary<string, object> configs = new Dictionary<string, object>();

        public void Add<T>(string key, T value)
        {
            configs.Add(key, value);
        }

        public T Get<T>(string key)
        {
            return (T)Convert.ChangeType(configs[key], typeof(T));
        }

        public T Get<T>(string key, T defaultValue)
        {
            return configs.ContainsKey(key) ? (T)Convert.ChangeType(configs[key], typeof(T)) : defaultValue;
        }
    }
}