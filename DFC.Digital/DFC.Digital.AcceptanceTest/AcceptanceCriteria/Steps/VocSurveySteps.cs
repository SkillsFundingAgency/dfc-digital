using DFC.Digital.AcceptanceTest.Infrastructure.Config;
using DFC.Digital.AcceptanceTest.Infrastructure.Pages;
using DFC.Digital.AcceptanceTest.Infrastructure.Utilities;
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

        public string CookieValue { get; set; }

        #region Given's
        [Given(@"I have not visited the job profile page previously")]
        public void GivenIHaveNotVisitedTheJobProfilePagePreviously()
        {
            DeleteCookie("vocPersonalisation");
        }

        #endregion

        #region When's

        [When(@"I enter the email '(.*)' and press send")]
        public void WhenIEnterAValidEmailAndPressSend(string email)
        {
            var survey = GetNavigatedPage<Homepage>();
            survey.ClickTakeSurvey();
            survey.SubmitEmail<VocSurveyPage>(email);
        }

        [When(@"I select to fill in the online survey")]
        public void WhenISelectToFillInTheOnlineSurvey()
        {
            var survey = GetNavigatedPage<Homepage>();
            survey.ClickTakeSurvey();
            survey.SelectOnlineSurvey<VocSurveyPage>()
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
            CookieValue = GetCookieValue("vocPersonalisation");
            CookieValue.Should().BeNullOrEmpty();
            }

        [Then(@"the vocPersonaliation cookie should display the last job profile title")]
        public void ThenTheVocPersonaliationCookieShouldDisplayTheLastJobProfileTitle()
        {
            CookieValue = GetCookieValue("vocPersonalisation");
            ScenarioContext.TryGetValue("profileURL", out string lastVisitedProfileUrl);
            CookieValue.Should().Contain(lastVisitedProfileUrl.ToLower());
        }

        [Then(@"the GA client ID should not be empty")]
        public void ThenTheGaClientIdShouldNotBeEmpty()
        {
            string gaCookieValue = GetCookieValue("_ga");
            gaCookieValue.Should().NotBeNullOrEmpty();
        }

        #endregion Then's
    }
}