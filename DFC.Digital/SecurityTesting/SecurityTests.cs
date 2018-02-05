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
        public readonly static string ZapApiKey = ConfigurationManager.AppSettings["apiKey"];
        public readonly static string ZapUrl = ConfigurationManager.AppSettings["host"];
        public readonly static string ZapPort = ConfigurationManager.AppSettings["port"];
        public readonly string TargetUrl = ConfigurationManager.AppSettings["rooturl"];
        public static string ReportPath = ConfigurationManager.AppSettings["reportPath"];
        public static ClientApi ZapClient;
        public IApiResponse Response;

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
            var reportFilename = $"{DateTime.Now.ToString("dd.MM.yy-hh.mm.ss")}-ZAP_Report";
            SaveSession(reportFilename);
            ZapClient.Dispose();

            GenerateHtmlReport(reportFilename);
        }

        public string StartSpidering()
        {
            Response = ZapClient.spider.scan(TargetUrl, string.Empty, string.Empty, string.Empty, string.Empty);
            return ((ApiResponseElement)Response).Value;
        }

        public void CheckSpideringProgress(string spideringId)
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

        public string StartActiveScan()
        {
            Response = ZapClient.ascan.scan(TargetUrl, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            return ((ApiResponseElement)Response).Value;
        }

        public void CheckActiveScanProgress(string activeScanId)
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

        public static void GenerateXmlReport(string filename)
        {
            var fileName = $@"{ReportPath}\{filename}.xml";
            File.WriteAllBytes(fileName, ZapClient.core.xmlreport());
        }

        public static void GenerateHtmlReport(string filename)
        {
            var fileName = $@"{ReportPath}\{filename}.html";
            File.WriteAllBytes(fileName, ZapClient.core.htmlreport());
        }

        public static void GenerateMarkdownReport(string filename)
        {
            var fileName = $@"{ReportPath}\{filename}.md";
            File.WriteAllBytes(fileName, ZapClient.core.mdreport());
        }

        public void SaveSession(string reportFilename)
        {
            var sessionReportPath = ReportPath + "\\Sessions\\" + DateTime.Now.ToString("dd.MM.yy");
            ZapClient.core.saveSession($@"{sessionReportPath}\{reportFilename}", "true");
        }
    }
}