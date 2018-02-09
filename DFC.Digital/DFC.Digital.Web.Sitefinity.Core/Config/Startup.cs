using Autofac;
using Autofac.Integration.Mvc;
using DFC.Digital.Web.Core.Config;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class Startup
    {
        public static void Install()
        {
            ObjectFactory.RegisteredIoCTypes += ObjectFactory_RegisteredIoCTypes;

            Bootstrapper.Initialized += Bootstrapper_Initialized;
            Bootstrapper.Bootstrapped += new EventHandler<EventArgs>(Bootstrapper_Bootstrapped);

            MvcHandler.DisableMvcResponseHeader = true;
        }

        private static void Bootstrapper_Bootstrapped(object sender, EventArgs e)
        {
            if (!Bootstrapper.IsAppRestarting)
            {
                ObjectFactory.Container.RegisterType<ISitefinityControllerFactory, AutofacContainerFactory>(new ContainerControlledLifetimeManager());

                var factory = ObjectFactory.Resolve<ISitefinityControllerFactory>();
                ControllerBuilder.Current.SetControllerFactory(factory);
            }

            FeatherActionInvokerCustom.Register();
        }

        private static void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName == "RegisterRoutes")
            {
                RegisterRoutes(RouteTable.Routes);
            }
        }

        private static void ObjectFactory_RegisteredIoCTypes(object sender, EventArgs e)
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

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");

            routes.MapRoute("govnotifyemail", "govnotifyemail/{controller}/{action}", new { controller = "VocSurvey", action = "SendEmail", id = string.Empty });

            routes.MapRoute("searchautocomplete", "searchautocomplete/{controller}/{action}", new { controller = "JobProfileSearchBox", action = "Suggestions", id = string.Empty });
        }
    }
}