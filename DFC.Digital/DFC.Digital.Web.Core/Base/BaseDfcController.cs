using DFC.Digital.Core;
using System;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfc", Justification = "Reviewed. Project name in correct spelling")]
    public class BaseDfcController : Controller
    {
        public BaseDfcController(IApplicationLogger loggingService)
        {
            this.Log = loggingService;
        }

        protected IApplicationLogger Log { get; private set; }

        protected override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext?.Exception;

            //Logging the Exception
            Log.Error($"Controller '{Convert.ToString(filterContext?.RouteData.Values["controller"])}' Action : '{Convert.ToString(filterContext?.RouteData.Values["action"])}' - failed with exception.", ex);
        }
    }
}