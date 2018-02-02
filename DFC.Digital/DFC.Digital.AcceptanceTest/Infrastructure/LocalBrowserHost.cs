using OpenQA.Selenium;
using System.Collections.Concurrent;
using System.Configuration;
using System.Threading;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.WebServers;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public static class LocalBrowserHost
    {
        public const string LocalInstanceKey = "localInstance";

        private static ConcurrentDictionary<string, bool> hasInitalised = new ConcurrentDictionary<string, bool>();
        private static ConcurrentDictionary<string, SelenoHost> localSelenoInstances = new ConcurrentDictionary<string, SelenoHost>();

        public static string ScreenShotFolder => ConfigurationManager.AppSettings["screenshotfolder"];

        public static SelenoHost GetInstanceFor(string key, string remoteRootUrl, Proxy proxy = null)
        {
            int counter = 0;
            if (!localSelenoInstances.ContainsKey(key))
            {
                var instance = new SelenoHost();
                if (InitiateInstance(instance, key, remoteRootUrl, proxy))
                {
                    localSelenoInstances.GetOrAdd(key, instance);
                }
                else
                {
                    if (counter < 3)
                    {
                        counter++;
                        Thread.Sleep(30000);
                        GetInstanceFor(key, remoteRootUrl, proxy);
                    }
                }
            }

            return localSelenoInstances[key];
        }

        public static void CleanUp(string key)
        {
            if (localSelenoInstances.ContainsKey(key))
            {
                SelenoHost selenoHost;
                localSelenoInstances.TryRemove(key, out selenoHost);
                selenoHost?.Dispose();
                selenoHost = null;
            }
        }

        private static bool InitiateInstance(SelenoHost instance, string browser, string remoteRootUrl, Proxy proxy)
        {
            //bool browserInitalised = false;
            //hasInitalised.TryGetValue(browser, out browserInitalised);

            //if (browserInitalised)
            //{
            //    return false;
            //}
            //else
            //{
            //    hasInitalised.GetOrAdd(browser, true);
            if (string.IsNullOrEmpty(remoteRootUrl))
            {
                instance.Run("DFC.Digital.Web.Sitefinity", 60876, config =>
                {
                    config
                        .WithRemoteWebDriver(() =>
                        {
                            switch (browser)
                            {
                                case "firefox":
                                    return BrowserFactory.FireFox();

                                case "chrome":
                                default:
                                    if (proxy != null)
                                    {
                                        return BrowserFactory.Chrome(new OpenQA.Selenium.Chrome.ChromeOptions
                                        {
                                            Proxy = proxy
                                        });
                                    }
                                    else
                                    {
                                        return BrowserFactory.Chrome();
                                    }
                            }
                        })
                        .UsingCamera(ScreenShotFolder);
                });
            }
            else
            {
                instance.Run(config =>
                {
                    config
                        .WithRemoteWebDriver(() =>
                        {
                            switch (browser)
                            {
                                case "firefox":
                                    return BrowserFactory.FireFox();

                                case "chrome":
                                default:
                                    if (proxy != null)
                                    {
                                        return BrowserFactory.Chrome(new OpenQA.Selenium.Chrome.ChromeOptions
                                        {
                                            Proxy = proxy
                                        });
                                    }
                                    else
                                    {
                                        return BrowserFactory.Chrome();
                                    }
                            }
                        })
                        .WithWebServer(new InternetWebServer(remoteRootUrl))
                        .UsingCamera(ScreenShotFolder)
                        ;
                });
            }

            //}
            return true;
        }
    }
}