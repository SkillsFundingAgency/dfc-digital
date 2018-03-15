using Autofac;
using Autofac.Extras.DynamicProxy2;
using DFC.Digital.Core.Interceptors;
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
            builder.RegisterGeneric(typeof(PreSearchFiltersRepository<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterGeneric(typeof(PreSearchFilterConverter<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            /*
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
                */
        }

        private static void RegisterDynamicModuleRepository(ContainerBuilder builder)
        {
            /* builder.RegisterGeneric(typeof(DynamicModuleRepository<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                .OnActivating(t => InitialiseDynamicModuleRepository(t.Instance))
                ;
                */

            builder.RegisterType<DynamicModuleRepository<JobProfile>>()
                .As<IDynamicModuleRepository<JobProfile>>()
                .InstancePerLifetimeScope()
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.JobprofileContentType, DynamicTypes.JobProfileModuleName))
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<DynamicModuleRepository<SocCode>>()
                .As<IDynamicModuleRepository<SocCode>>()
                .InstancePerLifetimeScope()
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.JobProfileSocContentType, DynamicTypes.JobProfileModuleName))
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<DynamicModuleRepository<ApprenticeVacancy>>()
                .As<IDynamicModuleRepository<ApprenticeVacancy>>()
                .InstancePerLifetimeScope()
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.JobProfileApprenticeshipContentType, DynamicTypes.JobProfileModuleName))
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<DynamicModuleRepository<PsfInterest>>()
                .As<IDynamicModuleRepository<PsfInterest>>()
                .InstancePerLifetimeScope()
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.InterestContentType, DynamicTypes.PreSearchFiltersModuleName))
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<DynamicModuleRepository<PsfEntryQualification>>()
                .As<IDynamicModuleRepository<PsfEntryQualification>>()
                .InstancePerLifetimeScope()
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.EntryQualificationContentType, DynamicTypes.PreSearchFiltersModuleName))
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<DynamicModuleRepository<PsfEnabler>>()
                .As<IDynamicModuleRepository<PsfEnabler>>()
                .InstancePerLifetimeScope()
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.EnablersContentType, DynamicTypes.PreSearchFiltersModuleName))
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<DynamicModuleRepository<PsfTrainingRoute>>()
                .As<IDynamicModuleRepository<PsfTrainingRoute>>()
                .InstancePerLifetimeScope()
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.TrainingRouteContentType, DynamicTypes.PreSearchFiltersModuleName))
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<DynamicModuleRepository<PsfJobArea>>()
                .As<IDynamicModuleRepository<PsfJobArea>>()
                .InstancePerLifetimeScope()
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.JobAreaContentType, DynamicTypes.PreSearchFiltersModuleName))
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<DynamicModuleRepository<PsfCareerFocus>>()
                .As<IDynamicModuleRepository<PsfCareerFocus>>()
                .InstancePerLifetimeScope()
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.CareerFocusContentType, DynamicTypes.PreSearchFiltersModuleName))
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<DynamicModuleRepository<PsfPreferredTaskType>>()
                .As<IDynamicModuleRepository<PsfPreferredTaskType>>()
                .InstancePerLifetimeScope()
                .OnActivating(t => t.Instance.Initialise(DynamicTypes.PreferredTaskTypeContentType, DynamicTypes.PreSearchFiltersModuleName))
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
        }

        private static void InitialiseDynamicModuleRepository(object instance)
        {
            switch (instance)
            {
                case DynamicModuleRepository<JobProfile> jpRepo:
                    jpRepo.Initialise(DynamicTypes.JobprofileContentType, DynamicTypes.JobProfileModuleName);
                    break;

                case DynamicModuleRepository<SocCode> socRepo:
                    socRepo.Initialise(DynamicTypes.JobProfileSocContentType, DynamicTypes.JobProfileModuleName);
                    break;
                /*
            case DynamicModuleRepository<JobProfile> jpRepo:
                jpRepo.Initialise(DynamicTypes.JobprofileContentType, DynamicTypes.JobProfileModuleName);
                break;

            case DynamicModuleRepository<JobProfile> jpRepo:
                jpRepo.Initialise(DynamicTypes.JobprofileContentType, DynamicTypes.JobProfileModuleName);
                break;

            case DynamicModuleRepository<JobProfile> jpRepo:
                jpRepo.Initialise(DynamicTypes.JobprofileContentType, DynamicTypes.JobProfileModuleName);
                break;

            case DynamicModuleRepository<JobProfile> jpRepo:
                jpRepo.Initialise(DynamicTypes.JobprofileContentType, DynamicTypes.JobProfileModuleName);
                break;

            case DynamicModuleRepository<JobProfile> jpRepo:
                jpRepo.Initialise(DynamicTypes.JobprofileContentType, DynamicTypes.JobProfileModuleName);
                break;

            case DynamicModuleRepository<JobProfile> jpRepo:
                jpRepo.Initialise(DynamicTypes.JobprofileContentType, DynamicTypes.JobProfileModuleName);
                break;

            case DynamicModuleRepository<JobProfile> jpRepo:
                jpRepo.Initialise(DynamicTypes.JobprofileContentType, DynamicTypes.JobProfileModuleName);
                break;

            case DynamicModuleRepository<JobProfile> jpRepo:
                jpRepo.Initialise(DynamicTypes.JobprofileContentType, DynamicTypes.JobProfileModuleName);
                break;
                */
                default:
                    break;
            }
        }
    }
}