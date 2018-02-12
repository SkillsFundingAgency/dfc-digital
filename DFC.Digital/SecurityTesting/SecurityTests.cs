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
        private static ClientApi ZapClient;
        private IApiResponse response;

        public SecurityTests()
        {
            ZapClient = new ClientApi(ZapUrl, Convert.ToInt32(ZapPort), ZapApiKey);
            ZapClient.core.excludeFromProxy("^(?:(?!" + TargetUrl + ").)+$");
        }

        [Fact, Priority(1)]
        public void AExecuteSpider()
        {
            var spiderId = StartSpidering();
            CheckSpideringProgress(spiderId);
        }

        [Fact, Priority(2)]
        public void BExecuteActiveScan()
        {
            var activeScanId = StartActiveScan();
            CheckActiveScanProgress(activeScanId);
            var reportFilename = $"{DateTime.Now:dd.MM.yy-hh.mm.ss}-ZAP_Report";
            SaveSession(reportFilename);
            ZapClient.Dispose();

            GenerateHtmlReport(reportFilename);
        }

        private static void CheckActiveScanProgress(string activeScanId)
        {
            int progress;
            while (true)
            {
                Thread.Sleep(10000);
                progress = int.Parse(((ApiResponseElement)ZapClient.ascan.status(activeScanId)).Value);

                if (progress >= 100)
                {
                    break;
                }
            }

            Thread.Sleep(5000);
        }

        private static void GenerateXmlReport(string filename)
        {
            var fileName = $@"{ReportPath}\{filename}.xml";
            File.WriteAllBytes(fileName, ZapClient.core.xmlreport());
        }

        private static void GenerateHtmlReport(string filename)
        {
            var fileName = $@"{ReportPath}\{filename}.html";
            File.WriteAllBytes(fileName, ZapClient.core.htmlreport());
        }

        private static void GenerateMarkdownReport(string filename)
        {
            var fileName = $@"{ReportPath}\{filename}.md";
            File.WriteAllBytes(fileName, ZapClient.core.mdreport());
        }

        private static void SaveSession(string reportFilename)
        {
            var sessionReportPath = ReportPath + "\\Sessions\\" + DateTime.Now.ToString("dd.MM.yy");
            ZapClient.core.saveSession($@"{sessionReportPath}\{reportFilename}", "true");
        }

        private static void CheckSpideringProgress(string spideringId)
        {
            int progress;
            while (true)
            {
                Thread.Sleep(10000);
                progress = int.Parse(((ApiResponseElement)ZapClient.spider.status(spideringId)).Value);
                if (progress >= 100)
                {
                    break;
                }
            }

            Thread.Sleep(5000);
        }

        private string StartSpidering()
        {
            response = ZapClient.spider.scan(TargetUrl, string.Empty, string.Empty, string.Empty, string.Empty);
            return ((ApiResponseElement)response).Value;
        }

        private string StartActiveScan()
        {
            response = ZapClient.ascan.scan(TargetUrl, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            return ((ApiResponseElement)response).Value;
        }
    }
}