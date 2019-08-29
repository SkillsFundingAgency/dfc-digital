using DFC.Digital.AcceptanceTest.Infrastructure;
using DFC.Digital.AcceptanceTest.Infrastructure.Pages;
using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using System;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.AcceptanceCriteria.Steps
{
    [Binding]
    public class JobProfileCategorySteps : BaseStep
    {
        //private HomePage homePage;
        //private JobProfilePage jobProfilePage;
        private Uri firstobProfileCategoryUrl;

        //private string jobProfileSelected;
        public JobProfileCategorySteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
        {
        }

        #region Givens

        [Given(@"I am viewing the '(.*)' category page")]
        public void GivenIAmViewingTheCategoryPage(string category)
        {
            category = category?.Replace(" ", "-");
            NavigateToCategoryPage<JobProfileCategoryPage, JobProfileByCategoryViewModel>(category);
        }

        #endregion Givens

        #region Whens

        [When(@"I click on '(.*)' under other categories")]
        public void WhenIClickOnUnderOtherCategories(string category)
        {
            ScenarioContext.Set(category, "category");
            GetNavigatedPage<JobProfileCategoryPage>().GoToCategory<JobProfileCategoryPage>(category).SaveTo(ScenarioContext);
        }

        [When(@"I click on job profile category no '(\d+)'")]
        public void WhenIClickOnJobProfileCategory(int category)
        {
            var homePage = GetNavigatedPage<ExploreCareersPage>();
            firstobProfileCategoryUrl = homePage.GetJobProfileCategoryUrl(category);
            homePage.GoToResult<JobProfileCategoryPage>(category).SaveTo(ScenarioContext);
        }

        [When(@"I click on the no '(\d+)' job profile on the categories page")]
        public void WhenIClickOnTheNoJobProfileOnTheCategoriesPage(int jobProfile)
        {
            var jobProfileCategoryPage = GetNavigatedPage<JobProfileCategoryPage>();
            jobProfileCategoryPage.GetJobProfileByIndex(jobProfile).SaveTo(ScenarioContext, "jobprofileselected");
            jobProfileCategoryPage.GoToResult<JobProfilePage>(jobProfile).SaveTo(ScenarioContext);
        }

        #endregion Whens

        #region Thens

        [Then(@"display the job profile category page")]
        public void ThenDisplayTheJobProfileCategoryPage()
        {
            var jobProfileCategoryPage = GetNavigatedPage<JobProfileCategoryPage>();
            jobProfileCategoryPage.ContainsUrlName(firstobProfileCategoryUrl.OriginalString).Should().BeTrue();
            jobProfileCategoryPage.HasJobProfiles.Should().BeTrue();
        }

        [Then(@"display a list of job profiles")]
        public void ThenDisplayAListOfJobProfiles()
        {
            var jobProfileCategoryPage = GetNavigatedPage<JobProfileCategoryPage>();
            jobProfileCategoryPage.HasJobProfiles.Should().BeTrue();
        }

        [Then(@"display the other job categories section")]
        public void ThenDisplayTheOtherJobCategoriesSection()
        {
            var jobProfileCategoryPage = GetNavigatedPage<JobProfileCategoryPage>();
            jobProfileCategoryPage.HasOtherJobCategoriesSection.Should().BeTrue();
        }

        [Then(@"the '(.*)' category should not be in the other job categories section")]
        public void ThenTheCategoryShouldNotBeInTheOtherJobCategoriesSection(string category)
        {
            category = category?.Replace("\t", " ");
            var jobProfileCategoryPage = GetNavigatedPage<JobProfileCategoryPage>();
            jobProfileCategoryPage.IsCategoryDisplayedInOtherCategorySection(category).Should().BeFalse();
        }

        [Then(@"display the correct category title")]
        public void ThenDisplayTheCorrectCategoryTitle()
        {
            string categorySelected;
            ScenarioContext.TryGetValue("category", out categorySelected);
            var jobProfileCategoryPage = GetNavigatedPage<JobProfileCategoryPage>();
            jobProfileCategoryPage.CategoryTitle.Should().Contain(categorySelected);
        }

        [Then(@"I am redirected to the BAU Job Profile landing page")]
        public void ThenIAmRedirectedToTheBauJobProfileLandingPage()
        {
            var bauJpPage = GetNavigatedPage<BauJpLandingPage>();
            bauJpPage.HasAtoZIndex.Should().BeTrue();
        }

        #endregion
    }
}