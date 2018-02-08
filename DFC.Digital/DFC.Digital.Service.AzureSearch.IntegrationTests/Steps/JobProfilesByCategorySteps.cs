using AutoMapper;
using DFC.Digital.Automation.Test.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace DFC.Digital.Service.AzureSearch.IntegrationTests.Steps
{
    [Binding]
    internal class JobProfilesByCategorySteps
    {
        private IEnumerable<JobProfile> result;

        private ITestOutputHelper OutputHelper { get; set; }

        private ISearchService<JobProfileIndex> searchService;
        private ISearchIndexConfig searchIndex;
        private IMapper mapper;

        public JobProfilesByCategorySteps(ITestOutputHelper outputHelper, ISearchService<JobProfileIndex> searchService, ISearchIndexConfig searchIndex, ISearchQueryService<JobProfileIndex> searchQueryService, IMapper mapper)
        {
            this.OutputHelper = outputHelper;
            this.searchService = searchService;
            this.searchIndex = searchIndex;
            this.SearchQueryService = searchQueryService;
            this.mapper = mapper;
        }

        private ISearchQueryService<JobProfileIndex> SearchQueryService { get; }

        [Given(@"the following job profiles in catogories  exist:")]
        public void GivenTheFollowingJobProfilesInCatogoriesExist(Table table)
        {
            searchService.EnsureIndex(searchIndex.Name);
            searchService.PopulateIndex(table.ToJobProfileSearchIndex());
        }

        [When(@"I filter by the category '(.*)'")]
        public void WhenIFilterByTheCategory(string filterCategory)
        {
            OutputHelper.WriteLine($"The filter category is '{filterCategory}'");

            //we need to dispose JobProfileCategoryRepository
            using (var jobProfileCategoryRepository = new JobProfileCategoryRepository(SearchQueryService, mapper, null))
            {
                this.result = jobProfileCategoryRepository.GetRelatedJobProfiles(filterCategory);
            }
        }

        [Then(@"the number of job profiles returned is (.*)")]
        public void ThenTheNumberOfJobProfilesReturnedIs(int profilesCount)
        {
            this.result.Count().Should().Be(profilesCount);
        }

        [Then(@"the job profiles are listed in the following order")]
        public void ThenTheJobProfilesAreListedInTheFollowingOrder(Table table)
        {
            this.result.Select(r => r.Title).ShouldBeEquivalentTo(table.Rows.Select(r => r[0]), opt => opt.WithStrictOrdering());
        }
    }
}