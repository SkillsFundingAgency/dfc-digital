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
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                return $"{Uri.UnescapeDataString(uri.Path)}.config";
            }
        }

        private static Configuration RawConfig
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

        private static AppSettingsSection Config => RawConfig.GetSection("dfc") as AppSettingsSection;

        internal static T Get<T>(string key)
        {
            return (T)Convert.ChangeType(Config.Settings[key].Value, typeof(T));
        }
    }
}