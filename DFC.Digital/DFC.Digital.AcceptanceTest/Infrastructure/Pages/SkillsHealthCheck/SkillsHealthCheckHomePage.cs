using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class SkillsHealthCheckHomePage : DFCPage
    {
        public string PageTitle => Find.Element(By.ClassName("heading-xlarge")).Text;

        public string SignOutLinkText => Find.Element(By.Id("signout-link")).Text;

        public string SectionHeading => Find.Element(By.ClassName("heading-large")).Text;

        public T ClickStartANewSkillsHealthCheck<T>()
           where T : UiComponent, new()
        {
            return NavigateTo<T>(By.CssSelector("input.button.button-start"));
        }

        public T ClickSignIn<T>()
         where T : UiComponent, new()
        {
            return NavigateTo<T>(By.Id("signin-link"));
        }

        public T ClickShowMySkillHealthCheck<T>()
        where T : UiComponent, new()
        {
            return NavigateTo<T>(By.XPath("//input[@value='Show my Skills Health Check documents']"));
        }
    }
}
