using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    internal static class HelperExtensions
    {
        public static OpenQA.Selenium.IWebElement ScrollToElement(this OpenQA.Selenium.IWebElement element, IExecutor execute)
        {
            if (element != null)
            {
                var js = string.Format("window.scroll(" + element.Location.X + "," + element.Location.Y + ");");
                execute.Script(js);
                return element;
            }
            else
            {
                return element;
            }
        }

        internal static bool IsShown(this string isShownFlag)
        {
            return isShownFlag.Equals("shown", StringComparison.InvariantCultureIgnoreCase);
        }

        internal static string UniqueScenarioTitle(this ScenarioContext scenarioContext)
        {
            var title = scenarioContext.ScenarioInfo.Title;
            var exampleTags = scenarioContext.ScenarioInfo.Tags.FirstOrDefault(t => t.StartsWith("args:", StringComparison.Ordinal));
            return
                string.IsNullOrEmpty(exampleTags)
                    ? title
                    : $"{title}-{exampleTags}";
        }

        internal static TPage SaveTo<TPage>(this TPage page, ScenarioContext scenarioContext)
            where TPage : UiComponent, new()
        {
            scenarioContext.Set(page);
            return page;
        }

        internal static string FormatTokens(this string value, ScenarioContext scenarioContext = null)
        {
            return Regex.Replace(value, @"{([^}]+)}", (m) =>
            {
                switch (m.Groups[1].Value.ToUpperInvariant())
                {
                    case "NOW":
                        return DateTime.Now.ToString("ddMMyyyy-HHmm");

                    case "TODAY":
                        return DateTime.Now.ToString("yyyyMMdd");

                    case "SCENARIOTITLE":
                        return (scenarioContext ?? ScenarioContext.Current).UniqueScenarioTitle();

                    default:
                        return m.Value;
                }
            });
        }

        internal static TPage NavigateAndWaitForStalenessTo<TPage>(this IElementFinder finder, IPageNavigator navigator, IWebDriver browser, string url, By checkUsing, int waitTimeout = 10)
            where TPage : UiComponent, new()
        {
            var element = finder.Element(checkUsing);
            var resultPage = navigator.To<TPage>(url);
            var wait = new WebDriverWait(browser, TimeSpan.FromSeconds(waitTimeout));
            wait.Until(ExpectedConditions.StalenessOf(element));

            return resultPage;
        }

        internal static TPage NavigateAndWaitForStalenessTo<TPage>(this IElementFinder finder, IPageNavigator navigator, IWebDriver browser, By by, int waitTimeout = 10)
           where TPage : UiComponent, new()
        {
            var element = finder.Element(by);
            var resultPage = navigator.To<TPage>(by);
            var wait = new WebDriverWait(browser, TimeSpan.FromSeconds(waitTimeout));
            wait.Until(ExpectedConditions.StalenessOf(element));

            return resultPage;
        }

        internal static bool IsThisStatusPage(this IWebDriver browser)
        {
            try
            {
                return browser.Url.Contains("/health/status");
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        internal static IWebElement OptionalElementNoWait(this IElementFinder finder, By by)
        {
            return finder is DfcElementFinder dfcElementFinder ? dfcElementFinder.OptionalElementNoExplicitWait(by) : finder.OptionalElement(by);
        }
    }
}