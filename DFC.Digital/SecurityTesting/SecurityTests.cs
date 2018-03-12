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
            zapClient.Dispose();

            GenerateHtmlReport(reportFilename);
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

        private static void GenerateXmlReport(string filename)
        {
            var fileName = $@"{ReportPath}\{filename}.xml";
            File.WriteAllBytes(fileName, zapClient.core.xmlreport());
        }

        private static void GenerateHtmlReport(string filename)
        {
            var fileName = $@"{ReportPath}\{filename}.html";
            File.WriteAllBytes(fileName, zapClient.core.htmlreport());
        }

        private static void GenerateMarkdownReport(string filename)
        {
            var fileName = $@"{ReportPath}\{filename}.md";
            File.WriteAllBytes(fileName, zapClient.core.mdreport());
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