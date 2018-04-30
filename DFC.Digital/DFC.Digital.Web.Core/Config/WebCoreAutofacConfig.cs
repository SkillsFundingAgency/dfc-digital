using Autofac;
using Autofac.Extras.NLog;
using Autofac.Integration.Mvc;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Repository.CosmosDb;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Service.AzureSearch;
using DFC.Digital.Service.Cognitive.BingSpellCheck.Config;
using DFC.Digital.Service.CourseSearchProvider;
using DFC.Digital.Service.GovUkNotify.Config;
using DFC.Digital.Service.LMIFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Hosting;

namespace DFC.Digital.Web.Core
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Autofac", Justification = "Reviewed. Product name in correct spelling.")]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class WebCoreAutofacConfig
    {
        public static IContainer BuildContainer(IContainer existingContainer)
        {
            var builder = new ContainerBuilder();

            RegisterCoreComponents(builder);

            if (existingContainer != null)
            {
                builder.Update(existingContainer);
                return existingContainer;
            }
            else
            {
                return builder.Build();
            }
        }

        private static void RegisterCoreComponents(ContainerBuilder builder)
        {
            builder.RegisterModule<NLogModule>();

            //Register modules cant use assembly scaning as they are not loaded on app domain yet
            builder
                .RegisterModule<CoreAutofacModule>()
                .RegisterModule<AzSearchAutofacModule>()
                .RegisterModule<CosmosDbAutofacModule>()
                .RegisterModule<SitefinityRepositoryAutofacModule>()
                .RegisterModule<GovUkNotifyAutofacModule>()
                .RegisterModule<LmiFeedAutofacModule>()
                .RegisterModule<CourseSearchProviderAutofacModule>()
                .RegisterModule<SpellCheckAutofacModule>();

            //Register defined modules from all DFC.Digital.Web assemblies
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (HostingEnvironment.InClientBuildManager)
            {
                assemblies = assemblies.Union(BuildManager.GetReferencedAssemblies().Cast<Assembly>())
                                       .Distinct();
            }

            var dfcDigitalAssemblies = assemblies.Where(a => a.FullName.StartsWith("DFC.Digital.Web", StringComparison.Ordinal)).ToArray();
            builder.RegisterAssemblyModules(dfcDigitalAssemblies);

            builder.RegisterAdapter<IServiceStatus, DependencyHealthCheckService>(service => new DependencyHealthCheckService(service));

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();
        }
    }
}