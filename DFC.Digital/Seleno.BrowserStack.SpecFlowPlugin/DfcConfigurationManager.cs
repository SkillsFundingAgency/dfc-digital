using System;
using System.Configuration;
using System.Reflection;

namespace Seleno.BrowserStack
{
    internal static class DfcConfigurationManager
    {
        private static Configuration configuration;

        private static string ConfigFilePath
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                return $"{Uri.UnescapeDataString(uri.Path)}.config";
            }
        }

        internal static Configuration RawConfig
        {
            get
            {
                if (configuration == null)
                {
                    var configFileMap = new ConfigurationFileMap(ConfigFilePath);
                    configuration = ConfigurationManager.OpenMappedMachineConfiguration(configFileMap);
                }

                return configuration;
            }
        }

        internal static AppSettingsSection Config
        {
            get
            {
                return RawConfig.GetSection("dfc") as AppSettingsSection;
            }
        }

        internal static T Get<T>(string key)
        {
            return (T)Convert.ChangeType(Config.Settings[key].Value, typeof(T));
        }
    }
}