using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.Core.Mvc.Controllers
{
    public class ServiceStatusController : Controller
    {
        #region private

        private readonly IEnumerable<DependencyHealthCheckService> dependencyHealth;
        private readonly IWebAppContext webAppContext;

        #endregion private

        #region Constructors

        public ServiceStatusController(IEnumerable<DependencyHealthCheckService> dependencyHealth, IWebAppContext webAppContext)
        {
            this.dependencyHealth = dependencyHealth;
            this.webAppContext = webAppContext;
        }

        #endregion Constructors

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

        #endregion Actions
    }
}