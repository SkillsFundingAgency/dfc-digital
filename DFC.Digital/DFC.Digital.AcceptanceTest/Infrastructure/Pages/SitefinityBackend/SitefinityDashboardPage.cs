using System;
using System.Linq;
using System.Threading;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages.SitefinityBackend
{
    public class SitefinityDashboardPage : Page
    {
        public bool VerifyOnDashboardPage()
        {
            var actualPage = Find.Element(By.Id("sfToMainContent")).Text;
            if (actualPage.Equals("Dashboard"))
            {
                return true;
            }
            else if (actualPage.Equals("Sitefinity"))
            {
                Find.Element(By.ClassName("sfLinkBtnIn")).Click();
                OpenQA.Selenium.IAlert alert = Browser.SwitchTo().Alert();
                alert.Accept();
                var page = Find.Element(By.Id("sfToMainContent")).Text;
                if (page.Equals("Dashboard"))
                {
                    return true;
                }
                else
                {
                    throw new Exception("Did not logout previous user correctly");
                }
            }
            else
            {
                throw new Exception("Unsuccessful login");
            }
        }

        public void OpenCMSReportsTab()
        {
            var listOfTabs = Find.Elements(By.ClassName("rmText")).ToList();
            var selectedTab = listOfTabs.Where(t => t.Text.Equals("CMS Reports")).FirstOrDefault();
            selectedTab.Click();
        }

        public T ClickCMSReportsOption<T>()
            where T : UiComponent, new()
        {
            return Navigate.To<T>(By.LinkText("Job Profile Apprenticeship Vacancies"));
        }
    }
}
