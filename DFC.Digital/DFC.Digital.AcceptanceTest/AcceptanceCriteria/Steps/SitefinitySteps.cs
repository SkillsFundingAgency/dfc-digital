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

        [Given(@"I am logged into Sitefinity")]
        public void GivenIAmLoggedIntoSitefinity()
        {
            NavigateToSitefinityBackendPage<SitefinityBackendPage>();
            GetNavigatedPage<SitefinityBackendPage>().Login<SitefinityDashboardPage>()
                .SaveTo(ScenarioContext);
            var dashboardPage = GetNavigatedPage<SitefinityDashboardPage>();
            dashboardPage.VerifyOnDashboardPage().Should().BeTrue();
        }
    }
}
