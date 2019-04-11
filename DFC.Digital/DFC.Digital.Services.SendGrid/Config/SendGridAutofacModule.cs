using Autofac;
using Autofac.Extras.DynamicProxy;
using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;
using SendGrid;
using SendGrid.Helpers.Reliability;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Digital.Services.SendGrid
{
    [ExcludeFromCodeCoverage]
    public class SendGridAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.Register(c => new SendGridClient(new SendGridClientOptions
                {
                    ApiKey = c.Resolve<IConfigurationProvider>().GetConfig<string>(Constants.SendGridApiKey),

                    // Summary:
                    //     Initializes a new instance of the SendGrid.Helpers.Reliability.ReliabilitySettings
                    //     class.
                    //
                    // Parameters:
                    //   maximumNumberOfRetries:
                    //     The maximum number of retries to execute against when sending an HTTP Request
                    //     before throwing an exception
                    //
                    //   minimumBackoff:
                    //     The minimum amount of time to wait between between HTTP retries
                    //
                    //   maximumBackOff:
                    //     the maximum amount of time to wait between between HTTP retries
                    //
                    //   deltaBackOff:
                    //     the value that will be used to calculate a random delta in the exponential delay
                    //     between retries
                ReliabilitySettings = new ReliabilitySettings(
                        c.Resolve<IConfigurationProvider>().GetConfig(Constants.SendGridDefaultNumberOfRetries, 2),
                        TimeSpan.FromSeconds(c.Resolve<IConfigurationProvider>().GetConfig(Constants.SendGridDefaultMinimumBackOff, 2)),
                        TimeSpan.FromSeconds(c.Resolve<IConfigurationProvider>().GetConfig(Constants.SendGridDefaultMaximumBackOff, 3)),
                        TimeSpan.FromSeconds(c.Resolve<IConfigurationProvider>().GetConfig(Constants.SendGridDeltaBackOff, 3)))
                })).As<ISendGridClient>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
        }
    }
}
