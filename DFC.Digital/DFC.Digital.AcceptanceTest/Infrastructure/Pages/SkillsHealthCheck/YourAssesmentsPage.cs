using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class YourAssesmentsPage : DFCPage
    {
        public string PageTitle => Find.Element(By.ClassName("heading-xlarge")).Text;

        public T StartASkillsHealthCheck<T>(string typeOfSkillsHealthCheck)
           where T : UiComponent, new()
        {
            return NavigateTo<T>(By.CssSelector($"a[href*='{typeOfSkillsHealthCheck}']"));
        }

        public void DownLoadSkillsHeathCheckReport(string reportDownLoadType)
        {
            Find.Element(By.XPath($"//input[@value='{reportDownLoadType}']")).Click();
            Find.Element(By.XPath("//input[@value='Download your report']")).Click();
        }
    }
}
