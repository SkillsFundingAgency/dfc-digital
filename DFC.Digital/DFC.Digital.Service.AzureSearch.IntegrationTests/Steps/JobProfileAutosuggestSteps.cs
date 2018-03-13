using System;
using System.Linq;
using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FluentAssertions;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace DFC.Digital.Service.AzureSearch.IntegrationTests.Steps
{
    [Binding]
    public class JobProfileAutosuggestSteps
    {
        private SuggestionResult<JobProfileIndex> results;
        private ISearchQueryService<JobProfileIndex> searchQueryService;

        public JobProfileAutosuggestSteps(ITestOutputHelper outputHelper, ISearchQueryService<JobProfileIndex> searchQueryService)
        {
            OutputHelper = outputHelper;
            this.searchQueryService = searchQueryService;
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
                OutputHelper.WriteLine($"Exception in When:- {ex}");
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
            actual.Should().AllBeEquivalentTo(expected);
        }
    }
}