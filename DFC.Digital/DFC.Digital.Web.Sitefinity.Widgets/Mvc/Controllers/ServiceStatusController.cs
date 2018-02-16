using DFC.Digital.Data.Interfaces;
using DFC.Digital.Service.ServiceStatusesToCheck;
using DFC.Digital.Web.Core.Base;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "ServiceStatus", Title = "Check the status of services", SectionName = SitefinityConstants.CustomWidgetSection)]

    public class ServiceStatusController : Controller
    {
        #region private
        private readonly IEnumerable<DependencyHealthCheckService> dependencyHealth;
        private readonly IWebAppContext webAppContext;
        private readonly IApplicationLogger applicationLogger;

        #endregion

        #region Constructors
        public ServiceStatusController(IEnumerable<DependencyHealthCheckService> dependencyHealth, IWebAppContext webAppContext, IApplicationLogger applicationLogger)
        {
            this.dependencyHealth = dependencyHealth;
            this.webAppContext = webAppContext;
            this.applicationLogger = applicationLogger;
        }

        #endregion

        #region Actions
        public ActionResult Index()
        {
            var serviceStatusModel = new ServiceStatusModel()
            {
                CheckDateTime = DateTime.Now,
                ServiceStatues = dependencyHealth.Select(d => d.Status),
            };

            return View(serviceStatusModel);
        }
        #endregion
    }
}