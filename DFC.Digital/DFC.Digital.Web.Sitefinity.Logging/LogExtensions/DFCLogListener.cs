using Autofac;
using DFC.Digital.Data.Interfaces;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;

namespace DFC.Digital.Web.Sitefinity.Logging
{
    public class DFCLogListener : CustomTraceListener
    {
        private const string SitefinityLogIdentifier = "Sitefinity-Log";
        private IApplicationLogger appLogger;

        public DFCLogListener()
        {
            var autofacContainer = ObjectFactory.Container.Resolve<ILifetimeScope>();
            this.appLogger = autofacContainer.Resolve<IApplicationLogger>();
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