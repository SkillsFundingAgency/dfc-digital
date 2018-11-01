using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages.SitefinityBackend
{
    public class SitefinityBasePage : Page
    {
        public void ClickLink(string tabName)
        {
            var listOfTabs = Find.Elements(By.ClassName("rmText")).ToList();
            var selectedTab = listOfTabs.Where(t => t.Text.Equals(tabName)).FirstOrDefault();
            selectedTab.Click();
        }

        public void ClickLink(string className, string tabName)
        {
            var listOfTabs = Find.Elements(By.ClassName(className)).ToList();
            var selectedTab = listOfTabs.Where(t => t.Text.Equals(tabName)).FirstOrDefault();
            selectedTab.Click();
        }
    }
}
