using DFC.Digital.AcceptanceTest.Infrastructure;
using DFC.Digital.AcceptanceTest.Infrastructure.Pages;
using DFC.Digital.AcceptanceTest.Infrastructure.Pages.SitefinityBackend;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.AcceptanceCriteria.Steps
{
    [Binding]
    public class SitefinitySteps : BaseStep
    {
        public SitefinitySteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
        {
        }

        #region Givens

        [Given(@"I am logged into Sitefinity")]
        public void GivenIAmLoggedIntoSitefinity()
        {
            NavigateToSitefinityBackendPage<SitefinityBackendPage>();
            GetNavigatedPage<SitefinityBackendPage>().Login<SitefinityDashboardPage>()
                .SaveTo(ScenarioContext);
        }

        #endregion

        #region Whens
        [When(@"I go to the CMS Reports page")]
        public void WhenIGoToTheCMSReportsPage()
        {
            var dashboardPage = GetNavigatedPage<SitefinityDashboardPage>();
            dashboardPage.OpenCMSReportsTab();
            dashboardPage.ClickCMSReportsOption<SitefinityCmsReportPage>()
                .SaveTo(ScenarioContext);
        }

        [When(@"I filter by contains on SOC using code (.*)")]
        public void WhenIFilterByContainsOnSOCUsingCode(string socCode)
        {
            var cmsReportsPage = GetNavigatedPage<SitefinityCmsReportPage>();
            cmsReportsPage.EnterFilterCriteria(socCode);
            cmsReportsPage.ApplyFilter<SitefinityCmsReportPage>();
        }

        #endregion

        #region Thens
        [Then(@"I can see the Dashboard page")]
        public void ThenICanSeeTheDashboardPage()
        {
            var dashboardPage = GetNavigatedPage<SitefinityDashboardPage>();
            dashboardPage.VerifyOnDashboardPage().Should().BeTrue();
        }

        [Then(@"the Job Profile report should be displayed")]
        public void ThenTheJobProfileReportShouldBeDisplayed()
        {
            var cmsReportsPage = GetNavigatedPage<SitefinityCmsReportPage>();
            cmsReportsPage.CmsReportId.Should().BeTrue();
        }

        [Then(@"the filter should be applied")]
        public void ThenTheFilterShouldBeApplied()
        {
            GetNavigatedPage<SitefinityCmsReportPage>().IsFilterOptionApplied.Should().BeTrue();
        }

        #endregion

    }
}