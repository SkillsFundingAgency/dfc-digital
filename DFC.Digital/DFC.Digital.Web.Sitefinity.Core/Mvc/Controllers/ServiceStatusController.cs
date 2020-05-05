using AutoMapper;
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
        private readonly IMapper mapper;

        #endregion private

        #region Constructors

        public ServiceStatusController(IEnumerable<DependencyHealthCheckService> dependencyHealth, IWebAppContext webAppContext, IMapper mapper)
        {
            this.dependencyHealth = dependencyHealth;
            this.webAppContext = webAppContext;
            this.mapper = mapper;
        }

        #endregion Constructors

        #region Actions

        public async Task<ActionResult> Index()
        {
            var serviceStatusModel = new ServiceStatuesModel()
            {
                CheckDateTime = DateTime.Now,
                ServiceStatues = new Collection<ServiceStatusModel>()
            };

            foreach (DependencyHealthCheckService d in dependencyHealth)
            {
                var serviceStatus = await d.GetServiceStatus();
                serviceStatusModel.ServiceStatues.Add(mapper.Map<ServiceStatusModel>(serviceStatus));
            }

            //if we have any state thats is not green
            if (serviceStatusModel.ServiceStatues.Any(s => s.Status != ServiceState.Green))
            {
                webAppContext.SetResponseStatusCode(200);
            }

            return View(serviceStatusModel);
        }

        #endregion Actions
    }
}