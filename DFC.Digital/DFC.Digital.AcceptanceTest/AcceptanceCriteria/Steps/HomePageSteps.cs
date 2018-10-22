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
        //private HomePage homePage;
        private Homepage resultPage;

        //private JobProfilePage jobProfilePage;
        private string jobProfileSelected;

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

        [When(@"I search without entering search term")]
        public void WhenISearchWithoutEnteringSearchTerm()
        {
            resultPage = GetNavigatedPage<Homepage>().Search<Homepage>(new JobProfileSearchBoxViewModel
            {
                SearchTerm = string.Empty
            });
        }

        [When(@"I click the Privacy link")]
        public void WhenIClickThePrivacyLink()
        {
            GetNavigatedPage<Homepage>()?.ClickPrivacyLink<PrivacyPage>()
                .SaveTo(ScenarioContext);
        }

        [When(@"I click the T&C link")]
        public void WhenIClickTheTCLink()
        {
            GetNavigatedPage<Homepage>()?.ClickTandCLink<TermsAndConditionPage>()
                .SaveTo(ScenarioContext);
        }

        [When(@"I click the Information Sources link")]
        public void WhenIClickTheInformationSourcesLink()
        {
            GetNavigatedPage<Homepage>()?.ClickInformationSourcesLink<InformationSourcesPage>()
                .SaveTo(ScenarioContext);
        }

        #endregion Whens

        #region Thens

        [Then(@"the user remains on the Home page")]
        public void ThenTheHomepageUrlChangesToShowSearchAction()
        {
            resultPage.UrlShowsSearchAction.Should().BeTrue();
        }

        [Then(@"no error message is displayed")]
        public void ThenNoErrorMessageIsDisplayed()
        {
            resultPage.HasErrorMessage.Should().BeFalse();
        }

        [Then(@"display Service Name ""(.*)"" and Search Box")]
        public void ThenDisplayServiceNameDescriptionContainsAndSearchBox(string serviceName)
        {
            GetNavigatedPage<Homepage>()?.PageHeading.Should().Contain(serviceName);
            GetNavigatedPage<Homepage>()?.HasSearchWidget.Should().BeTrue();
        }

        [Then(@"display the ""(.*)"" text")]
        public void ThenDisplayTheText(string exploreCategoriesText)
        {
            GetNavigatedPage<Homepage>()?.HasExploreCategoryText(exploreCategoriesText).Should().BeTrue();
        }

        [Then(@"display a list of job profile categories")]
        public void ThenDisplayAListOfJobProfileCategories()
        {
            GetNavigatedPage<Homepage>()?.HasJobProfileCategoriesSection.Should().BeTrue();
        }

        [Then(@"display the correct job profile page")]
        public void ThenDisplayTheCorrectJobProfilePage()
        {
            if (jobProfileSelected == null)
            {
                ScenarioContext.TryGetValue("jobprofileselected", out jobProfileSelected);
            }

            GetNavigatedPage<Homepage>()?.PageHeading.Should().Contain(jobProfileSelected);
        }

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
            infoSourcesPage.PageTitle.Should().Contain("Salary information");
        }

        [Then(@"I am redirected to the Health Status page")]
        public void ThenIAmRedirectedToTheHealthStatusPage()
        {
            GetNavigatedPage<HealthStatusPage>().ListOfServices.Should().BeTrue();
        }

        #endregion Thens
    }
}