using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class BauSearchPage : Page
    {
        public bool ResultsSectionDisplated => Find.Element(OpenQA.Selenium.By.ClassName("results-block")) != null;

        public string PopulatedSearchText => Find.Element(OpenQA.Selenium.By.ClassName("form-control")).GetAttribute("value");

        public bool UrlContains(string urlFragment)
        {
            return Browser.Url.ToUpperInvariant().Contains(urlFragment?.ToUpperInvariant());
        }
    }
}
