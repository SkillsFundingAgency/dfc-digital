using Autofac;
using DFC.Digital.Core;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;

namespace DFC.Digital.Web.Sitefinity.Logging
{
    public class DfcLogListener : CustomTraceListener
    {
        private const string SitefinityLogIdentifier = "Sitefinity-Log";
        private IApplicationLogger appLogger;

        public DfcLogListener()
        {
            var autofacApplicationLifetimeScope = ObjectFactory.Container.Resolve<ILifetimeScope>();

            //This instance of logger will only destroyed whe w3wp is recycled
            this.appLogger = autofacApplicationLifetimeScope.Resolve<IApplicationLogger>();
        }

        public override void Write(string message)
        {
            this.appLogger.Warn($"{SitefinityLogIdentifier}:- {message}", null);
        }

        public override void WriteLine(string message)
        {
            this.appLogger.Warn($"{SitefinityLogIdentifier}:- {message}", null);
        }
    }
}