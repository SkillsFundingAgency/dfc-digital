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
    public class SitefinityLoginPage : BasePage
    {
        public SitefinityLoginPage(IWebDriver webDriver) : base(webDriver)
        {
            PageHelper.VerifyPage(By.ClassName("ng-binding"), "Login to manage");
        }


    }
}
