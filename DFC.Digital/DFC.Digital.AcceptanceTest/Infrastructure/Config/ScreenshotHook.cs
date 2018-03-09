using DFC.Digital.AcceptanceTest.Infrastructure.Config;
using System;
using System.IO;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    [Binding]
    public class ScreenshotHook
    {
        private ScenarioContext scenarioContext;

        public ScreenshotHook(ITestOutputHelper outputHelper, ScenarioContext scenarioContext, BrowserStackSelenoHost browserStackSelenoHost)
        {
            OutputHelper = outputHelper;
            this.scenarioContext = scenarioContext;
            BrowserStackSelenoHost = browserStackSelenoHost;
        }

        public BrowserStackSelenoHost BrowserStackSelenoHost { get; private set; }

        private ITestOutputHelper OutputHelper { get; set; }

        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        [AfterScenario]
        public void AfterScenario()
        {
            string testInfoName = $"{scenarioContext.ScenarioInfo.Title.Substring(0, scenarioContext.ScenarioInfo.Title.LastIndexOf(']') + 1)}";

            //Take a screen shot and log the filename to output
            OutputHelper.WriteLine($"Saved error screenshot {TakeScreenShot($"{testInfoName}")}");
        }

        /// <summary>
        /// Take a screen shot of the current browser screen in the test
        /// </summary>
        /// <param name="screenShotName">Name of the file to save the screen shot under</param>
        /// <returns>The full path and filename that the screenshot got saved as</returns>
        private string TakeScreenShot(string screenShotName)
        {
            if (BrowserStackSelenoHost.RunProfile == RunProfile.Local)
            {
                var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                var r = new Regex($"[{Regex.Escape(regexSearch)}]");
                var screenShotFile = $"{r.Replace(screenShotName, string.Empty)}_{DateTime.Now:dd-MMM-yy-H-mm-ss}.jpg";
                BrowserStackSelenoHost.Seleno.Application.Camera.TakeScreenshot(screenShotFile);
                return $"{LocalBrowserHost.ScreenshotFolder}\\{screenShotFile}";
            }
            else
            {
                return "No screenshot taken - Running in remote bowserstack mode";
            }
        }
    }
}