using DFC.Digital.SitefinityTest.HelperExtenstions;
using DFC.Digital.SitefinityTest.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.SitefinityTest.Pages
{
    public class SitefinityDashboadPage : BasePage
    {
        public By LinkName => By.ClassName("rmText");
        public SitefinityDashboadPage(IWebDriver webDriver) : base(webDriver)
        {
            PageHelper.VerifyPage(By.Id("sfToMainContent"), "Dashboard");
        }

        public void OpenContentTab()
        {
            PageHelper.ClickLink(LinkName, "Content");
        }

        public SitefinityJobProfilesPage SelectJobProfileLink()
        {
            PageHelper.ClickLink(LinkName, "Job Profiles");
            return new SitefinityJobProfilesPage(_driver);
        }
    }
}
