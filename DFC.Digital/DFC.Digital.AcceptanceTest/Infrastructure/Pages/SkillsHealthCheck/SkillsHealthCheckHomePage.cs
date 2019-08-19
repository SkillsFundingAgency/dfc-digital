using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class SkillsHealthCheckHomePage : DFCPage
    {
        public T ClickStartANewSkillsHealthCheck<T>()
           where T : UiComponent, new()
        {
            return NavigateTo<T>(By.CssSelector("input.button.button-start"));
        }
    }
}
