using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace DFC.Digital.NonUIAcceptanceTest.Steps
{
    [Binding]
    public class JobProfileSearchSteps
    {
        private const string AllProfileResultList = "AllProfileResultList";
        private const string SearchFailuresList = "SearchFailuresList";
             
        private ISearchIndexConfig searchIndex;
        private ISearchQueryService<JobProfileIndex> searchQueryService;
        private IAsyncHelper asyncHelper;
        private ITestOutputHelper OutputHelper { get; set; }

        public JobProfileSearchSteps(ITestOutputHelper outputHelper, ISearchIndexConfig searchIndex, ISearchQueryService<JobProfileIndex> searchQueryService)
        {
            this.OutputHelper = outputHelper;
            this.searchIndex = searchIndex;
            this.searchQueryService = searchQueryService;
            asyncHelper = new AsyncHelper();
        }

        [Given(@"I have a list of all job profile with titles and alterantive title")]
        public void GivenIHaveAListOfAllJobProfileWithTitlesAndAlterantiveTitle()
        {
            OutputHelper.WriteLine($"Search for * to get all profile");
            try
            {
                SearchResult<JobProfileIndex> searchResults = searchQueryService.Search("*", new SearchProperties { Count = 10000 });
                OutputHelper.WriteLine($"Got {searchResults.Count} profiles");
                ScenarioContext.Current.Add(AllProfileResultList, searchResults);
            }
            catch (Exception ex)
            {
                OutputHelper.WriteLine($"Exception in When:- {ex.ToString()}");
                throw;
            }
        }

        [When(@"I search by each alternative title for each of the  job profiles")]
        public void WhenISeachByEachAlternativeTitleForEachOfTheJobProfiles()
        {
            var allProfiles = (SearchResult<JobProfileIndex>)ScenarioContext.Current[AllProfileResultList];
            var searchProperties = new SearchProperties { Count = 10 };
            var testFailuresList = new List<string>();

            foreach (var profile in allProfiles.Results)
            {
                foreach (var alternativeTitle in profile.ResultItem.AlternativeTitle)
                {
                    SearchResult<JobProfileIndex> alternativeTitleResult = searchQueryService.Search(alternativeTitle, searchProperties);
                    if (alternativeTitleResult is null || !alternativeTitleResult.Results.Where(p => p.ResultItem.Title == profile.ResultItem.Title).Any())
                    {
                        testFailuresList.Add($"Title:{profile.ResultItem.Title} - Alterantive Title:{alternativeTitle}");
                    }
                }
            }

            ScenarioContext.Current.Add(SearchFailuresList, testFailuresList);
        }

        [Then(@"all the results returned should have the job profile with the matching alterantive tag on the first page\.")]
        public void ThenAllTheResultsReturnedShouldHaveTheJobProfileWithTheMatchingAlterantiveTagOnTheFirstPage_()
        {
            var alternativeTitleFailures = (List<string>)ScenarioContext.Current[SearchFailuresList];
            if (alternativeTitleFailures.Count() > 0)
            {
                OutputHelper.WriteLine($"Total Failures: {alternativeTitleFailures.Count()} searches. The following profiles do not appear in the first position when searched by the indicated alterantive title");
                foreach (var item in alternativeTitleFailures)
                {
                    OutputHelper.WriteLine(item);
                }
            }

            alternativeTitleFailures.Count().Should().Be(0);
        }
               
        [When(@"I search by each title for each of the job profiles")]
        public void WhenISearchByEachTitleForEachOfTheJobProfiles()
        {
            var allProfiles = (SearchResult<JobProfileIndex>)ScenarioContext.Current[AllProfileResultList];
            var searchProperties = new SearchProperties { Count = 1 };
            var testFailuresList = new List<string>();

            foreach (var profile in allProfiles.Results)
            {
                SearchResult<JobProfileIndex> titleResult = searchQueryService.Search(profile.ResultItem.Title, searchProperties);
                if (titleResult is null)
                {
                    testFailuresList.Add($"Title:{profile.ResultItem.Title}");
                }
            }

            ScenarioContext.Current.Add(SearchFailuresList, testFailuresList);
        }

        [Then(@"all the results returned should have the job profile with the matching title in the first position\.")]
        public void ThenAllTheResultsReturnedShouldHaveTheJobProfileWithTheMatchingTitleInTheFirstPosition_()
        {
            var titleFailures = (List<string>)ScenarioContext.Current[SearchFailuresList];
            if (titleFailures.Count() > 0)
            {
                OutputHelper.WriteLine($"Total Failures: {titleFailures.Count()} searches. The following profiles do not appear in the first position when searched by the indicated title");
                foreach (var item in titleFailures)
                {
                    OutputHelper.WriteLine(item);
                }
            }

            titleFailures.Count().Should().Be(0);
        }

    }
}
