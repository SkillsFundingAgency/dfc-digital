using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;

namespace DFC.Digital.MigrationTool.App_Start
{
    public class AutofacConfig
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            //builder.RegisterType<JobProfileRepository>()
            //     .UsingConstructor(typeof(IDynamicModuleRepository<JobProfile>), 
            //                        typeof(IDynamicModuleConverter<JobProfile>),
            //                        typeof(IDynamicContentExtensions),
            //                        typeof(IDynamicModuleRepository<SocSkillMatrix>),
            //                        typeof(IDynamicModuleConverter<JobProfileOverloadForWhatItTakes>),
            //                        typeof(IDynamicModuleConverter<JobProfileOverloadForSearch>));

            //builder.RegisterGeneric(typeof(IJobProfileRepository))
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();

            //RegisterComponents(builder);

            return builder.Build();
        }

        private static void RegisterComponents(ContainerBuilder builder)
        {
            //builder.RegisterModule<NLogModule>();

            //Register modules cant use assembly scaning as they are not loaded on app domain yet
            builder.RegisterModule<MigrationToolModule>();
            //.RegisterModule<AzSearchAutofacModule>()
            //.RegisterModule<CosmosDbAutofacModule>()
            //.RegisterModule<SitefinityRepositoryAutofacModule>()
            //.RegisterModule<GovUkNotifyAutofacModule>()
            //.RegisterModule<LmiFeedAutofacModule>()
            //.RegisterModule<CourseSearchProviderAutofacModule>()
            //.RegisterModule<SpellCheckAutofacModule>()
            //.RegisterModule<SkillsFrameworkEngineAutofacModule>()
            //.RegisterModule<SkillsFrameworkAutofacModule>()
            //.RegisterModule<SendGridAutofacModule>()
            //.RegisterModule<CUIHealthStatusChecksAutofacModule>();



            ////Register defined modules from all DFC.Digital.Web assemblies
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (HostingEnvironment.InClientBuildManager)
            {
                assemblies = assemblies.Union(BuildManager.GetReferencedAssemblies().Cast<Assembly>())
                                       .Distinct();
            }

            var dfcDigitalAssemblies = assemblies.Where(a => a.FullName.StartsWith("DFC.Digital.Web", StringComparison.Ordinal)).ToArray();
            builder.RegisterAssemblyModules(dfcDigitalAssemblies);

            builder.RegisterAdapter<IServiceStatus, DependencyHealthCheckService>(service => new DependencyHealthCheckService(service));

            //// OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            //// OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            //// OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            ////Register automapper globally
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
            })).AsSelf().SingleInstance();

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().InstancePerLifetimeScope();
        }
    }
}