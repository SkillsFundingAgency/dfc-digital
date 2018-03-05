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
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            RegisterPreSearchFilters(builder);
        }

        private static void RegisterPreSearchFilters(ContainerBuilder builder)
        {
            builder.RegisterType<PreSearchFiltersRepository<PsfInterest>>()
                .As<IPreSearchFiltersRepository<PsfInterest>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<PreSearchFiltersRepository<PsfEntryQualification>>()
                .As<IPreSearchFiltersRepository<PsfEntryQualification>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PsfEnabler>>()
                .As<IPreSearchFiltersRepository<PsfEnabler>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PsfTrainingRoute>>()
                .As<IPreSearchFiltersRepository<PsfTrainingRoute>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PsfJobArea>>()
                .As<IPreSearchFiltersRepository<PsfJobArea>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PsfCareerFocus>>()
                .As<IPreSearchFiltersRepository<PsfCareerFocus>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PsfPreferredTaskType>>()
                .As<IPreSearchFiltersRepository<PsfPreferredTaskType>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<PreSearchFilterConverter<PsfInterest>>()
                .As<IDynamicModuleConverter<PsfInterest>>()
                .InstancePerLifetimeScope()
               .EnableInterfaceInterceptors()
               .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
               ;
            builder.RegisterType<PreSearchFilterConverter<PsfEntryQualification>>()
                .As<IDynamicModuleConverter<PsfEntryQualification>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFilterConverter<PsfEnabler>>()
                .As<IDynamicModuleConverter<PsfEnabler>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFilterConverter<PsfTrainingRoute>>()
                .As<IDynamicModuleConverter<PsfTrainingRoute>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFilterConverter<PsfJobArea>>()
                .As<IDynamicModuleConverter<PsfJobArea>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFilterConverter<PsfCareerFocus>>()
                .As<IDynamicModuleConverter<PsfCareerFocus>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFilterConverter<PsfPreferredTaskType>>()
                .As<IDynamicModuleConverter<PsfPreferredTaskType>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
        }
    }
}