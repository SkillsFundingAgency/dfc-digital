using Autofac;
using Autofac.Integration.Mvc;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Logging;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.SitemapGenerator.Abstractions.Events;
using Telerik.Sitefinity.SitemapGenerator.Data;

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
                if (!Bootstrapper.IsAppRestarting)
                {
                    ObjectFactory.Container.RegisterType<ISitefinityControllerFactory, AutofacContainerFactory>(new ContainerControlledLifetimeManager());

                    var factory = ObjectFactory.Resolve<ISitefinityControllerFactory>();
                    ControllerBuilder.Current.SetControllerFactory(factory);
                }

                FeatherActionInvokerCustom.Register();
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

            if (e.CommandName == "Bootstrapped")
            {
                EventHub.Subscribe<ISitemapGeneratorBeforeWriting>(BeforeWritingSitemap);
            }
        }

        private static void BeforeWritingSitemap(ISitemapGeneratorBeforeWriting evt)
        {
            // gets the entries that are about to be written in the sitemap
            var entries = evt.Entries.ToList();

            var jobCategoryPageEntry =
                entries.FirstOrDefault(x => x.Location.ToUpperInvariant().EndsWith("/JOB-CATEGORIES"));

            if (jobCategoryPageEntry != null)
            {
                //Add categories
                var autofacContainer = ObjectFactory.Container.Resolve<ILifetimeScope>();
                var repository = autofacContainer.Resolve<IJobProfileCategoryRepository>();
                var cats = repository.GetJobProfileCategories();

                foreach (var category in cats)
                {
                    // adds the new sitemap entry to the collection of the entries
                    entries.Add(new SitemapEntry
                    {
                        Location = $"{jobCategoryPageEntry.Location}/{category.Url}",
                        Priority = (float)0.5
                    });
                }
            }

            //Clean up
            entries.RemoveAll(x => x.Location.ToUpperInvariant().Contains("/ALERTS/")
                                || x.Location.ToUpperInvariant().EndsWith("/JOB-CATEGORIES")
                                || x.Location.ToUpperInvariant().EndsWith("/JOB-PROFILES"));

            // sets the collection of entries to modified collection
            evt.Entries = entries.OrderBy(x => x.Location);
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