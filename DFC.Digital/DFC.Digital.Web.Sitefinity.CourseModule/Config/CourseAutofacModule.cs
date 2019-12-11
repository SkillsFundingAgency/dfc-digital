using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using DFC.Digital.Core.Interceptors;
using DFC.FindACourseClient;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    [ExcludeFromCodeCoverage]
    public class CourseAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            // Note that ASP.NET MVC requests controllers by their concrete types,
            // so registering them As<IController>() is incorrect.
            // Also, if you register controllers manually and choose to specify
            // lifetimes, you must register them as InstancePerDependency() or
            // InstancePerHttpRequest() - ASP.NET MVC will throw an exception if
            // you try to reuse a controller instance for multiple requests.
            builder.RegisterControllers(ThisAssembly)
                   .InstancePerRequest()

                   //.EnableClassInterceptors()
                   ;

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(ThisAssembly);
            builder.RegisterModelBinderProvider();
            builder.RegisterFindACourse();
        }
    }
}