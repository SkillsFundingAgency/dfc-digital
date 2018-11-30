using AutoMapper;
using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace DFC.Digital.Service.AzureSearch.IntegrationTests
{
    [Binding]
    public class JobProfileSearchSteps
    {
        private const string AllProfileResultList = "AllProfileResultList";
        private const string AlternativeTitleFailuresList = "AlternativeTitleFailuresList";

        private SearchResult<JobProfileIndex> results;
        private ISearchService<JobProfileIndex> searchService;
        private ISearchIndexConfig searchIndex;
        private ISearchQueryService<JobProfileIndex> searchQueryService;
        private IAsyncHelper asyncHelper;

        public JobProfileSearchSteps(ITestOutputHelper outputHelper, ISearchService<JobProfileIndex> searchService, ISearchIndexConfig searchIndex, ISearchQueryService<JobProfileIndex> searchQueryService)
        {
            this.OutputHelper = outputHelper;
            this.searchService = searchService;
            this.searchIndex = searchIndex;
            this.searchQueryService = searchQueryService;
            asyncHelper = new AsyncHelper();
        }

        private ITestOutputHelper OutputHelper { get; set; }

        [Given(@"the following job profiles exist:")]
        public void GivenTheFollowingJobProfilesExistAsync(Table table)
        {
            try
            {
                asyncHelper.Synchronise(() => searchService.EnsureIndexAsync(searchIndex.Name));
                asyncHelper.Synchronise(() => searchService.PopulateIndexAsync(table.ToJobProfileSearchIndex()));
            }
            catch (Exception ex)
            {
                OutputHelper.WriteLine($"Exception in When:- {ex.ToString()}");
            }
        }

        [Given(@"that '(.*)' job profiles exist with '(.*)':")]
        public void GivenThatJobProfilesExistWithAsync(int countOfDummies, string jobTitle)
        {
            try
            {
                asyncHelper.Synchronise(() => searchService.EnsureIndexAsync(searchIndex.Name));
                asyncHelper.Synchronise(() => searchService.PopulateIndexAsync(countOfDummies.CreateWithTitle(jobTitle)));
            }
            catch (Exception ex)
            {
                OutputHelper.WriteLine($"Exception in When:- {ex.ToString()}");
            }
        }

        [When(@"I search using the search term '(.*)'")]
        public void WhenISearchUsingTheSearchTerm(string searchTerm)
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

        [Then(@"the result list will contain '(.*)' profile\(s\)")]
        public void ThenTheResultListWillContainProfileS(int totalNumber)
        {
            //Log results
            OutputHelper.WriteLine($"Number of results expected {totalNumber}  number returned {results?.Results.Count()} actual result {results?.ToJson()}");
            results?.Results.Count().Should().Be(totalNumber);
        }

        [Then(@"the profiles are listed in the following order:")]
        public void ThenDisplayAllTheSearchResultsAsBelow(Table table)
        {
            var expected = table.ToJobProfileSearchIndex();
            var actual = results?.Results.Select(r => r.ResultItem);

            //Log results
            OutputHelper.WriteLine($"Expected order {expected.ToJson()}");
            OutputHelper.WriteLine($"Actual order {actual?.ToJson()}");

            actual.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Then(@"the profiles are listed in no specific order:")]
        public void ThenDisplayAllTheSearchResultsInNoSpecificOrderAsBelow(Table table)
        {
            var expected = table.ToJobProfileSearchIndex();
            var actual = results?.Results.Select(r => r.ResultItem);

            //Log results
            OutputHelper.WriteLine($"Expected {expected.ToJson()}");
            OutputHelper.WriteLine($"Actual {actual?.ToJson()}");

            actual.Should().BeEquivalentTo(expected);
        }

        [Then(@"the profiles are listed first in no specific order:")]
        public void ThenTheProfilesAreListedFirstInNoSpecificOrder(Table table)
        {
            var expected = table.ToJobProfileSearchIndex();
            var actual = results?.Results.Select(r => r.ResultItem).Take(expected.Count());

            //Log results
            OutputHelper.WriteLine($"Expected {expected.ToJson()}");
            OutputHelper.WriteLine($"Actual {actual?.ToJson()}");

            actual.Should().BeEquivalentTo(expected);
        }

        [Then(@"the following profiles are listed in no specific order skip '(.*)' results:")]
        public void ThenTheFollowingProfilesAreListedFromIndexInNoSpecificOrder(int skip, Table table)
        {
            var expected = table.ToJobProfileSearchIndex();
            var actual = results?.Results.Select(r => r.ResultItem).Skip(skip);

            //Log results
            OutputHelper.WriteLine($"Expected {expected.ToJson()}");
            OutputHelper.WriteLine($"Actual {actual?.ToJson()}");

            actual.Should().BeEquivalentTo(expected);
        }

        [Then(@"the result count should match '(.*)'")]
        public void ThenTheResultCountShouldMatch(int countOfDummies)
        {
            results?.Count.Should().Be(countOfDummies);
        }

        [Given(@"there are '(.*)' profiles which have a Title of '(.*)'")]
        public async Task GivenThereAreProfilesWhichHaveATitleOfAsync(int countOfDummies, string jobProfileTitle)
        {
            await searchService.EnsureIndexAsync(searchIndex.Name);
            await searchService.PopulateIndexAsync(countOfDummies.CreateWithTitle(jobProfileTitle));
        }

        [Then(@"the number of job profiles shown on the page is less than or equal to '(.*)'\. \(i\.e\. the page limit\)")]
        public void ThenTheNumberOfJobProfilesShownOnThePageIsLessThanOrEqualToIEThePageLimit(int pageLimit)
        {
            results.Results.Count().Should().BeLessOrEqualTo(pageLimit).And.BeGreaterThan(0);
        }

        [Then(@"the number of job profiles shown on the page is equal to '(.*)'\. \(i\.e\. the page limit\)")]
        public void ThenTheNumberOfJobProfilesShownOnThePageIsEqualToIEThePageLimit(int pageLimit)
        {
            results?.Results.Count().Should().Be(pageLimit);
        }

        [Given(@"I have a list of all alterantive title for each jop profile")]
        public void GivenIHaveAListOfAllAlterantiveTitleForEachJopProfile()
        {
            OutputHelper.WriteLine($"Search for * to get all profile");
            try
            {
                SearchResult<JobProfileIndex> searchResults = searchQueryService.Search("*",  new SearchProperties { Count = 10000 });
                OutputHelper.WriteLine($"Got {searchResults.Count} profiles to check for alternative title");
                ScenarioContext.Current.Add(AllProfileResultList, searchResults);
            }
            catch (Exception ex)
            {
                OutputHelper.WriteLine($"Exception in When:- {ex.ToString()}");
            }
        }

        [When(@"I seach by each alternative title for each of the  job profiles")]
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

            ScenarioContext.Current.Add(AlternativeTitleFailuresList, testFailuresList);
        }

        [Then(@"all the results returned should have the job profile with the matching alterantive tag in the first position")]
        public void ThenAllTheResultsReturendShouldHaveTheJobProfileWithTheMatchingAlterantiveTagInTheFirstPosition()
        {
            var alternativeTitleFailures = (List<string>)ScenarioContext.Current[AlternativeTitleFailuresList];
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
    }
}