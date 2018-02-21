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
    public class JobProfileAutosuggestSteps
    {
        private SuggestionResult<JobProfileIndex> results;
        private ISearchService<JobProfileIndex> searchService;
        private ISearchIndexConfig searchIndex;
        private ISearchQueryService<JobProfileIndex> searchQueryService;
        private IMapper mapper;

        public JobProfileAutosuggestSteps(ITestOutputHelper outputHelper, ISearchService<JobProfileIndex> searchService, ISearchIndexConfig searchIndex, ISearchQueryService<JobProfileIndex> searchQueryService, IMapper mapper)
        {
            this.OutputHelper = outputHelper;
            this.searchService = searchService;
            this.searchIndex = searchIndex;
            this.searchQueryService = searchQueryService;
            this.mapper = mapper;
        }

        private ITestOutputHelper OutputHelper { get; set; }

        [When(@"I type the term '(.*)'")]
        public void WhenITypeTheTerm(string suggestionTerm)
        {
            OutputHelper.WriteLine($"The suggestion term is '{suggestionTerm}'");
            try
            {
                results = searchQueryService.GetSuggestion(
                    suggestionTerm,
                    new SuggestProperties { UseFuzzyMatching = true, MaxResultCount = 5 });
            }
            catch (Exception ex)
            {
                OutputHelper.WriteLine($"Exception in When:- {ex.ToString()}");
            }

            //Log results
            var actual = results?.Results.Select(r => r.Index);
            OutputHelper.WriteLine($"Actual order {actual?.ToJson()}");
        }

        [Then(@"the result list will contain '(.*)' suggestion\(s\)")]
        public void ThenTheResultListWillContainSuggestionS(int expectedCount)
        {
            results.Results.Count().Should().Be(expectedCount);
        }

        [Then(@"the suggestion are listed in no specific order:")]
        public void ThenTheSuggestionAreListedInNoSpecificOrder(Table table)
        {
            var expected = table.ToJobProfileSearchIndex();
            var actual = results?.Results.Select(r => r.Index);

            //Log results
            OutputHelper.WriteLine($"Expected order {expected.ToJson()}");
            actual.ShouldAllBeEquivalentTo(expected);
        }
    }
}