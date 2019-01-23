using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Logging;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.SitemapGenerator.Abstractions.Events;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [ExcludeFromCodeCoverage]
    public sealed class Startup
    {
        private Startup()
        {
        }

        public static void Install()
        {
            ObjectFactory.RegisteredIoCTypes += ObjectFactory_RegisteredIoCTypes;
            Bootstrapper.Initialized += Bootstrapper_Initialized;
            Bootstrapper.Bootstrapped += Bootstrapper_Bootstrapped;

            MvcHandler.DisableMvcResponseHeader = true;
        }

        private static void Bootstrapper_Bootstrapped(object sender, EventArgs e)
        {
            try
            {
                if (!Bootstrapper.IsReady)
                {
                    ObjectFactory.Container.RegisterType<ISitefinityControllerFactory, AutofacContainerFactory>(new ContainerControlledLifetimeManager());

                    var factory = ObjectFactory.Resolve<ISitefinityControllerFactory>();
                    ControllerBuilder.Current.SetControllerFactory(factory);
                }

                FeatherActionInvokerCustom.Register();
                EventHub.Subscribe<ISitemapGeneratorBeforeWriting>(BeforeWritingSitemap);
                GlobalConfiguration.Configure(WebApiConfig.Register);
            }
            catch (Exception ex)
            {
                Logger.Writer.Write($"Fatal error - Failed to register AutofacContainerFactory - Re-start sitefinity -{ex}");
            }
        }

        private static void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName == "RegisterRoutes")
            {
                RegisterRoutes(RouteTable.Routes);
            }
        }

        private static void BeforeWritingSitemap(ISitemapGeneratorBeforeWriting evt)
        {
            var autofacLifetimeScope = AutofacDependencyResolver.Current.RequestLifetimeScope;
            var sitemapHandler = autofacLifetimeScope.Resolve<SiteMapHandler>();

            // sets the collection of entries to modified collection
            evt.Entries = sitemapHandler.ManipulateSiteMap(evt.Entries.ToList());
        }

        private static void ObjectFactory_RegisteredIoCTypes(object sender, EventArgs e)
        {
            try
            {
                IContainer existingAutofacContainer = null;
                try
                {
                    if (ObjectFactory.Container.IsRegistered<IContainer>())
                    {
                        existingAutofacContainer = ObjectFactory.Container.Resolve<IContainer>();
                    }
                }
                catch (ResolutionFailedException)
                {
                    //Sitefinity has not got an existing autofac container registered to its bootstrapper unity container.
                }

                var autofacContainer = WebCoreAutofacConfig.BuildContainer(existingAutofacContainer);

                ObjectFactory.Container.RegisterInstance(autofacContainer);
                DependencyResolver.SetResolver(new AutofacDependencyResolver(autofacContainer));
                GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(autofacContainer);

                //Application lifetime scope
                ObjectFactory.Container.RegisterInstance(autofacContainer.BeginLifetimeScope());
            }
            catch (Exception ex)
            {
                Logger.Writer.Write($"Fatal error - Failed to register AutofacContainer - Re-start sitefinity -{ex}");
            }
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");

            routes.MapRoute("govnotifyemail", "govnotifyemail/{controller}/{action}", new { controller = "VocSurvey", action = "SendEmail", id = string.Empty });
            routes.MapRoute("searchautocomplete", "searchautocomplete/{controller}/{action}", new { controller = "JobProfileSearchBox", action = "Suggestions", id = string.Empty });

            routes.MapRoute("serviceStatus", "health/{controller}/{action}", new { controller = "ServiceStatus", action = "Index", id = string.Empty });
            routes.MapRoute("restartsitefinity", "restartsitefinity/{controller}/{action}", new { controller = "AdminPanel", action = "RestartSitefinity", id = string.Empty });
        }
    }
}