﻿using AutoMapper;
using DFC.Digital.Automation.Test.Utilities;
using DFC.Digital.Core.Extensions;
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
    public class JobProfileSearchSteps
    {
        private SearchResult<JobProfileIndex> results;
        private ISearchService<JobProfileIndex> searchService;
        private ISearchIndexConfig searchIndex;
        private ISearchQueryService<JobProfileIndex> searchQueryService;
        private IMapper mapper;

        public JobProfileSearchSteps(ITestOutputHelper outputHelper, ISearchService<JobProfileIndex> searchService, ISearchIndexConfig searchIndex, ISearchQueryService<JobProfileIndex> searchQueryService, IMapper mapper)
        {
            this.OutputHelper = outputHelper;
            this.searchService = searchService;
            this.searchIndex = searchIndex;
            this.searchQueryService = searchQueryService;
            this.mapper = mapper;
        }

        private ITestOutputHelper OutputHelper { get; set; }

        [Given(@"the following job profiles exist:")]
        public void GivenTheFollowingJobProfilesExist(Table table)
        {
            try
            {
                searchService.EnsureIndex(searchIndex.Name);
                searchService.PopulateIndex(table.ToJobProfileSearchIndex());
            }
            catch (Exception ex)
            {
                OutputHelper.WriteLine($"Exception in When:- {ex.ToString()}");
            }
        }

        [Given(@"that '(.*)' job profiles exist with '(.*)':")]
        public void GivenThatJobProfilesExistWith(int countOfDummies, string jobTitle)
        {
            try
            {
                searchService.EnsureIndex(searchIndex.Name);
                searchService.PopulateIndex(countOfDummies.CreateWithTitle(jobTitle));
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

            actual.ShouldBeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Then(@"the profiles are listed in no specific order:")]
        public void ThenDisplayAllTheSearchResultsInNoSpecificOrderAsBelow(Table table)
        {
            var expected = table.ToJobProfileSearchIndex();
            var actual = results?.Results.Select(r => r.ResultItem);

            //Log results
            OutputHelper.WriteLine($"Expected {expected.ToJson()}");
            OutputHelper.WriteLine($"Actual {actual?.ToJson()}");

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"the profiles are listed first in no specific order:")]
        public void ThenTheProfilesAreListedFirstInNoSpecificOrder(Table table)
        {
            var expected = table.ToJobProfileSearchIndex();
            var actual = results?.Results.Select(r => r.ResultItem).Take(expected.Count());

            //Log results
            OutputHelper.WriteLine($"Expected {expected.ToJson()}");
            OutputHelper.WriteLine($"Actual {actual?.ToJson()}");

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"the following profiles are listed in no specific order skip '(.*)' results:")]
        public void ThenTheFollowingProfilesAreListedFromIndexInNoSpecificOrder(int skip, Table table)
        {
            var expected = table.ToJobProfileSearchIndex();
            var actual = results?.Results.Select(r => r.ResultItem).Skip(skip);

            //Log results
            OutputHelper.WriteLine($"Expected {expected.ToJson()}");
            OutputHelper.WriteLine($"Actual {actual?.ToJson()}");

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"the result count should match '(.*)'")]
        public void ThenTheResultCountShouldMatch(int countOfDummies)
        {
            results?.Count.Should().Be(countOfDummies);
        }

        [Given(@"there are '(.*)' profiles which have a Title of '(.*)'")]
        public void GivenThereAreProfilesWhichHaveATitleOf(int countOfDummies, string jobProfileTitle)
        {
            searchService.EnsureIndex(searchIndex.Name);
            searchService.PopulateIndex(countOfDummies.CreateWithTitle(jobProfileTitle));
        }

        [Then(@"the number of job profiles shown on the page is less than or equal to '(.*)'\. \(i\.e\. the page limit\)")]
        public void ThenTheNumberOfJobProfilesShownOnThePageIsLessThanOrEqualTo_I_E_ThePageLimit(int pageLimit)
        {
            results.Results.Count().Should().BeLessOrEqualTo(pageLimit).And.BeGreaterThan(0);
        }

        [Then(@"the number of job profiles shown on the page is equal to '(.*)'\. \(i\.e\. the page limit\)")]
        public void ThenTheNumberOfJobProfilesShownOnThePageIsEqualTo_I_E_ThePageLimit(int pageLimit)
        {
            results?.Results.Count().ShouldBeEquivalentTo(pageLimit);
        }
    }
}