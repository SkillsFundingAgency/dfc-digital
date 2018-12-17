using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace DFC.Digital.NonUIAcceptanceTest.Steps
{
    [Binding]
    public class JobProfileSearchSteps
    {
        private const string AllProfileResultList = "AllProfileResultList";
        private const string SearchFailuresList = "SearchFailuresList";
        private const string AllAlternativeSearchResults = "AllAlternativeSearchResults";
        private const string AllTitleSearchResults = "AllTitleSearchResults";

        private readonly ISearchIndexConfig searchIndex;
        private readonly ISearchQueryService<JobProfileIndex> searchQueryService;
        private readonly ScenarioContext scenarioContext;
        private readonly ITestOutputHelper outputHelper;

        public JobProfileSearchSteps(ITestOutputHelper outputHelper, ISearchIndexConfig searchIndex, ISearchQueryService<JobProfileIndex> searchQueryService, ScenarioContext scenarioContext)
        {
            this.outputHelper = outputHelper;
            this.searchIndex = searchIndex;
            this.searchQueryService = searchQueryService;
            this.scenarioContext = scenarioContext;
        }

        [Given(@"I have a list of all job profile with titles and alternative title")]
        public void GivenIHaveAListOfAllJobProfileWithTitlesAndAlterantiveTitleAsync()
        {
            outputHelper.WriteLine($"Search for * to get all profile");
            try
            {
                var searchResults = searchQueryService.Search("*", new SearchProperties
                {
                    UseRawSearchTerm = true,
                    Count = 10000
                });

                searchResults.SaveTo(scenarioContext, AllProfileResultList);
                outputHelper.WriteLine($"Got {searchResults.Count} profiles");
            }
            catch (Exception ex)
            {
                outputHelper.WriteLine($"Exception in When:- {ex.ToString()}");
                throw;
            }
        }

        [When(@"I search by each alternative title for each of the  job profiles")]
        public void WhenISeachByEachAlternativeTitleForEachOfTheJobProfiles()
        {
            var searchResults = new Dictionary<string, SearchResult<JobProfileIndex>>();
            var allProfiles = scenarioContext.Get<SearchResult<JobProfileIndex>>(AllProfileResultList);

            foreach (var profile in allProfiles.Results)
            {
                foreach (var alternativeTitle in profile.ResultItem.AlternativeTitle)
                {
                    if (!searchResults.ContainsKey(alternativeTitle))
                    {
                        searchResults.Add(alternativeTitle, searchQueryService.Search(alternativeTitle, new SearchProperties()));
                    }
                }
            }

            searchResults.SaveTo(scenarioContext, AllAlternativeSearchResults);
        }

        [Then(@"all the results returned should have the job profile with the matching alternative title in the first position\.")]
        public void ThenAllTheResultsReturnedShouldHaveTheJobProfileWithTheMatchingAlterantiveTagOnTheFirstPage()
        {
            int failures = 0;
            var searchResults = scenarioContext.Get<Dictionary<string, SearchResult<JobProfileIndex>>>(AllAlternativeSearchResults);
            foreach (var item in searchResults)
            {
                var resultItem = item.Value.Results.FirstOrDefault()?.ResultItem;
                if (resultItem is null || !resultItem.AlternativeTitle.Any(a => a.Equals(item.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    failures++;
                    outputHelper.WriteLine($"Searched for {item.Key}, And the first result is Title: {resultItem?.Title ?? "NULL"} and alternative titles: {string.Join(",", resultItem?.AlternativeTitle)}");
                }
            }

            outputHelper.WriteLine($"Total Failures: {failures}.");
            failures.Should().Be(0);
        }

        [When(@"I search by each title for each of the job profiles")]
        public void WhenISearchByEachTitleForEachOfTheJobProfiles()
        {
            var searchResults = new Dictionary<string, SearchResult<JobProfileIndex>>();
            var allProfiles = scenarioContext.Get<SearchResult<JobProfileIndex>>(AllProfileResultList);

            foreach (var profile in allProfiles.Results)
            {
                searchResults.Add(profile.ResultItem.Title, searchQueryService.Search(profile.ResultItem.Title, new SearchProperties()));
            }

            searchResults.SaveTo(scenarioContext, AllTitleSearchResults);
        }

        [Then(@"all the results returned should have the job profile with the matching title in the first position\.")]
        public void ThenAllTheResultsReturnedShouldHaveTheJobProfileWithTheMatchingTitleInTheFirstPosition()
        {
            int failures = 0;
            var searchResults = scenarioContext.Get<Dictionary<string, SearchResult<JobProfileIndex>>>(AllTitleSearchResults);
            foreach (var item in searchResults)
            {
                var resultItem = item.Value.Results.FirstOrDefault()?.ResultItem;
                if (resultItem is null || !resultItem.Title.Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    failures++;
                    outputHelper.WriteLine($"Searched for {item.Key}, And the first result is Title: {resultItem?.Title ?? "NULL"}");
                }
            }

            outputHelper.WriteLine($"Total Failures: {failures}.");
            failures.Should().Be(0);
        }
    }
}