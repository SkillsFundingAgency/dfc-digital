using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
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
            return Navigate.To<T>(By.ClassName("betaBanner"));
        }
    }
}
