using System;
using System.Threading;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages.SitefinityBackend
{
    public class SitefinityDashboardPage : SitefinityBasePage
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

        public void OpenAdminTab()
        {
            ClickLink("Administration");
            ClickLink("Export / Import");
        }

        public void ClickContinueButton()
        {
            Find.Element(By.ClassName("sfLinkBtnIn")).Click();
        }

        public void SelectPages()
        {
            ClickLink("sfTxtLbl", "Pages");
        }

        public void ClickExport()
        {
            Find.Element(By.ClassName("sfSave")).Click();
            Thread.Sleep(5000);
        }
    }
}
