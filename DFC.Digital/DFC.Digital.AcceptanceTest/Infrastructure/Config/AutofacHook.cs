using Autofac;
using BoDi;
using DFC.Digital.Web.Core.Config;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.Infrastructure.Config
{
    [Binding]
    public class AutofacHook
    {
        private IObjectContainer scenarioContainer;

        public AutofacHook(IObjectContainer scenarioContainer)
        {
            this.scenarioContainer = scenarioContainer;
        }

        [BeforeScenario(Order = 0)]
        public void SetupAutofac()
        {
            //Nothing specific to test that require registration
            var autofacContainer = WebCoreAutofacConfig.BuildContainer(null);
            scenarioContainer.RegisterInstanceAs<IContainer>(autofacContainer);
        }
    }
}