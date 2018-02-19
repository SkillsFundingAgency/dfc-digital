using DFC.Digital.AcceptanceTest.Infrastructure.Config;
using DFC.Digital.AcceptanceTest.Infrastructure.Pages;
using DFC.Digital.AcceptanceTest.Infrastructure.Utilities;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.AcceptanceCriteria.Steps
{
    [Binding]
    public class BauSteps : BaseStep
    {
        #region Ctor
        public BauSteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
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
