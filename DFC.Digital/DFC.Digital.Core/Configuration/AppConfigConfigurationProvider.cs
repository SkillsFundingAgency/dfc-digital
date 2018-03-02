using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Core.Configuration
{
    public class AppConfigConfigurationProvider : IConfigurationProvider
    {
        public void Add<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            var value = ConfigurationManager.AppSettings.Get(key);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public T Get<T>(string key, T defaultValue)
        {
            var value = ConfigurationManager.AppSettings.Get(key);
            return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
