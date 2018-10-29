using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.SitefinityTest.Utilities
{
    public class AppSettings
    {
        private static AppSettings appSettingsInstance = null;

        public static AppSettings GetAppSettings()
        {
            if (appSettingsInstance == null)
            {
                appSettingsInstance = new AppSettings();
            }
            return appSettingsInstance;
        }

        public string GetBrowser()
        {
            return ConfigurationManager.AppSettings["browser"];
        }

        public string GetBaseUrl()
        {
            return ConfigurationManager.AppSettings["rooturl"];
        }

    }
}
