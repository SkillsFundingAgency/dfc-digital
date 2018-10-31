using DFC.Digital.SitefinityTest.HelperExtenstions;
using DFC.Digital.SitefinityTest.Pages;
using DFC.Digital.SitefinityTest.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace DFC.Digital.SitefinityTest.Steps
{
    [Binding]
    public class SfContentSteps : BaseTest
    {

        #region Givens
        [Given("I log into Sitefinity")]
        public void GivenILoginToSitefinity()
        {
            _driver.Url = AppSettings.GetAppSettings().GetBaseUrl();
            SitefinityLoginPage sfLogin = new SitefinityLoginPage(_driver);
            sfLogin.Login();
        }
        #endregion

        #region Whens

        [When(@"I open the Content dropdown")]
        public void WhenIOpenTheContentDropdown()
        {
            SitefinityDashboadPage sfDashboard = new SitefinityDashboadPage(_driver);
            sfDashboard.OpenContentTab();
        }

        [When(@"I select the '(.*)' link")]
        public void WhenISelectTheLink(string link)
        {
            switch (link)
            {
                case "Job Profiles":
                    SitefinityDashboadPage sfDashboard = new SitefinityDashboadPage(_driver);
                    sfDashboard.SelectJobProfileLink();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Thens
        [Then(@"log off sitefinity")]
        public void ThenLogoffSitefinity()
        {
            GenericPage page = new GenericPage(_driver);
            page.LogOut();
        }

        [Then(@"I am redirected to the '(.*)' content page")]
        public void ThenIAmRedirectedToTheContentPage(string page)
        {
            switch (page)
            {
                case "Job Profiles":
                    PageHelper.VerifyPageHeader("Job Profiles");
                    break;
                default:
                    break;
            }
        }

        #endregion


    }
}
