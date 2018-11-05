using DFC.Digital.SitefinityTest.HelperExtenstions;
using DFC.Digital.SitefinityTest.Pages;
using DFC.Digital.SitefinityTest.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Excel = Microsoft.Office.Interop.Excel;


namespace DFC.Digital.SitefinityTest.Steps
{
    [Binding]
    public class SfContentSteps : BaseTest
    {

        #region Givens
        [Given("I log into Sitefinity")]
        public void GivenILoginToSitefinity()
        {
            _driver.Url = AppSettings.GetAppSettings().GetBaseUrl();
            SitefinityLoginPage sfLogin = new SitefinityLoginPage(_driver);
            sfLogin.Login();
        }

        [Given(@"I navigate to the homepage")]
        public void GivenINavigateToTheHomepage()
        {
            _driver.Url = "https://staging-beta.nationalcareersservice.direct.gov.uk/";
        }

        #endregion

        #region Whens

        [When(@"I open the Content dropdown")]
        public void WhenIOpenTheContentDropdown()
        {
            SitefinityDashboadPage sfDashboard = new SitefinityDashboadPage(_driver);
            sfDashboard.OpenContentTab();
        }

        [When(@"I select the '(.*)' link")]
        public void WhenISelectTheLink(string link)
        {
            switch (link)
            {
                case "Job Profiles":
                    SitefinityDashboadPage sfDashboard = new SitefinityDashboadPage(_driver);
                    sfDashboard.SelectJobProfileLink();
                    break;
                default:
                    break;
            }
        }

        [When(@"I search for the data items")]
        public void WhenISearchFor()
        {
            PageHelper page = new PageHelper();

            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook = app.Workbooks.Open(@"C:\Users\Asus\Documents\SFA\SFA.xlsx");
            Excel._Worksheet worksheet = workbook.Sheets[1];
            Excel.Range range = worksheet.UsedRange;

            int i = 2;
            foreach (var row in range.Rows)
            {
                string search = worksheet.Cells[i, 1].Value.ToString();
                page.Search(search);

                worksheet.Cells[i, 2].Value = page.GetNumberOfResults();
                worksheet.Cells[i, 3].Value = page.GetTopResults();
                workbook.Save();
                page.ClickHomeLink();

                i++;
            }
            workbook.Close();
        }

        #endregion

        #region Thens
        [Then(@"log off sitefinity")]
        public void ThenLogoffSitefinity()
        {
            GenericPage page = new GenericPage(_driver);
            page.LogOut();
        }

        [Then(@"I am redirected to the '(.*)' content page")]
        public void ThenIAmRedirectedToTheContentPage(string page)
        {
            switch (page)
            {
                case "Job Profiles":
                    PageHelper.VerifyPageHeader("Job Profiles");
                    break;
                default:
                    break;
            }
        }

        #endregion


    }
}
