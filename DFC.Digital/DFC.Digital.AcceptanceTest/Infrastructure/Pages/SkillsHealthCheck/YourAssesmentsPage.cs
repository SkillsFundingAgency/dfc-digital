using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;
using Xunit;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Pages
{
    public class YourAssesmentsPage : DFCPage
    {
        public string PageTitle => Find.Element(By.ClassName("heading-xlarge")).Text;

        public string FirstCompletedRowName => Find.Element(By.XPath("//[@id='shc']/tbody/tr/td[1]")).Text;

        public T StartASkillsHealthCheck<T>(string typeOfSkillsHealthCheck)
           where T : UiComponent, new()
        {
            return NavigateTo<T>(By.CssSelector($"a[href*='{typeOfSkillsHealthCheck}']"));
        }

        public T ClickYourAccountLink<T>()
          where T : UiComponent, new()
        {
            return NavigateTo<T>(By.Id("myaccount-link"));
        }

        public void DownLoadSkillsHeathCheckReport(string reportDownLoadType)
        {
            //I can click on the download buttons and that they are there
            Find.Element(By.XPath($"//input[@value='{reportDownLoadType}']")).Click();
            Find.Element(By.XPath("//input[@value='Download your report']")).Click();

            //The server does return a  file of the correct type
            var downloadRequestType = reportDownLoadType.ToLowerInvariant();
            var responseType = downloadRequestType == "word" ? "docx" : downloadRequestType;
            CheckFileIsDownDownLoaded(downloadRequestType, responseType);
        }

        private void CheckFileIsDownDownLoaded(string downloadRequestType, string expectedResponseType)
        {
            object response = ((IJavaScriptExecutor)Browser).ExecuteAsyncScript(
               "var url = arguments[0];" +
               "var callback = arguments[arguments.length - 1];" +
               $"var params = 'DownLoadType={downloadRequestType}';" +
               "var xhr = new XMLHttpRequest();" +
               "xhr.open('POST', url , true);" +
               "xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');" +
               "xhr.onreadystatechange = function() {" +
               "  if (xhr.readyState == 4) {" +
               "    callback(xhr.getAllResponseHeaders());" +
               "  }" +
               "};" +
               "xhr.send(params);", "/skills-health-check/your-assessments/DownloadDocument/");

            Assert.Contains($"content-type: application/{expectedResponseType}", response.ToString());
        }
    }
}
