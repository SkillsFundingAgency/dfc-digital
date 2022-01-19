using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.MigrationTool.App_Start;
using DFC.Digital.MigrationTool.Controllers;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;

namespace DFC.Digital.MigrationTool
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            AutofacConfig.BuildContainer();


            //ObjectFactory.RegisteredIoCTypes += ObjectFactory_RegisteredIoCTypes;

            var autofacContainer = WebCoreAutofacConfig.BuildContainer();

            ObjectFactory.Container.RegisterInstance(autofacContainer);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(autofacContainer));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(autofacContainer);

            //Application lifetime scope
            ObjectFactory.Container.RegisterInstance(autofacContainer.BeginLifetimeScope());

            /*
            //var autofacContainer = AutofacConfig.BuildContainer();
            var autofacContainer = WebCoreAutofacConfig.BuildContainer();
            
            ObjectFactory.Container.RegisterInstance(autofacContainer);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(autofacContainer));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(autofacContainer);

            //Application lifetime scope
            ObjectFactory.Container.RegisterInstance(autofacContainer.BeginLifetimeScope());
            */

        }

        private static void ObjectFactory_RegisteredIoCTypes(object sender, EventArgs e)
        {
            try
            {
                var autofacContainer = WebCoreAutofacConfig.BuildContainer();

                ObjectFactory.Container.RegisterInstance(autofacContainer);
                DependencyResolver.SetResolver(new AutofacDependencyResolver(autofacContainer));
                GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(autofacContainer);

                //Application lifetime scope
                ObjectFactory.Container.RegisterInstance(autofacContainer.BeginLifetimeScope());
            }
            catch (Exception ex)
            {
                //Logger.Writer.Write($"Fatal error - Failed to register AutofacContainer - Re-start sitefinity -{ex}");
                string errorMessage = ex.Message;
            }
        }
    }
}
