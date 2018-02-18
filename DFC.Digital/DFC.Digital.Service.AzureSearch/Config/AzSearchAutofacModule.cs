using Autofac;
using Autofac.Extras.DynamicProxy2;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Core.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Microsoft.Azure.Search;
using System.Configuration;

namespace DFC.Digital.Service.AzureSearch
{
    public class AzSearchAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(CreateSearchServiceClient).InstancePerLifetimeScope()
                .InstancePerLifetimeScope()
                ;

            builder.Register(CreateSearchIndexClient)
                .InstancePerLifetimeScope()
                ;

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;

            builder.RegisterType<AzSearchService<JobProfileIndex>>().As<ISearchService<JobProfileIndex>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;

            builder.RegisterType<DfcSearchQueryService<JobProfileIndex>>().As<ISearchQueryService<JobProfileIndex>>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME)
                ;
        }

        private ISearchServiceClient CreateSearchServiceClient(IComponentContext arg)
        {
            string searchServiceName = ConfigurationManager.AppSettings[Constants.KeysSearchServiceName];
            string adminApiKey = ConfigurationManager.AppSettings[Constants.KeysSearchServiceAdminAPIKey];

            return new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
        }

        private ISearchIndexClient CreateSearchIndexClient(IComponentContext arg)
        {
            string searchServiceName = ConfigurationManager.AppSettings[Constants.KeysSearchServiceName];
            string queryApiKey = ConfigurationManager.AppSettings[Constants.KeysSearchServiceAdminAPIKey];

            return new SearchIndexClient(searchServiceName, arg.Resolve<ISearchIndexConfig>().Name, new SearchCredentials(queryApiKey));
        }
    }
}