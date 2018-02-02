using DFC.Digital.Data.Interfaces;
using System;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core.Base
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfc", Justification = "Reviewed. Project name in correct spelling")]
    public class BaseDfcController : Controller
    {
        private readonly IApplicationLogger loggingService;

        public BaseDfcController(IApplicationLogger loggingService)
        {
            this.loggingService = loggingService;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;

            //Logging the Exception
            loggingService.Error($"Controller '{Convert.ToString(filterContext.RouteData.Values["controller"])}' Action : '{Convert.ToString(filterContext.RouteData.Values["action"])}' - failed with exception.", ex);
        }
    }
}