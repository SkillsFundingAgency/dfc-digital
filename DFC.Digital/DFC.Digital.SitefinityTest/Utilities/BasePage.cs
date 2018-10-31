using DFC.Digital.SitefinityTest.HelperExtenstions;
using DFC.Digital.SitefinityTest.Pages;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.SitefinityTest.Utilities
{
    public class BasePage
    {
        public static IWebDriver _driver;
        public By UsernameField => By.Id("username");
        public By PasswordField => By.Id("password");
        public By LoginButton => By.Id("loginButton");

        public BasePage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(driver, this);
        }

        internal SitefinityDashboadPage Login()
        {
            PageHelper.EnterText(UsernameField, "");
            PageHelper.EnterText(PasswordField, "");
            PageHelper.ClickElement(LoginButton);
            return new SitefinityDashboadPage(_driver);
        }

    }
}
