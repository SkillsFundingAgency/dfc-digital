using AutoMapper;
using DFC.Digital.Automation.Test.Utilities;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces; using DFC.Digital.Core;
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
        private ISearchService<JobProfileIndex> searchService;
        private ISearchIndexConfig searchIndex;
        private IMapper mapper;
        private IAsyncHelper asyncHelper;

        public JobProfilesByCategorySteps(ITestOutputHelper outputHelper, ISearchService<JobProfileIndex> searchService, ISearchIndexConfig searchIndex, ISearchQueryService<JobProfileIndex> searchQueryService, IMapper mapper)
        {
            this.OutputHelper = outputHelper;
            this.searchService = searchService;
            this.searchIndex = searchIndex;
            this.SearchQueryService = searchQueryService;
            this.mapper = mapper;
            this.asyncHelper = new AsyncHelper();
        }

        private ITestOutputHelper OutputHelper { get; set; }

        private ISearchQueryService<JobProfileIndex> SearchQueryService { get; }

        [Given(@"the following job profiles in catogories  exist:")]
        public void GivenTheFollowingJobProfilesInCatogoriesExistAsync(Table table)
        {
            asyncHelper.Synchronise(() => searchService.EnsureIndexAsync(searchIndex.Name));
            asyncHelper.Synchronise(() => searchService.PopulateIndexAsync(table.ToJobProfileSearchIndex()));
        }

        [When(@"I filter by the category '(.*)'")]
        public void WhenIFilterByTheCategory(string filterCategory)
        {
            OutputHelper.WriteLine($"The filter category is '{filterCategory}'");
            var jobProfileCategoryRepository = new JobProfileCategoryRepository(SearchQueryService, mapper, null);
            this.result = jobProfileCategoryRepository.GetRelatedJobProfiles(filterCategory);
        }

        [Then(@"the number of job profiles returned is (.*)")]
        public void ThenTheNumberOfJobProfilesReturnedIs(int profilesCount)
        {
            this.result.Count().Should().Be(profilesCount);
        }

        [Then(@"the job profiles are listed in the following order")]
        public void ThenTheJobProfilesAreListedInTheFollowingOrder(Table table)
        {
            this.result.Select(r => r.Title).Should().BeEquivalentTo(table.Rows.Select(r => r[0]), opt => opt.WithStrictOrdering());
        }
    }
}