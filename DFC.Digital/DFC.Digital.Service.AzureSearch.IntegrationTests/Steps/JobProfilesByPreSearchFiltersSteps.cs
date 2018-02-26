using AutoMapper;
using DFC.Digital.Automation.Test.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FluentAssertions;
using System;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace DFC.Digital.Service.AzureSearch.IntegrationTests.Steps
{
    [Binding]
    public class JobProfilesByPreSearchFiltersSteps
    {
        private SearchResult<JobProfileIndex> results;
        private ISearchService<JobProfileIndex> searchService;
        private ISearchIndexConfig searchIndex;
        private IMapper mapper;

        public JobProfilesByPreSearchFiltersSteps(ITestOutputHelper outputHelper, ISearchService<JobProfileIndex> searchService, ISearchIndexConfig searchIndex, ISearchQueryService<JobProfileIndex> searchQueryService, IMapper mapper)
        {
            this.OutputHelper = outputHelper;
            this.searchService = searchService;
            this.searchIndex = searchIndex;
            this.SearchQueryService = searchQueryService;
            this.mapper = mapper;
        }

        private ISearchQueryService<JobProfileIndex> SearchQueryService { get; }

        private ITestOutputHelper OutputHelper { get; set; }

        [Given(@"Given I have the following profiles tagged with the following PSF tags")]
        public void GivenGivenIHaveTheFollowingProfilesTaggedWithTheFollowingPsfTags(Table table)
        {
            searchService.EnsureIndex(searchIndex.Name);
            searchService.PopulateIndex(table.ToJobProfileSearchIndex());
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

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}