using BoDi;
using System.Linq;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Config
{
    [Binding]
    public class BrowserStackHook
    {
        private readonly ScenarioContext scenarioContext;
        private IObjectContainer scenarioContainer;

        public BrowserStackHook(IObjectContainer scenarioContainer, ScenarioContext scenarioContext)
        {
            this.scenarioContainer = scenarioContainer;
            this.scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void SetupBrowserStack()
        {
            //Nothing specific to test that require registration
            var browser = scenarioContext.ScenarioInfo.Tags.FirstOrDefault(t => t.StartsWith("browser:", System.StringComparison.Ordinal))?.Replace("browser:", string.Empty);
            var browserHost = new BrowserStackSelenoHost
            {
                BrowserName = browser ?? "chrome",
                ScenarioContext = scenarioContext,
            };

            browserHost.Initalise();
            scenarioContainer.RegisterInstanceAs(browserHost);
        }

        [AfterScenario]
        public void TeardownBrowserStack()
        {
            var instance = scenarioContainer.Resolve<BrowserStackSelenoHost>();
            instance?.Dispose();
        }
    }
}