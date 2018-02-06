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
            builder.Register(ctx => TaxonomyManager.GetManager()).As<ITaxonomyManager>();
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;

            builder.RegisterType<PreSearchFiltersRepository<PSFInterest>>().As<IPreSearchFiltersRepository<PSFInterest>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PSFEntryQualification>>().As<IPreSearchFiltersRepository<PSFEntryQualification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PSFEnabler>>().As<IPreSearchFiltersRepository<PSFEnabler>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PSFTrainingRoute>>().As<IPreSearchFiltersRepository<PSFTrainingRoute>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PSFJobArea>>().As<IPreSearchFiltersRepository<PSFJobArea>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PSFCareerFocus>>().As<IPreSearchFiltersRepository<PSFCareerFocus>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PSFPreferredTaskType>>().As<IPreSearchFiltersRepository<PSFPreferredTaskType>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;

            builder.RegisterType<PreSearchFilterConverter<PSFInterest>>().As<IDynamicModuleConverter<PSFInterest>>()
               .EnableInterfaceInterceptors()
               .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
               ;
            builder.RegisterType<PreSearchFilterConverter<PSFEntryQualification>>().As<IDynamicModuleConverter<PSFEntryQualification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFilterConverter<PSFEnabler>>().As<IDynamicModuleConverter<PSFEnabler>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFilterConverter<PSFTrainingRoute>>().As<IDynamicModuleConverter<PSFTrainingRoute>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFilterConverter<PSFJobArea>>().As<IDynamicModuleConverter<PSFJobArea>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFilterConverter<PSFCareerFocus>>().As<IDynamicModuleConverter<PSFCareerFocus>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
            builder.RegisterType<PreSearchFilterConverter<PSFPreferredTaskType>>().As<IDynamicModuleConverter<PSFPreferredTaskType>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
        }
    }
}