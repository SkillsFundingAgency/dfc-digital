﻿using DFC.Digital.AcceptanceTest.Infrastructure.Config;
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

        private CookiesPage cookiesPage;

        public HomepageSteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
        {
        }

        #region Givens

        [Given(@"that I am viewing the Home page")]
        public void GivenThatIAmViewingTheHomepage()
        {
            NavigateToHomePage<Homepage, JobProfileSearchBoxViewModel>();
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

        [When(@"I click on the Cookies link")]
        public void WhenIClickOnTheCookiesLink()
        {
            cookiesPage = GetNavigatedPage<Homepage>()?.ClickCookiesLink<CookiesPage>();
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

        [Then(@"I am redirected to the cookies page")]
        public void ThenIAmRedirectedToTheCookiesPage()
        {
            cookiesPage.CookiesHeadingText.Should().Be("Privacy and cookies");
        }

        [Then(@"I am redirected to the homepage")]
        public void ThenIAmRedirectedToTheHomepage()
        {
            GetNavigatedPage<Homepage>()?.ServiceName.Should().BeTrue();
        }

        #endregion Thens
    }
}