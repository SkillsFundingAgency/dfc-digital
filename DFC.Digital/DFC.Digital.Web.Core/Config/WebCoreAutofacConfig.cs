using Autofac;
using Autofac.Extras.NLog;
using Autofac.Integration.Mvc;
using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Repository.CosmosDb;
using DFC.Digital.Repository.ONET;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Service.AzureSearch;
using DFC.Digital.Service.Cognitive.BingSpellCheck.Config;
using DFC.Digital.Service.CourseSearchProvider;
using DFC.Digital.Service.CUIStatusChecks.Config;
using DFC.Digital.Service.GovUkNotify.Config;
using DFC.Digital.Service.LMIFeed;
using DFC.Digital.Service.SkillsFramework;
using DFC.Digital.Services.SendGrid;
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
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            RegisterCoreComponents(builder);

            return builder.Build();
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
                .RegisterModule<SpellCheckAutofacModule>()
                .RegisterModule<SkillsFrameworkEngineAutofacModule>()
                .RegisterModule<SkillsFrameworkAutofacModule>()
                .RegisterModule<SendGridAutofacModule>()
                .RegisterModule<CUIHealthStatusChecksAutofacModule>();

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

            //Register automapper globally
            RegisterAutomapper(builder, assemblies);
        }

        private static void RegisterAutomapper(ContainerBuilder builder, IEnumerable<Assembly> assemblies)
        {
            //Register automapper
            builder.RegisterAssemblyTypes(assemblies.ToArray())
            .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
            .As<Profile>().SingleInstance();

            //Autofact config
            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                foreach (var profile in ctx.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().InstancePerLifetimeScope();
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().SingleInstance();
        }
    }
}