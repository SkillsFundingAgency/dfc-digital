using DFC.Digital.AcceptanceTest.AcceptanceCriteria.Steps;
using DFC.Digital.AcceptanceTest.Infrastructure;
using DFC.Digital.AcceptanceTest.Infrastructure.Pages;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.AcceptanceCriteria
{
    [Binding]
    public class ExploreCareersSteps : BaseStep
    {
        //private HomePage homePage;
        private ExploreCareersPage resultPage;

        //private JobProfilePage jobProfilePage;
        private string jobProfileSelected;

        public ExploreCareersSteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
        {
        }

        #region Givens
        [Given(@"that I am viewing the Explore careers page")]
        public void GivenThatIAmViewingTheExploreCareersPage()
        {
            NavigateToExploreCareersPage<ExploreCareersPage, JobProfileSearchBoxViewModel>();
        }

        #endregion

        #region Whens
        [When(@"I search without entering search term")]
        public void WhenISearchWithoutEnteringSearchTerm()
        {
            resultPage = GetNavigatedPage<ExploreCareersPage>().Search<ExploreCareersPage>(new JobProfileSearchBoxViewModel
            {
                SearchTerm = string.Empty
            });
        }

        #endregion

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
            GetNavigatedPage<ExploreCareersPage>()?.PageHeading.Should().Contain(serviceName);
            GetNavigatedPage<ExploreCareersPage>()?.HasSearchWidget.Should().BeTrue();
        }

        [Then(@"display the ""(.*)"" text")]
        public void ThenDisplayTheText(string exploreCategoriesText)
        {
            GetNavigatedPage<ExploreCareersPage>()?.HasExploreCategoryText(exploreCategoriesText).Should().BeTrue();
        }

        [Then(@"display a list of job profile categories")]
        public void ThenDisplayAListOfJobProfileCategories()
        {
            GetNavigatedPage<ExploreCareersPage>()?.HasJobProfileCategoriesSection.Should().BeTrue();
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

        #endregion
    }
}
