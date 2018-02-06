using AutoMapper;
using DFC.Digital.Automation.Test.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Linq;
using DFC.Digital.Core.Extensions;
using FluentAssertions;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace DFC.Digital.Service.AzureSearch.IntegrationTests.Steps
{
    [Binding]
    public class JobProfilesByPreSearchFiltersSteps
    {
        private SearchResult<JobProfileIndex> results;

        private ITestOutputHelper outputHelper { get; set; }

        private ISearchService<JobProfileIndex> searchService;
        private ISearchIndexConfig searchIndex;
        private IMapper mapper;

        public ISearchQueryService<JobProfileIndex> searchQueryService { get; }

        public JobProfilesByPreSearchFiltersSteps(ITestOutputHelper outputHelper, ISearchService<JobProfileIndex> searchService, ISearchIndexConfig searchIndex, ISearchQueryService<JobProfileIndex> searchQueryService, IMapper mapper)
        {
            this.outputHelper = outputHelper;
            this.searchService = searchService;
            this.searchIndex = searchIndex;
            this.searchQueryService = searchQueryService;
            this.mapper = mapper;
        }

        [Given(@"Given I have the following profiles tagged with the following PSF tags")]
        public void GivenGivenIHaveTheFollowingProfilesTaggedWithTheFollowingPsfTags(Table table)
        {
            searchService.EnsureIndex(searchIndex.Name);
            searchService.PopulateIndex(table.ToJobProfileSearchIndex());
        }

        [When(@"I filter with the following PSF items")]
        public void WhenIFilterWithTheFollowingTags(Table table)
        {
            outputHelper.WriteLine($"The filter term is '{table}'");
            SearchProperties filterProperties = table.ToSearchProperties();
            try
            {
                results = searchQueryService.Search("*", filterProperties);
            }
            catch (Exception ex)
            {
                outputHelper.WriteLine($"Exception in When:- {ex.ToString()}");
            }
        }

        [Then(@"the psf search profiles are listed in no specific order:")]
        public void ThenDisplayAllTheSearchResultsInNoSpecificOrderAsBelow(Table table)
        {
            var expected = table.ToJobProfileSearchIndex()?.Select(jp => jp.Title);
            var actual = results?.Results.Select(r => r.ResultItem.Title);

            //Log results
            outputHelper.WriteLine($"Expected {string.Join(",", expected)}");
            outputHelper.WriteLine($"Actual  {string.Join(",", actual)}");

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}