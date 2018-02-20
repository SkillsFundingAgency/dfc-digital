using Autofac;
using Autofac.Extras.DynamicProxy2;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Web;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public class SitefinityRepositoryAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(ctx => TaxonomyManager.GetManager())
                .As<ITaxonomyManager>()
                .InstancePerLifetimeScope()
                ;

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;

            RegisterPreSearchFilters(builder);
        }

        private static void RegisterPreSearchFilters(ContainerBuilder builder)
        {
            builder.RegisterType<PreSearchFiltersRepository<PSFInterest>>()
                .As<IPreSearchFiltersRepository<PSFInterest>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;

            builder.RegisterType<PreSearchFiltersRepository<PSFEntryQualification>>()
                .As<IPreSearchFiltersRepository<PSFEntryQualification>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PSFEnabler>>()
                .As<IPreSearchFiltersRepository<PSFEnabler>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PSFTrainingRoute>>()
                .As<IPreSearchFiltersRepository<PSFTrainingRoute>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PSFJobArea>>()
                .As<IPreSearchFiltersRepository<PSFJobArea>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PSFCareerFocus>>()
                .As<IPreSearchFiltersRepository<PSFCareerFocus>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PSFPreferredTaskType>>()
                .As<IPreSearchFiltersRepository<PSFPreferredTaskType>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;

            builder.RegisterType<PreSearchFilterConverter<PSFInterest>>()
                .As<IDynamicModuleConverter<PSFInterest>>()
                .InstancePerLifetimeScope()
               .EnableInterfaceInterceptors()
               .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
               ;
            builder.RegisterType<PreSearchFilterConverter<PSFEntryQualification>>()
                .As<IDynamicModuleConverter<PSFEntryQualification>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFilterConverter<PSFEnabler>>()
                .As<IDynamicModuleConverter<PSFEnabler>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFilterConverter<PSFTrainingRoute>>()
                .As<IDynamicModuleConverter<PSFTrainingRoute>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFilterConverter<PSFJobArea>>()
                .As<IDynamicModuleConverter<PSFJobArea>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFilterConverter<PSFCareerFocus>>()
                .As<IDynamicModuleConverter<PSFCareerFocus>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFilterConverter<PSFPreferredTaskType>>()
                .As<IDynamicModuleConverter<PSFPreferredTaskType>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
        }
    }
}