using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Core;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly).InstancePerLifetimeScope();
            builder.RegisterType<WebAppContext>().As<IWebAppContext>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<SitefinityCurrentContext>().As<ISitefinityCurrentContext>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<SitefinityPage>().As<ISitefinityPage>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<JobProfilePageContentService>().As<IJobProfilePage>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<HttpClientService<IAssetLocationAndVersion>>().AsImplementedInterfaces()
            .SingleInstance()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<AssetLocationAndVersion>().As<IAssetLocationAndVersion>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<SitefinityManagerProxy>().As<ISitefinityManagerProxy>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<SitefinityDataEventProxy>().As<ISitefinityDataEventProxy>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<CompositePageBuilder>().As<ICompositePageBuilder>()
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
        }
    }
}