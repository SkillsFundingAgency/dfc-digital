using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OWASPZAPDotNetAPI;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using Xunit;

namespace SecurityTesting
{
    [TestClass]
    public class SecurityTests
    {
        private static readonly string ZapApiKey = ConfigurationManager.AppSettings["apiKey"];
        private static readonly string ZapUrl = ConfigurationManager.AppSettings["host"];
        private static readonly string ZapPort = ConfigurationManager.AppSettings["port"];
        private static readonly string TargetUrl = ConfigurationManager.AppSettings["rooturl"];
        private static readonly string ReportPath = ConfigurationManager.AppSettings["reportPath"];
        private static ClientApi zapClient;
        private IApiResponse response;

        public SecurityTests()
        {
            zapClient = new ClientApi(ZapUrl, Convert.ToInt32(ZapPort), ZapApiKey);
            zapClient.core.excludeFromProxy("^(?:(?!" + TargetUrl + ").)+$");
        }

        private enum ReportFileExtention
        {
            Html,
            Xml,
            Md
        }

        [Fact, Priority(1)]
        public void AExecuteSpider()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ShouldRunSpiderAndScan").ToLower()))
            {
                var spiderId = StartSpidering();
                CheckSpideringProgress(spiderId);
            }
        }

        [Fact]
        public void CheckForHighOrMediumAlerts()
        {
            ApiResponseSet alertSummary = (ApiResponseSet)zapClient.core.alertsSummary(TargetUrl);
            alertSummary.Dictionary.TryGetValue("High", out var high);
            alertSummary.Dictionary.TryGetValue("Medium", out var medium);

            Convert.ToInt32(high).Should().Be(0);
            Convert.ToInt32(medium).Should().Be(0);
        }

        [Fact, Priority(2)]
        public void BExecuteActiveScan()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ShouldRunSpiderAndScan").ToLower()))
            {
                var activeScanId = StartActiveScan();
                CheckActiveScanProgress(activeScanId);
                var reportFilename = $"{DateTime.Now:dd.MM.yy-hh.mm.ss}-ZAP_Report";
                SaveSession(reportFilename);
                zapClient.Dispose();

                GenerateReport(reportFilename, ReportFileExtention.Html);
            }
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

        private static void GenerateReport(string filename, ReportFileExtention fileExtention)
        {
            var fileName = $@"{ReportPath}\{filename}.{fileExtention.ToString().ToLower()}";
            File.WriteAllBytes(fileName, zapClient.core.xmlreport());
        }

        private static void SaveSession(string reportFilename)
        {
            var sessionReportPath = ReportPath + "\\Sessions\\" + DateTime.Now.ToString("dd.MM.yy");
            zapClient.core.saveSession($@"{sessionReportPath}\{reportFilename}", "true");
        }

        private static void CheckSpideringProgress(string spideringId)
        {
            int progress;
            while (true)
            {
                Thread.Sleep(10000);
                progress = int.Parse(((ApiResponseElement)zapClient.spider.status(spideringId)).Value);
                if (progress >= 100)
                {
                    break;
                }
            }

            Thread.Sleep(5000);
        }

        private string StartSpidering()
        {
            response = zapClient.spider.scan(TargetUrl, string.Empty, string.Empty, string.Empty, string.Empty);
            return ((ApiResponseElement)response).Value;
        }

        private string StartActiveScan()
        {
            response = zapClient.ascan.scan(TargetUrl, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            return ((ApiResponseElement)response).Value;
        }
    }
}