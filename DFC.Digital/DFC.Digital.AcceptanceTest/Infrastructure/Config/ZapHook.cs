using DFC.Digital.AutomationTest.Utilities;
using OWASPZAPDotNetAPI;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    [Binding]
    public static class ZapHook
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        private static readonly string ZapApiKey = ConfigurationManager.AppSettings["zapApiKey"];

        private static readonly string ZapUrl = ConfigurationManager.AppSettings["zapHost"];
        private static readonly string ZapPort = ConfigurationManager.AppSettings["zapPort"];
        private static readonly string ReportPath = ConfigurationManager.AppSettings["zapReportPath"];
        private static readonly RunProfile RunProfile = (RunProfile)Enum.Parse(typeof(RunProfile), ConfigurationManager.AppSettings.Get("RunProfile"), true);
        private static ClientApi zapClient;
        private static IApiResponse response;

        private static string TargetUrl
        {
            get
            {
#if DEBUG
                return $"http://localhost:60876/";
#else
                return ConfigurationManager.AppSettings["rooturl"];
#endif
            }
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            if (RunProfile == RunProfile.Owasp)
            {
                zapClient = new ClientApi(ZapUrl, Convert.ToInt32(ZapPort), ZapApiKey);
                zapClient.core.excludeFromProxy("^(?:(?!" + TargetUrl + ").)+$");
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            if (RunProfile == RunProfile.Owasp)
            {
                var activeScanId = StartActiveScan();
                CheckActiveScanProgress(activeScanId);
                var reportFilename = $"{DateTime.Now:dd.MM.yy-hh.mm}_Selenium_Zap_Report";

                SaveSession(reportFilename);
                GenerateHtmlReport(reportFilename);
            }
        }

        private static string StartActiveScan()
        {
            response = zapClient.ascan.scan(TargetUrl, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            return ((ApiResponseElement)response).Value;
        }

        private static void CheckActiveScanProgress(string activeScanId)
        {
            int progress;
            while (true)
            {
                Thread.Sleep(10000);
                progress = int.Parse(((ApiResponseElement)zapClient.ascan.status(activeScanId)).Value);

                if (progress >= 100)
                {
                    break;
                }
            }

            Thread.Sleep(5000);
        }

        private static void GenerateHtmlReport(string filename)
        {
            var fileName = $@"{ReportPath}\{filename}.html";
            File.WriteAllBytes(fileName, zapClient.core.htmlreport());
        }

        private static void SaveSession(string reportFilename)
        {
            var sessionReportPath = ReportPath + "\\Sessions\\" + DateTime.Now.ToString("dd.MM.yy");
            zapClient.core.saveSession($@"{sessionReportPath}\{reportFilename}", "true");
        }

        private static void CheckForHighOrMediumAlerts()
        {
            ApiResponseSet alertSummary = (ApiResponseSet)zapClient.core.alertsSummary(TargetUrl);
            alertSummary.Dictionary.TryGetValue("High", out var high);
            alertSummary.Dictionary.TryGetValue("Medium", out var medium);

            if (Convert.ToInt32(high) > 0 || Convert.ToInt32(medium) > 0)
            {
                throw new TestException("High or Medium alert has been found");
            }
        }
    }
}