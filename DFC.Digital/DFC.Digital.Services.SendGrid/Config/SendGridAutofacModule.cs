using Autofac;
using Autofac.Extras.DynamicProxy;
using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;
using SendGrid;
using System.Configuration;

namespace DFC.Digital.Services.SendGrid
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SendGridAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.Register(c => new SendGridClient(c.Resolve<IConfigurationProvider>().GetConfig<string>(Constants.SendGridApiKey))).As<ISendGridClient>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
        }
    }
}
