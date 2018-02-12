using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core.Base;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "Service Status", Title = "Checks and displays the status of services intergrated with this application", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class ServiceStatusController : Controller
    {
        #region private
        private IServiceStatus serviceStatus;
        private IWebAppContext webAppContext;
        private IApplicationLogger applicationLogger;

        #endregion

        #region Constructors
        public ServiceStatusController(IServiceStatus serviceStatus, IWebAppContext webAppContext, IApplicationLogger applicationLogger)
        {
            this.serviceStatus = serviceStatus;
            this.webAppContext = webAppContext;
            this.applicationLogger = applicationLogger;
        }
        #endregion

        #region Actions
        public ActionResult Index()
        {
            return View();
        }
        #endregion
    }
}