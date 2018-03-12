using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class ServiceStatusController : Controller
    {
        #region private
        private readonly IEnumerable<DependencyHealthCheckService> dependencyHealth;
        private readonly IWebAppContext webAppContext;
        #endregion

        #region Constructors
        public ServiceStatusController(IEnumerable<DependencyHealthCheckService> dependencyHealth, IWebAppContext webAppContext)
        {
            this.dependencyHealth = dependencyHealth;
            this.webAppContext = webAppContext;
        }

        #endregion

        #region Actions
        public async Task<ActionResult> Index()
        {
            var serviceStatusModel = new ServiceStatusModel()
            {
                CheckDateTime = DateTime.Now,
                ServiceStatues = new Collection<ServiceStatus>()
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