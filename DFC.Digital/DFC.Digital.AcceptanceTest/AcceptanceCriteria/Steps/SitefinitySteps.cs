using DFC.Digital.AcceptanceTest.Infrastructure;
using DFC.Digital.AcceptanceTest.Infrastructure.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            SitefinityBackendPage
        }
    }
}
