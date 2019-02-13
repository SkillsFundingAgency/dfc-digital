using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using DFC.Digital.Core.Interceptors;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class WebApiAutofacModule : Module
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
            builder.RegisterApiControllers(ThisAssembly)
                   .InstancePerRequest();

            //.EnableClassInterceptors()
            //.InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(ThisAssembly);
            builder.RegisterModelBinderProvider();

            //builder.RegisterWebApiFilterProvider();
            builder.RegisterWebApiModelBinderProvider();
            builder.RegisterWebApiModelBinders();
        }
    }
}