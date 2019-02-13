using Autofac;
using Autofac.Extras.DynamicProxy;
using DFC.Digital.Core.Interceptors;

namespace DFC.Digital.Service.CourseSearchProvider
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CourseSearchProviderAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);
        }
    }
}