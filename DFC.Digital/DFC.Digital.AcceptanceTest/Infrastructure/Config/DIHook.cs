using Autofac;
using BoDi;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    [Binding]
    public class DiHook
    {
        private IObjectContainer scenarioContainer;
        private IContainer autofacContainer;

        public DiHook(IObjectContainer scenarioContainer, IContainer autofacContainer)
        {
            this.scenarioContainer = scenarioContainer;
            this.autofacContainer = autofacContainer;
        }

        [BeforeScenario(Order = 10)]
        public void RegisterToDiReolvedFromAutofac()
        {
            var searchService = autofacContainer.Resolve<ISearchService<JobProfileIndex>>();
            var searchQueryService = autofacContainer.Resolve<ISearchQueryService<JobProfileIndex>>();
            var indexConfig = autofacContainer.Resolve<ISearchIndexConfig>();

            scenarioContainer.RegisterInstanceAs<ISearchService<JobProfileIndex>>(searchService);
            scenarioContainer.RegisterInstanceAs<ISearchQueryService<JobProfileIndex>>(searchQueryService);
            scenarioContainer.RegisterInstanceAs<ISearchIndexConfig>(indexConfig);
        }
    }
}