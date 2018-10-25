using DFC.Digital.AcceptanceTest.Infrastructure;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.AcceptanceCriteria.Steps
{
    [Binding]
    public class VocSurveySteps : BaseStep
    {
        public VocSurveySteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
        {
        }

        #region Given's
        [Given(@"I have not visited the job profile page previously")]
        public void GivenIHaveNotVisitedTheJobProfilePagePreviously()
        {
            DeleteCookie("vocPersonalisation");
        }

        #endregion

        #region When's

        [When(@"I select to fill in the online survey")]
        public void WhenISelectToFillInTheOnlineSurvey()
        {
            var survey = GetNavigatedPage<Homepage>();
            survey.ClickTakeSurvey<VocSurveyPage>()
                .SaveTo(ScenarioContext);
        }

       #endregion When's

        #region Then's

        [Then(@"the success message is displayed")]
        public void ThenTheSuccessMessageIsDisplayed()
        {
            var survey = GetNavigatedPage<Homepage>();
            survey.SuccessEmailMessageDisplayed.Should().BeTrue();
        }

        [Then(@"the vocPersonalisation cookie should not be displayed")]
        public void ThenTheVocPersonalisationCookieShouldNotBeDisplayed()
        {
            GetCookieValue("vocPersonalisation").Should().BeNullOrEmpty();
        }

        [Then(@"the vocPersonaliation cookie should display the last job profile title")]
        public void ThenTheVocPersonaliationCookieShouldDisplayTheLastJobProfileTitle()
        {
            ScenarioContext.TryGetValue("profileURL", out string lastVisitedProfileUrl);
            GetCookieValue("vocPersonalisation").Should().Contain(lastVisitedProfileUrl.ToLower());
        }

        [Then(@"the GA client ID should not be empty")]
        public void ThenTheGaClientIdShouldNotBeEmpty()
        {
            GetCookieValue("_ga").Should().NotBeNullOrEmpty();
        }

        #endregion Then's
    }
}