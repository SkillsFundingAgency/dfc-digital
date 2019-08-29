using DFC.Digital.AcceptanceTest.Infrastructure;
using DFC.Digital.AcceptanceTest.Infrastructure.Pages;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.AcceptanceCriteria.Steps
{
    [Binding]
    public class HomepageSteps : BaseStep
    {
        public HomepageSteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
        {
        }

        #region Givens

        [Given(@"that I am viewing the Home page")]
        public void GivenThatIAmViewingTheHomepage()
        {
            NavigateToHomePage<Homepage, JobProfileSearchBoxViewModel>();
        }

        [Given(@"that I am viewing the health status page")]
        public void GivenThatIAmViewingTheHealthStatusPage()
        {
            NavigateToHealthStatusPage<HealthStatusPage>();
        }

        #endregion Givens

        #region Whens
        [When(@"I click the Privacy link")]
        public void WhenIClickThePrivacyLink()
        {
            GetNavigatedPage<Homepage>()?.ClickPrivacyLink<PrivacyPage>()
                .SaveTo(ScenarioContext);
        }

        [When(@"I click the T&C link")]
        public void WhenIClickTheTCLink()
        {
            GetNavigatedPage<Homepage>()?.ClickTermAndCondLink<TermsAndConditionPage>()
                .SaveTo(ScenarioContext);
        }

        [When(@"I click the Information Sources link")]
        public void WhenIClickTheInformationSourcesLink()
        {
            GetNavigatedPage<Homepage>()?.ClickInformationSourcesLink<InformationSourcesPage>()
                .SaveTo(ScenarioContext);
        }

        [When(@"I click the Help link")]
        public void WhenIClickTheHelpLink()
        {
            GetNavigatedPage<Homepage>()?.ClickHelpLink<HelpPage>()
                .SaveTo(ScenarioContext);
        }

        #endregion Whens

        #region Thens

        [Then(@"I am redirected to the homepage")]
        public void ThenIAmRedirectedToTheHomepage()
        {
            GetNavigatedPage<Homepage>()?.ServiceName.Should().BeTrue();
        }

        [Then(@"I am redirected to the Privacy page")]
        public void ThenIAmRedirectedToThePrivacyPage()
        {
            var privacyPage = GetNavigatedPage<PrivacyPage>();
            privacyPage.PageTitle.Should().Contain("Privacy and cookies");
        }

        [Then(@"I am redirected to the T&C page")]
        public void ThenIAmRedirectedToTheTCPage()
        {
            var termsPage = GetNavigatedPage<TermsAndConditionPage>();
            termsPage.PageTitle.Should().Contain("Terms and Conditions");
        }

        [Then(@"I am redirected to the Information Sources page")]
        public void ThenIAmRedirectedToTheInformationSourcesPage()
        {
            var infoSourcesPage = GetNavigatedPage<InformationSourcesPage>();
            infoSourcesPage.PageTitle.Should().Contain("Information sources");
        }

        [Then(@"I am redirected to the Help page")]
        public void ThenIAmRedirectedToTheHelpPage()
        {
            GetNavigatedPage<HelpPage>()?.Url.Should().Contain("help");
        }

        [Then(@"I am redirected to the Health Status page")]
        public void ThenIAmRedirectedToTheHealthStatusPage()
        {
            GetNavigatedPage<HealthStatusPage>().ListOfServices.Should().BeTrue();
        }

        #endregion Thens
    }
}