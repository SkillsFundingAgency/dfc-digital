﻿using DFC.Digital.AcceptanceTest.Infrastructure;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.AcceptanceCriteria.Steps
{
    [Binding]
    public class BAUSteps : BaseStep
    {
        #region Ctor
        public BAUSteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
        {
        }
        #endregion

        [When(@"I click the BAU JP signpost banner")]
        public void WhenIClickTheBaujpSignpostBanner()
        {
            var bauPage = GetNavigatedPage<BauProfilePage>();
            bauPage.ClickBetaBanner<JobProfilePage>().SaveTo(ScenarioContext);
        }

        [Then(@"I am redirected to the BAU Search results page")]
        public void ThenIAmRedirectedToTheBauSearchResultsPage()
        {
            var bauSearch = GetNavigatedPage<BauSearchPage>();
            bauSearch.ResultsSectionDisplated.Should().BeTrue();
        }

        [Then(@"the form contains the (.*) searched term")]
        public void ThenTheUrlContainsTheSearchTerm(string searchedTerm)
        {
            var bauSearch = GetNavigatedPage<BauSearchPage>();
            bauSearch.PopulatedSearchText.Should().Be(searchedTerm);
        }
    }
}
