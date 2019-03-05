using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class BauProfilePage : DFCPage
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
    }
}
