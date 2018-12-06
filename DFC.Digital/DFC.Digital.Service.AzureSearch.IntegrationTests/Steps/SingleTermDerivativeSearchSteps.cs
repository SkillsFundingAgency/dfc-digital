using AutoMapper;
using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace DFC.Digital.Service.AzureSearch.IntegrationTests
{
    [Binding]
    public class SingleTermDerivativeSearchSteps
    {
        private SearchResult<JobProfileIndex> results;
        private ISearchService<JobProfileIndex> searchService;
        private ISearchIndexConfig searchIndex;
        private ISearchQueryService<JobProfileIndex> searchQueryService;
        private IAsyncHelper asyncHelper;

        public SingleTermDerivativeSearchSteps(ITestOutputHelper outputHelper, ISearchService<JobProfileIndex> searchService, ISearchIndexConfig searchIndex, ISearchQueryService<JobProfileIndex> searchQueryService)
        {
            this.OutputHelper = outputHelper;
            this.searchService = searchService;
            this.searchIndex = searchIndex;
            this.searchQueryService = searchQueryService;
            asyncHelper = new AsyncHelper();
        }

        private ITestOutputHelper OutputHelper { get; set; }

        [Given(@"there was atleast '(.*)' job profile which have a title '(.*)' exist:")]
        public void GivenThereWasAtleastJobProfileWhichHaveATitleExistAsync(int profileCount, string title)
        {
            try
            {
                asyncHelper.Synchronise(() => searchService.EnsureIndexAsync(searchIndex.Name));
                asyncHelper.Synchronise(() => searchService.PopulateIndexAsync(profileCount.CreateWithTitle(title)));
            }
            catch (Exception ex)
            {
                OutputHelper.WriteLine($"Exception in When:- {ex.ToString()}");
            }
        }

        [When(@"I search for search term '(.*)'")]
        public void WhenISearchForSearchTerm(string searchTerm)
        {
            OutputHelper.WriteLine($"The search term is '{searchTerm}'");
            try
            {
                results = searchQueryService.Search(searchTerm);
            }
            catch (Exception ex)
            {
                OutputHelper.WriteLine($"Exception in When:- {ex.ToString()}");
            }
        }

        [Then(@"the result will contain '(.*)' profile(s) greater than or equal to 1")]
        public void ThenTheResultWillContainProfilesGreaterThanOrEqualTo(int profileCount)
        {
            //Log results
            OutputHelper.WriteLine($"Number of results expected {profileCount}  number returned {results?.Results.Count()} actual result {results?.ToJson()}");
            results?.Results.Count().Should().BeGreaterOrEqualTo(profileCount);
        }
    }
}