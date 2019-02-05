using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Logging;

namespace DFC.Digital.Web.Sitefinity.Logging
{
    public static class LogInstaller
    {
        public static void Install()
        {
            Log.Configuring += Log_Configuring;
        }

        private static void Log_Configuring(object sender, LogConfiguringEventArgs e)
        {
            //ObjectFactory.Container.RegisterInstance<ISitefinityLogCategoryConfigurator>(new DfcConfigurator());
        }
    }
}