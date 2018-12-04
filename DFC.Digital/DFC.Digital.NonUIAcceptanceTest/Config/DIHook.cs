using Autofac;
using AutoMapper;
using BoDi;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.AzureSearch;
using TechTalk.SpecFlow;

namespace DFC.Digital.NonUIAcceptanceTest
{
    [Binding]
    public class DiHook
    {
        private IObjectContainer scenarioContainer;
        private ILifetimeScope autofac;

        public DiHook(IObjectContainer scenarioContainer)
        {
            this.scenarioContainer = scenarioContainer;

            var builder = new Autofac.ContainerBuilder();
            builder.RegisterModule<AzSearchAutofacModule>();
            builder.RegisterModule<AutofacAcceptanceTestModule>();

            this.autofac = builder.Build().BeginLifetimeScope();
        }

        [BeforeScenario(Order = 10)]
        public void RegisterToDiReolvedFromAutofac()
        {          
            if (!scenarioContainer.IsRegistered<ISearchQueryService<JobProfileIndex>>())
            {
                scenarioContainer.RegisterInstanceAs(autofac.Resolve<ISearchQueryService<JobProfileIndex>>());
            }

            if (!scenarioContainer.IsRegistered<ISearchIndexConfig>())
            {
                scenarioContainer.RegisterInstanceAs(autofac.Resolve<ISearchIndexConfig>());
            }

            if (!scenarioContainer.IsRegistered<IMapper>())
            {
                scenarioContainer.RegisterInstanceAs(autofac.Resolve<IMapper>());
            }
         
        }
    }
}