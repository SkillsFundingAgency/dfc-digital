using DFC.Digital.AcceptanceTest.Infrastructure.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Specialized;
using System.Configuration;
using TechTalk.SpecFlow;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.WebServers;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Config
{
    public class BrowserStackSelenoHost : IDisposable
    {
        public SelenoHost Seleno { get; private set; }

        public RemoteWebDriver Browser { get; internal set; }

        public string BrowserName { get; internal set; }

        public ScenarioContext ScenarioContext { get; internal set; }

        internal string BrowserStackBrowserUri => ConfigurationManager.AppSettings.Get("browserStack.RemoteDriverUrl");

        internal RunProfile RunProfile => (RunProfile)Enum.Parse(typeof(RunProfile), ConfigurationManager.AppSettings.Get("RunProfile"), true);

        internal string RootUrl { get; set; } = ConfigurationManager.AppSettings["rooturl"];

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void Initalise()
        {
            switch (RunProfile)
            {
                case RunProfile.Remote:
                    DesiredCapabilities capability = PopulateCapabilities();
                    SetBrowserStackCredentials(capability);
                    Browser = new RemoteWebDriver(new Uri(this.BrowserStackBrowserUri), capability);
                    var selenoHost = new SelenoHost();
                    selenoHost.Run(config =>
                    {
                        config
                            .WithRemoteWebDriver(() => Browser)
                            .WithWebServer(new InternetWebServer(RootUrl));
                    });

                    Seleno = selenoHost;
                    break;

                case RunProfile.Owasp:
                    var zapHost = ConfigurationManager.AppSettings["zapHost"];
                    var zapPort = ConfigurationManager.AppSettings["zapPort"];
                    Proxy proxy = new Proxy
                    {
                        SslProxy = $"{zapHost}:{zapPort}",
                        HttpProxy = $"{zapHost}:{zapPort}",
                        FtpProxy = $"{zapHost}:{zapPort}",
                    };

                    Seleno = LocalBrowserHost.GetInstanceFor(BrowserName, RootUrl, proxy);
                    RootUrl = Seleno.Application.WebServer.BaseUrl;
                    break;

                case RunProfile.Local:
                default:
                    Seleno = LocalBrowserHost.GetInstanceFor(BrowserName, RootUrl);
                    RootUrl = Seleno.Application.WebServer.BaseUrl;
                    break;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (RunProfile == RunProfile.Remote)
                {
                    //Browser?.Quit();
                    //Browser = null;
                    Seleno?.Dispose();
                    Seleno = null;
                }
            }
        }

        private static void SetBrowserStackCredentials(DesiredCapabilities capability)
        {
            var username = ConfigurationManager.AppSettings.Get("browserStack.user");
            var accesskey = ConfigurationManager.AppSettings.Get("browserstack.key");
            capability.SetCapability("browserstack.user", username);
            capability.SetCapability("browserstack.key", accesskey);
        }

        private DesiredCapabilities PopulateCapabilities()
        {
            NameValueCollection capabilities = ConfigurationManager.GetSection("capabilities/" + RunProfile.ToString().ToLower()) as NameValueCollection;
            NameValueCollection browserSettings = ConfigurationManager.GetSection("environments/" + BrowserName.ToLower()) as NameValueCollection;
            DesiredCapabilities capability = new DesiredCapabilities();

            foreach (string key in capabilities.AllKeys)
            {
                capability.SetCapability(key, capabilities[key].FormatTokens(ScenarioContext));
            }

            foreach (string key in browserSettings.AllKeys)
            {
                capability.SetCapability(key, browserSettings[key]);
            }

            return capability;
        }
    }
}