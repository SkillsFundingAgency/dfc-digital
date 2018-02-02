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

        private ITestOutputHelper outputHelper { get; set; }

        private ISearchService<JobProfileIndex> searchService;
        private ISearchIndexConfig searchIndex;
        private IMapper mapper;

        public ISearchQueryService<JobProfileIndex> searchQueryService { get; }

        public JobProfilesByCategorySteps(ITestOutputHelper outputHelper, ISearchService<JobProfileIndex> searchService, ISearchIndexConfig searchIndex, ISearchQueryService<JobProfileIndex> searchQueryService, IMapper mapper)
        {
            this.outputHelper = outputHelper;
            this.searchService = searchService;
            this.searchIndex = searchIndex;
            this.searchQueryService = searchQueryService;
            this.mapper = mapper;
        }

        [Given(@"the following job profiles in catogories  exist:")]
        public void GivenTheFollowingJobProfilesInCatogoriesExist(Table table)
        {
            searchService.EnsureIndex(searchIndex.Name);
            searchService.PopulateIndex(table.ToJobProfileSearchIndex());
        }

        [When(@"I filter by the category '(.*)'")]
        public void WhenIFilterByTheCategory(string filterCategory)
        {
            outputHelper.WriteLine($"The filter category is '{filterCategory}'");
            var JobProfileCategoryRepository = new JobProfileCategoryRepository(searchQueryService, mapper, null);
            this.result = JobProfileCategoryRepository.GetRelatedJobProfiles(filterCategory);
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