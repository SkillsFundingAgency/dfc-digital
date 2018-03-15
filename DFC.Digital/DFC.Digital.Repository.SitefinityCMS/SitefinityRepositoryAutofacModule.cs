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

            RegisterDynamicModuleRepository(builder);
            RegisterPreSearchFiltersRepository(builder);
        }

        private static void RegisterPreSearchFiltersRepository(ContainerBuilder builder)
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

        private static void RegisterDynamicModuleRepository(ContainerBuilder builder)
        {
            builder.RegisterType<DynamicModuleRepository<JobProfile>>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.JobprofileContentType, DynamicTypes.JobProfileModuleName))
                ;

            builder.RegisterType<DynamicModuleRepository<IJobProfileSocCodeRepository>>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.JobProfileSocContentType, DynamicTypes.JobProfileModuleName))
                ;

            builder.RegisterType<DynamicModuleRepository<ApprenticeVacancy>>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.JobProfileApprenticeshipContentType, DynamicTypes.JobProfileModuleName))
                ;

            builder.RegisterType<DynamicModuleRepository<PsfInterest>>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.InterestContentType, DynamicTypes.PreSearchFiltersModuleName))
                ;

            builder.RegisterType<DynamicModuleRepository<PsfEntryQualification>>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.EntryQualificationContentType, DynamicTypes.PreSearchFiltersModuleName))
                ;

            builder.RegisterType<DynamicModuleRepository<PsfEnabler>>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.EnablersContentType, DynamicTypes.PreSearchFiltersModuleName))
                ;

            builder.RegisterType<DynamicModuleRepository<PsfTrainingRoute>>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.TrainingRouteContentType, DynamicTypes.PreSearchFiltersModuleName))
                ;

            builder.RegisterType<DynamicModuleRepository<PsfJobArea>>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.JobAreaContentType, DynamicTypes.PreSearchFiltersModuleName))
                ;

            builder.RegisterType<DynamicModuleRepository<PsfCareerFocus>>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.CareerFocusContentType, DynamicTypes.PreSearchFiltersModuleName))
                ;

            builder.RegisterType<DynamicModuleRepository<PsfPreferredTaskType>>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.PreferredTaskTypeContentType, DynamicTypes.PreSearchFiltersModuleName))
                ;
        }
    }
}