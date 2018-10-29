using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.Digital.SitefinityTest.HelperExtenstions
{
    public class PageHelper
    {
        public static IWebDriver _driver;

        public static void SetDriver(IWebDriver webDriver)
        {
            _driver = webDriver;
        }

        public static bool VerifyPage(By by, string expected)
        {
            Thread.Sleep(500);

            var actual = _driver.FindElement(by).Text;
            if (actual.Contains(expected))
            {
                return true;
            }
            else if (_driver.Url.Contains("SignOut/selflogout"))
            {
                _driver.FindElement(By.ClassName("sfLinkBtnIn")).Click();
                IAlert alert = _driver.SwitchTo().Alert();
                alert.Accept();
                return true;
            }
            else
            {
                throw new Exception("Page verification failed");
            }
        }

        public static void EnterText(By by, string text)
        {
            _driver.FindElement(by).Clear();
            _driver.FindElement(by).SendKeys(text);
        }

        public static void ClickElement(By by)
        {
            _driver.FindElement(by).Click();
        }

        public static void ClickLink(By by, string TabName)
        {
            List<IWebElement> Tabs = _driver.FindElements(by).ToList();
            var x = Tabs.Where(t => t.Text.Equals(TabName)).FirstOrDefault();
            x.Click();

        }

    }
}
