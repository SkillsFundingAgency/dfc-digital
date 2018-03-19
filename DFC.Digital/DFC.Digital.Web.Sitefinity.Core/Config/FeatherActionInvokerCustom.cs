using Autofac;
using DFC.Digital.Core;
using System.Diagnostics;
using System.Threading;
using System.Web;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Frontend.Mvc.Helpers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Routing;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Mvc.Proxy;

namespace DFC.Digital.Web.Sitefinity.Core
{
    /// <summary>
    /// Used to bubble the exception to application level when Feather widget controls run into an exception
    /// </summary>
    /// <seealso cref="Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Routing.FeatherActionInvoker" />
    public class FeatherActionInvokerCustom : FeatherActionInvoker
    {
        private readonly IApplicationLogger logger;
        private ILifetimeScope lifetimeScope;

        public FeatherActionInvokerCustom(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
            this.logger = lifetimeScope.Resolve<IApplicationLogger>();
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        internal static void Register()
        {
            ObjectFactory.Container.RegisterType<IControllerActionInvoker, FeatherActionInvokerCustom>();
        }

        protected override void ExecuteController(MvcProxyBase proxyControl)
        {
            Stopwatch watch = Stopwatch.StartNew();
            logger.Trace($"Executing controller {proxyControl?.ControllerName}");
            try
            {
                base.ExecuteController(proxyControl);
            }
            finally
            {
                watch.Stop();
                logger.Trace($"Completed executing controller {proxyControl?.ControllerName} and spent {watch.Elapsed} on it.");
            }
        }

        /// <summary>
        /// Handles the exception that occurred when executing the controller.
        /// </summary>
        /// <param name="err">The exception.</param>
        protected override void HandleControllerException(System.Exception err)
        {
            if (SitefinityContext.IsBackend)
            {
                base.HandleControllerException(err);
            }
            else
            {
                // Propagate Exceptions to Application Level
                if (!(err is ThreadAbortException))
                {
                    throw err;
                }
            }
        }

        protected override void RestoreHttpContext(string output, HttpContext initialContext)
        {
            if (!SitefinityContext.IsBackend)
            {
                SetHttpStatusCumulative(initialContext);
            }

            base.RestoreHttpContext(output, initialContext);
        }

        private static void SetHttpStatusCumulative(HttpContext initialContext)
        {
            if (initialContext.Response.StatusCode < HttpContext.Current.Response.StatusCode)
            {
                initialContext.Response.Status = HttpContext.Current.Response.Status;
                initialContext.Response.StatusCode = HttpContext.Current.Response.StatusCode;
                initialContext.Response.StatusDescription = HttpContext.Current.Response.StatusDescription;
            }
        }
    }
}