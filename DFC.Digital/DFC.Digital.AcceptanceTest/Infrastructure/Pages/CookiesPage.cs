using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class CookiesPage : Page
    {
        public string CookiesHeadingText => Find.OptionalElement(By.ClassName("heading-xlarge")).Text;
    }
}