using System;
using System.Configuration;
using DFC.Digital.SitefinityTest.HelperExtenstions;
using DFC.Digital.SitefinityTest.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace DFC.Digital.SitefinityTest.Utilities
{
    [Binding]
    public class BaseTest
    {

        public static IWebDriver _driver;

        [Before]
        public static void SetUp()
        {
            string browser = AppSettings.GetAppSettings().GetBrowser();
            switch (browser)
            {
                case "chrome":
                    _driver = new ChromeDriver();
                    break;
                default:
                    throw new Exception("Incorect browser specified OR this framework does not support the browser");
            }

            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Manage().Cookies.DeleteAllCookies();
            string currentWindow = _driver.CurrentWindowHandle;
            _driver.SwitchTo().Window(currentWindow);

            PageHelper.SetDriver(_driver);
        }

        [After]
        public static void Teardown()
        {
            _driver.Quit();
        }

    }
}
