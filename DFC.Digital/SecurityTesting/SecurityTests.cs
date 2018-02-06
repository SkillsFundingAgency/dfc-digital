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
        public readonly static string _zapApiKey = ConfigurationManager.AppSettings["apiKey"];
        public readonly static string _zapUrl = ConfigurationManager.AppSettings["host"];
        public readonly static string _zapPort = ConfigurationManager.AppSettings["port"];
        public readonly string _targetUrl = ConfigurationManager.AppSettings["rooturl"];
        public static string reportPath = ConfigurationManager.AppSettings["reportPath"];
        public static ClientApi _zapClient;
        public IApiResponse _response;

        public SecurityTests()
        {
            _zapClient = new ClientApi(_zapUrl, Convert.ToInt32(_zapPort), _zapApiKey);
            _zapClient.core.excludeFromProxy("^(?:(?!" + _targetUrl + ").)+$");
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
            _zapClient.Dispose();

            GenerateHTMLReport(reportFilename);
        }

        public string StartSpidering()
        {
            _response = _zapClient.spider.scan(_targetUrl, string.Empty, string.Empty, string.Empty, string.Empty);
            return ((ApiResponseElement)_response).Value;
        }

        public void CheckSpideringProgress(string spideringId)
        {
            int progress;
            while (true)
            {
                Thread.Sleep(10000);
                progress = int.Parse(((ApiResponseElement)_zapClient.spider.status(spideringId)).Value);
                if (progress >= 100)
                {
                    break;
                }
            }

            Thread.Sleep(5000);
        }

        public string StartActiveScan()
        {
            _response = _zapClient.ascan.scan(_targetUrl, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            return ((ApiResponseElement)_response).Value;
        }

        public void CheckActiveScanProgress(string activeScanId)
        {
            int progress;
            while (true)
            {
                Thread.Sleep(10000);
                progress = int.Parse(((ApiResponseElement)_zapClient.ascan.status(activeScanId)).Value);

                if (progress >= 100)
                {
                    break;
                }
            }

            Thread.Sleep(5000);
        }

        public static void GenerateXmlReport(string filename)
        {
            var fileName = $@"{reportPath}\{filename}.xml";
            File.WriteAllBytes(fileName, _zapClient.core.xmlreport());
        }

        public static void GenerateHTMLReport(string filename)
        {
            var fileName = $@"{reportPath}\{filename}.html";
            File.WriteAllBytes(fileName, _zapClient.core.htmlreport());
        }

        public static void GenerateMarkdownReport(string filename)
        {
            var fileName = $@"{reportPath}\{filename}.md";
            File.WriteAllBytes(fileName, _zapClient.core.mdreport());
        }

        public void SaveSession(string reportFilename)
        {
            var sessionReportPath = reportPath + "\\Sessions\\" + DateTime.Now.ToString("dd.MM.yy");
            _zapClient.core.saveSession($@"{sessionReportPath}\{reportFilename}", "true");
        }
    }
}