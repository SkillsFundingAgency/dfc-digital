using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using TestStack.Seleno.PageObjects.Actions;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    internal class DfcElementFinder : IElementFinder
    {
        private IElementFinder find;
        private IExecutor execute;

        public DfcElementFinder(IElementFinder find, IExecutor execute)
        {
            this.execute = execute;
            this.find = find;
        }

        public IWebElement Element(By findExpression, TimeSpan maxWait = default(TimeSpan))
        {
            return find.Element(findExpression, maxWait).ScrollToElement(execute);
        }

        public IWebElement Element(TestStack.Seleno.PageObjects.Locators.By.jQueryBy jQueryFindExpression, TimeSpan maxWait = default(TimeSpan))
        {
            return find.Element(jQueryFindExpression, maxWait).ScrollToElement(execute);
        }

        public IEnumerable<IWebElement> Elements(By findExpression, TimeSpan maxWait = default(TimeSpan))
        {
            return find.Elements(findExpression, maxWait);
        }

        public IEnumerable<IWebElement> Elements(TestStack.Seleno.PageObjects.Locators.By.jQueryBy jQueryFindExpression, TimeSpan maxWait = default(TimeSpan))
        {
            return find.Elements(jQueryFindExpression, maxWait);
        }

        public IWebElement ElementWithWait(By findElement, int waitInSeconds = 20)
        {
            return find.Element(findElement);
        }

        public IWebElement OptionalElement(By findExpression, TimeSpan maxWait = default(TimeSpan))
        {
            return find.OptionalElement(findExpression, maxWait).ScrollToElement(execute);
        }

        public IWebElement OptionalElement(TestStack.Seleno.PageObjects.Locators.By.jQueryBy jQueryFindExpression, TimeSpan maxWait = default(TimeSpan))
        {
            return find.OptionalElement(jQueryFindExpression, maxWait).ScrollToElement(execute);
        }

        public IWebElement TryFindElement(By by)
        {
            return find.OptionalElement(by);
        }

        public IWebElement TryFindElementByjQuery(TestStack.Seleno.PageObjects.Locators.By.jQueryBy by)
        {
            return find.OptionalElement(by);
        }
    }
}