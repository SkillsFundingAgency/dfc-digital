using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel;

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
                throw new Exception("Page verification failed Expected Page: " + expected + ", Actual Page: " + actual);
            }
        }

        public static bool VerifyPageHeader(string expected)
        {
            Thread.Sleep(500);

            var actual = _driver.FindElement(By.ClassName("sfViewTitle")).Text;
            if (actual.Contains(expected))
            {
                return true;
            }
            else
            {
                throw new Exception("Landed on the incorrect page. Expected Page: " + expected + ", Actual Page: " + actual);
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
            var Element = Tabs.Where(t => t.Text.Equals(TabName)).FirstOrDefault();
            Element.Click();

        }

        public void Search(string search)
        {
            _driver.FindElement(By.ClassName("search-input")).Clear();
            _driver.FindElement(By.ClassName("search-input")).SendKeys(search);
            _driver.FindElement(By.ClassName("submit")).Click();
        }

        public string GetNumberOfResults()
        {
            Thread.Sleep(250);
            return _driver.FindElement(By.ClassName("result-count")).Text;
        }

        public void ClickHomeLink()
        {
            _driver.FindElement(By.Id("nav-EC")).Click();
        }

        public string GetTopResults()
        {

            if (_driver.FindElement(By.ClassName("result-count")).Text.Equals("0 results found - try again using a different job title"))
            {
                return "0 Results Found";
            }
            else
            {
                var x = _driver.FindElements(By.ClassName("dfc-code-search-jpTitle")).ToList();
                return string.Concat(x.Select(s => s.Text + ", "));
            }

        }
    }
}
