using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class BauProfilePage : Page
    {
        public bool UrlContains(string urlFragment)
        {
            return Browser.Url.ToUpperInvariant().Contains(urlFragment?.ToUpperInvariant());
        }

        public T ClickBetaBanner<T>()
            where T : UiComponent, new()
        {
            return NavigateTo<T>(By.ClassName("betaBanner"));
        }

        protected virtual TPage NavigateTo<TPage>(By by, int waitTimeout = 10)
            where TPage : UiComponent, new()
        {
            var element = Find.Element(by);
            var resultPage = Navigate.To<TPage>(by);
            var wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(waitTimeout));
            wait.Until(ExpectedConditions.StalenessOf(element));

            return resultPage;
        }
    }
}
