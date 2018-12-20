using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FluentAssertions;
using System;
using System.Linq;
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

        public SingleTermDerivativeSearchSteps(
            ITestOutputHelper outputHelper,
            ISearchService<JobProfileIndex> searchService,
            ISearchIndexConfig searchIndex,
            ISearchQueryService<JobProfileIndex> searchQueryService)
        {
            this.OutputHelper = outputHelper;
            this.searchService = searchService;
            this.searchIndex = searchIndex;
            this.searchQueryService = searchQueryService;
            asyncHelper = new AsyncHelper();
        }

        private ITestOutputHelper OutputHelper { get; set; }

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

        [Then(@"the result will contain more than 1 result and '(.*)' should be in the first page")]
        public void ThenTheResultWillContainMoreThanResultAndShouldBeInTheFirstPage(string jobProfile)
        {
            //Log results
            OutputHelper.WriteLine($"Expecte to see {jobProfile} in results, Number of results returned {results.Results.Count()} actual result {results.ToJson()}");
            results.Results.Count().Should().BeGreaterOrEqualTo(1);
            results.Results.Any(a => a.ResultItem.Title.Equals(jobProfile, StringComparison.OrdinalIgnoreCase)).Should().BeTrue();
        }
    }
}