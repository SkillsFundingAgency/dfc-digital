using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages.SitefinityBackend
{
    public class SitefinityCmsReportPage : Page
    {
        public bool CmsReportId => Find.Element(By.Id("jobProfileReport")) != null;

        public bool IsFilterOptionApplied => Find.Element(By.ClassName("applied")) != null;

        public void EnterFilterCriteria(string soc)
        {
            var socFilterLink = Find.Element(By.CssSelector("thead > tr > th:nth-child(5) > a"));
            socFilterLink.Click();

            EnterText(soc.Replace("'", string.Empty));
        }

        public void EnterText(string text)
        {
            Find.Element(By.CssSelector("body > div > div.popup-content > div.first-filter > div:nth-child(2) > input")).SendKeys(text);
        }

        public T ApplyFilter<T>()
            where T : UiComponent, new()
        {
            return Navigate.To<T>(By.ClassName("mvc-grid-apply"));
        }
    }
}
