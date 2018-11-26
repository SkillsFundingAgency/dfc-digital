using DFC.Digital.AcceptanceTest.Infrastructure.Pages.SitefinityBackend;
using System.Configuration;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class SitefinityBackendPage : Page
    {
        public T Login<T>()
            where T : UiComponent, new()
        {
            Find.Element(By.Id("username")).SendKeys(ConfigurationManager.AppSettings["sfUser"]);
            Find.Element(By.Id("password")).SendKeys(ConfigurationManager.AppSettings["sfPassword"]);

            return Navigate.To<T>(By.Id("loginButton"));
        }
    }
}
