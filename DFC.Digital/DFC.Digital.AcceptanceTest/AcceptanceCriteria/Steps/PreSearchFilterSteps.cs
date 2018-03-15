using DFC.Digital.AcceptanceTest.Infrastructure;
using DFC.Digital.AcceptanceTest.Infrastructure.Pages;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.AcceptanceCriteria.Steps
{
    [Binding]
    public class PreSearchFilterSteps : BaseStep
    {
        #region Ctor

        public PreSearchFilterSteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
        {
        }

        #endregion Ctor
        #region Whens

        [When(@"I click on the PSF Continue button")]
        public void WhenIClickOnThePSFContinueButton()
        {
            var homepage = GetNavigatedPage<Homepage>();
            homepage.ClickPSFContinueButton<PreSearchFilterPage>()
                .SaveTo(ScenarioContext);
        }

        [When(@"I select the (.*) tags (.*)")]
        public void WhenISelectTheJobLevelTagsJobLevel(string page, string tags)
        {
            var filterPage = GetNavigatedPage<PreSearchFilterPage>();
            filterPage.SelectTags(tags);
        }

        [When(@"I press continue on the '(.*)' page")]
        public void WhenIPressContinue(string filterPage)
        {
            var psfPage = GetNavigatedPage<PreSearchFilterPage>();
                    psfPage.ClickContinue<PreSearchFilterPage>()
                        .SaveTo(ScenarioContext);
        }

        [When(@"I click on filter results no '(.*)'")]
        public void WhenIClickOnFilterResultsNo(int result)
        {
            var resultPage = GetNavigatedPage<PreSearchFilterPage>();
            ScenarioContext.Set(resultPage.SelectedProfileTitle(result), "profileSelected");
            var jobProfilePage = resultPage.GoToResult<JobProfilePage>(result)
                .SaveTo(ScenarioContext);
        }

        [When(@"I press the back link on the '(.*)' page")]
        public void WhenIPressTheBackLinkOnThePage(string page)
        {
            var resultPage = GetNavigatedPage<PreSearchFilterPage>();
            resultPage.ClickBack<PreSearchFilterPage>();
        }

        #endregion

        #region Thens

        [Then(@"I am redirected to the '(.*)' page")]
        public void ThenIAmRedirectedToTheCorrectPage(string filter)
        {
            var filterPage = GetNavigatedPage<PreSearchFilterPage>();
            if (filter.Equals("Filter search results"))
            {
                filterPage.FilterResultsTitleDisplayed.Should().BeTrue("Should display results page");
            }
            else
            {
                filterPage.HasCorrectTitle(filter).Should().BeTrue(filter + " page should have loaded");
            }
        }

        [Then(@"Job Profiles are displayed")]
        public void ThenJobProfilesAreDisplayed()
        {
            GetNavigatedPage<PreSearchFilterPage>().JobProfilesAreShown.Should().BeTrue();
        }

        [Then(@"the no filter results message is displayed")]
        public void ThenTheNoFilterResultsMessageIsDisplayed()
        {
            var resultPage = GetNavigatedPage<PreSearchFilterPage>();
            resultPage.NoResultsMessage.Should().Contain("0 results found - try again using a different filters");
        }

        [Then(@"the (.*) tags are still selected")]
        public void ThenTheNoneTagsAreStillSelected(string tags)
        {
            var filterPage = GetNavigatedPage<PreSearchFilterPage>();
            filterPage.IsTagsSelected(tags).Should().BeTrue("Following tags should be selected: " + tags);
        }

        [Then(@"the (.*) tags are not selected")]
        public void ThenTheJobLevelJobLevelJobLevelTagsAreNotSelected(string tags)
        {
            var filterPage = GetNavigatedPage<PreSearchFilterPage>();
            filterPage.IsTagsSelected(tags).Should().BeFalse("Tags should be de-selected after pressing None");
        }
        #endregion
    }
}
