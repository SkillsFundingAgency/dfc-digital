using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using DFC.Digital.Core;
using DFC.Digital.Web.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.HtmlControls;
using Telerik.Microsoft.Practices.EnterpriseLibrary.Logging;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.Data.Metadata;
using Telerik.Sitefinity.DynamicModules.Events;
using Telerik.Sitefinity.Metadata.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.SitemapGenerator.Abstractions.Events;
using Telerik.Sitefinity.Web.Events;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [ExcludeFromCodeCoverage]
    public sealed class Startup
    {
        private const string MetaAttrKey = "name";
        private const string MetaAttrValue = "Generator";

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
                ObjectFactory.Container.RegisterType<ISitefinityControllerFactory, AutofacContainerFactory>(new ContainerControlledLifetimeManager());

                var factory = ObjectFactory.Resolve<ISitefinityControllerFactory>();
                ControllerBuilder.Current.SetControllerFactory(factory);

                FeatherActionInvokerCustom.Register();

                EventHub.Subscribe<ISitemapGeneratorBeforeWriting>(BeforeWritingSitemap);
                EventHub.Subscribe<IDataEvent>(Content_Action);
                EventHub.Subscribe<IDynamicContentUpdatedEvent>(Dynamic_Content_Updated_Action);
                EventHub.Subscribe<IDynamicContentDeletingEvent>(Dynamic_Content_Deleteing_Action);

                EventHub.Subscribe<IPagePreRenderCompleteEvent>(OnPagePreRenderCompleteEventHandler);
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

        private static void Content_Action(IDataEvent eventInfo)
        {
            var autofacLifetimeScope = AutofacDependencyResolver.Current.RequestLifetimeScope;
            var dataEventHandler = autofacLifetimeScope.Resolve<DataEventProcessor>();
            dataEventHandler.ExportContentData(eventInfo);
        }

        // Event Action for Job Profile Dynamic content PUBLISH event
        private static void Dynamic_Content_Updated_Action(IDynamicContentUpdatedEvent updatedEventInfo)
        {
            var autofacLifetimeScope = AutofacDependencyResolver.Current.RequestLifetimeScope;
            var dataEventHandler = autofacLifetimeScope.Resolve<DataEventProcessor>();
            dataEventHandler.PublishDynamicContent(updatedEventInfo.Item, Constants.WorkflowStatusPublished);
        }

        private static void Dynamic_Content_Deleteing_Action(IDynamicContentDeletingEvent updatedEventInfo)
        {
            var autofacLifetimeScope = AutofacDependencyResolver.Current.RequestLifetimeScope;
            var dataEventHandler = autofacLifetimeScope.Resolve<DataEventProcessor>();
            dataEventHandler.PublishDynamicContent(updatedEventInfo.Item, Constants.ItemActionDeleted);
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
                var autofacContainer = WebCoreAutofacConfig.BuildContainer();

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

        private static void OnPagePreRenderCompleteEventHandler(IPagePreRenderCompleteEvent evt)
        {
            var page = evt.Page;
            if (page != null)
            {
                var headerControls = page.Header.Controls;
                foreach (var control in headerControls)
                {
                    if (control is HtmlMeta metaTag)
                    {
                        if (metaTag.Attributes[MetaAttrKey] == MetaAttrValue)
                        {
                            headerControls.Remove(metaTag);
                            break;
                        }
                    }
                }
            }
        }
    }
}