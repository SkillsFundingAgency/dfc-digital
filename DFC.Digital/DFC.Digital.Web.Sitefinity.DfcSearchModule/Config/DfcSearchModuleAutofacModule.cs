using Autofac;
using Autofac.Extras.DynamicProxy2;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DfcSearchModuleAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<JobProfileIndexEnhancer>()
                .As<IJobProfileIndexEnhancer>()
                .InstancePerLifetimeScope()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);
        }
    }
}