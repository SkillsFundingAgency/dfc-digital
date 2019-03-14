using Autofac;
using Autofac.Extras.DynamicProxy;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Web;

namespace DFC.Digital.Repository.SitefinityCMS
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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
        }

        private static void RegisterDynamicModuleRepository(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(DynamicModuleRepository<>))
                .As(typeof(IDynamicModuleRepository<>))
                .InstancePerLifetimeScope()
                .OnActivating(t =>
                {
                    switch (t)
                    {
                        case var _ when t.Instance is DynamicModuleRepository<JobProfile> instance:
                            instance.Initialise(DynamicTypes.JobprofileContentType, DynamicTypes.JobProfileModuleName);
                            break;

                        case var _ when t.Instance is DynamicModuleRepository<SocCode> instance:
                            instance.Initialise(DynamicTypes.JobProfileSocContentType, DynamicTypes.JobProfileModuleName);
                            break;

                        case var _ when t.Instance is DynamicModuleRepository<WhatItTakesSkill> instance:
                            instance.Initialise(DynamicTypes.JobProfileSkillsMatrixContentType, DynamicTypes.JobProfileModuleName);
                            break;

                        case var _ when t.Instance is DynamicModuleRepository<ApprenticeVacancy> instance:
                            instance.Initialise(DynamicTypes.JobProfileApprenticeshipContentType, DynamicTypes.JobProfileModuleName);
                            break;

                        case var _ when t.Instance is DynamicModuleRepository<FrameworkSkill> instance:
                            instance.Initialise(DynamicTypes.OnetSkillTypeContentType, DynamicTypes.JobProfileModuleName);
                            break;

                        case var _ when t.Instance is DynamicModuleRepository<SocSkillMatrix> instance:
                            instance.Initialise(DynamicTypes.SocSkillMatrixTypeContentType, DynamicTypes.JobProfileModuleName);
                            break;

                        case var _ when t.Instance is DynamicModuleRepository<EmailTemplate> instance:
                            instance.Initialise(DynamicTypes.EmailTemplateContentType, DynamicTypes.ConfigurationsModuleName);
                            break;
                    }
                })
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
        }
    }
}