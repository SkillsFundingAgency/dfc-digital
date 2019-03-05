using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using TestStack.Seleno.PageObjects.Actions;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    internal class DfcElementFinder : IElementFinder
    {
        private readonly IElementFinder find;
        private readonly IExecutor execute;
        private readonly IWait waitFor;
        private readonly IWebDriver browser;

        public DfcElementFinder(IElementFinder find, IExecutor execute, IWait wait, IWebDriver browser)
        {
            this.execute = execute;
            this.waitFor = wait;
            this.browser = browser;
            this.find = find;
        }

        public IWebElement Element(By findExpression, TimeSpan maxWait = default(TimeSpan))
        {
            ExplicitCheckForPageReady(findExpression, maxWait);
            return find.Element(findExpression, maxWait).ScrollToElement(execute);
        }

        public IWebElement Element(TestStack.Seleno.PageObjects.Locators.By.jQueryBy jQueryFindExpression, TimeSpan maxWait = default(TimeSpan))
        {
            return find.Element(jQueryFindExpression, maxWait).ScrollToElement(execute);
        }

        public IEnumerable<IWebElement> Elements(By findExpression, TimeSpan maxWait = default(TimeSpan))
        {
            ExplicitCheckForPageReady(findExpression, maxWait);
            return find.Elements(findExpression, maxWait);
        }

        public IEnumerable<IWebElement> Elements(TestStack.Seleno.PageObjects.Locators.By.jQueryBy jQueryFindExpression, TimeSpan maxWait = default(TimeSpan))
        {
            return find.Elements(jQueryFindExpression, maxWait);
        }

        public IWebElement ElementWithWait(By findElement, int waitInSeconds = 20)
        {
            ExplicitCheckForPageReady(findElement, explicitWaitSeconds: waitInSeconds);
            return find.Element(findElement);
        }

        public IWebElement OptionalElement(By findExpression, TimeSpan maxWait = default(TimeSpan))
        {
            ExplicitCheckForPageReady(findExpression, maxWait);
            return find.OptionalElement(findExpression, maxWait).ScrollToElement(execute);
        }

        public IWebElement OptionalElement(TestStack.Seleno.PageObjects.Locators.By.jQueryBy jQueryFindExpression, TimeSpan maxWait = default(TimeSpan))
        {
            return find.OptionalElement(jQueryFindExpression, maxWait).ScrollToElement(execute);
        }

        public IWebElement TryFindElement(By by)
        {
            ExplicitCheckForPageReady(by);
            return find.OptionalElement(by);
        }

        public IWebElement TryFindElementByjQuery(TestStack.Seleno.PageObjects.Locators.By.jQueryBy by)
        {
            return find.OptionalElement(by);
        }

        public IWebElement OptionalElementNoExplicitWait(By by)
        {
            return find.OptionalElement(by).ScrollToElement(execute);
        }

        private void ExplicitCheckForPageReady(By by, TimeSpan maxWait = default(TimeSpan), int explicitWaitSeconds = 20)
        {
            waitFor.AjaxCallsToComplete(new TimeSpan(0, 0, explicitWaitSeconds));

            var wait = new WebDriverWait(browser, TimeSpan.FromSeconds(explicitWaitSeconds));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
        }
    }
}