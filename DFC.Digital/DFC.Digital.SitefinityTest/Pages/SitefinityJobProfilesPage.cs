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
    public class SitefinityJobProfilesPage : BasePage
    {
        public SitefinityJobProfilesPage(IWebDriver webDriver) : base(webDriver)
        {
            PageHelper.VerifyPage(By.ClassName("sfViewTitle"), "Job Profiles");

        }
    }
}
