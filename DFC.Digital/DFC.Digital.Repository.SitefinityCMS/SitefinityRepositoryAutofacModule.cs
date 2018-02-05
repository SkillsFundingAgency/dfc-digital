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
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<PreSearchFiltersRepository<PsfInterest>>().As<IPreSearchFiltersRepository<PsfInterest>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PsfEntryQualification>>().As<IPreSearchFiltersRepository<PsfEntryQualification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PsfEnabler>>().As<IPreSearchFiltersRepository<PsfEnabler>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PsfTrainingRoute>>().As<IPreSearchFiltersRepository<PsfTrainingRoute>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PsfJobArea>>().As<IPreSearchFiltersRepository<PsfJobArea>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PsfCareerFocus>>().As<IPreSearchFiltersRepository<PsfCareerFocus>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFiltersRepository<PsfPreferredTaskType>>().As<IPreSearchFiltersRepository<PsfPreferredTaskType>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<PreSearchFilterConverter<PsfInterest>>().As<IDynamicModuleConverter<PsfInterest>>()
               .EnableInterfaceInterceptors()
               .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
               ;
            builder.RegisterType<PreSearchFilterConverter<PsfEntryQualification>>().As<IDynamicModuleConverter<PsfEntryQualification>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFilterConverter<PsfEnabler>>().As<IDynamicModuleConverter<PsfEnabler>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFilterConverter<PsfTrainingRoute>>().As<IDynamicModuleConverter<PsfTrainingRoute>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFilterConverter<PsfJobArea>>().As<IDynamicModuleConverter<PsfJobArea>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFilterConverter<PsfCareerFocus>>().As<IDynamicModuleConverter<PsfCareerFocus>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<PreSearchFilterConverter<PsfPreferredTaskType>>().As<IDynamicModuleConverter<PsfPreferredTaskType>>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
        }
    }
}