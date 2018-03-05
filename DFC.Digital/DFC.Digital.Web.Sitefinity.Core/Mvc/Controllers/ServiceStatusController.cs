using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core.Base;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Core.Mvc.Models;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Core.Mvc.Controllers
{
    public class ServiceStatusController : Controller
    {
        #region private
        private readonly IEnumerable<DependencyHealthCheckService> dependencyHealth;
        private readonly IApplicationLogger applicationLogger;
        private readonly IWebAppContext webAppContext;
        #endregion

        #region Constructors
        public ServiceStatusController(IEnumerable<DependencyHealthCheckService> dependencyHealth, IApplicationLogger applicationLogger, IWebAppContext webAppContext)
        {
            this.dependencyHealth = dependencyHealth;
            this.applicationLogger = applicationLogger;
            this.webAppContext = webAppContext;
        }

        #endregion

        #region Actions
        public async Task<ActionResult> Index()
        {
            var serviceStatusModel = new ServiceStatusModel()
            {
                CheckDateTime = DateTime.Now,
                ServiceStatues = new List<ServiceStatus>()
            };

            foreach (DependencyHealthCheckService d in dependencyHealth)
            {
                serviceStatusModel.ServiceStatues.Add(await d.GetServiceStatus());
            }

            //if we have any state thats is not green
            if (serviceStatusModel.ServiceStatues.Any(s => s.Status != ServiceState.Green))
            {
                webAppContext.SetResponseStatusCode(502);
            }

            return View(serviceStatusModel);
        }
        #endregion
    }

}