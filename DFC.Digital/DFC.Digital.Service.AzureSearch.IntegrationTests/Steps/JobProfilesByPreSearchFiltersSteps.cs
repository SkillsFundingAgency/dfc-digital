using AutoMapper;
using DFC.Digital.Automation.Test.Utilities;
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
    public class JobProfilesByPreSearchFiltersSteps
    {
        private SearchResult<JobProfileIndex> results;
        private ISearchService<JobProfileIndex> searchService;
        private ISearchIndexConfig searchIndex;
        private IAsyncHelper asyncHelper;

        public JobProfilesByPreSearchFiltersSteps(ITestOutputHelper outputHelper, ISearchService<JobProfileIndex> searchService, ISearchIndexConfig searchIndex, ISearchQueryService<JobProfileIndex> searchQueryService)
        {
            this.OutputHelper = outputHelper;
            this.searchService = searchService;
            this.searchIndex = searchIndex;
            this.SearchQueryService = searchQueryService;
            asyncHelper = new AsyncHelper();
        }

        private ISearchQueryService<JobProfileIndex> SearchQueryService { get; }

        private ITestOutputHelper OutputHelper { get; set; }

        [Given(@"Given I have the following profiles tagged with the following PSF tags")]
        public void GivenGivenIHaveTheFollowingProfilesTaggedWithTheFollowingPsfTagsAsync(Table table)
        {
            asyncHelper.Synchronise(() => searchService.EnsureIndexAsync(searchIndex.Name));
            asyncHelper.Synchronise(() => searchService.PopulateIndexAsync(table.ToJobProfileSearchIndex()));
        }

        [When(@"I filter with the following PSF items")]
        public void WhenIFilterWithTheFollowingTags(Table table)
        {
            OutputHelper.WriteLine($"The filter term is '{table}'");
            SearchProperties filterProperties = table.ToSearchProperties();
            try
            {
                results = SearchQueryService.Search("*", filterProperties);
            }
            catch (Exception ex)
            {
                OutputHelper.WriteLine($"Exception in When:- {ex.ToString()}");
            }
        }

        [Then(@"the psf search profiles are listed in no specific order:")]
        public void ThenDisplayAllTheSearchResultsInNoSpecificOrderAsBelow(Table table)
        {
            var expected = table.ToJobProfileSearchIndex()?.Select(jp => jp.Title);
            var actual = results?.Results.Select(r => r.ResultItem.Title);

            //Log results
            OutputHelper.WriteLine($"Expected {string.Join(",", expected)}");
            OutputHelper.WriteLine($"Actual  {string.Join(",", actual)}");

            actual.Should().BeEquivalentTo(expected);
        }
    }
}