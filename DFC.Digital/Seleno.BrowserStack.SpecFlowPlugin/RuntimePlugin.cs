//using Seleno.BrowserStack;
//using TechTalk.SpecFlow.Plugins;
//using TechTalk.SpecFlow.UnitTestProvider;

//[assembly: RuntimePlugin(typeof(RuntimePlugin))]
//namespace Seleno.BrowserStack
//{
//    public class RuntimePlugin : IRuntimePlugin
//    {
//        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters)
//        {
//            runtimePluginEvents.RegisterGlobalDependencies += (sender, args) =>
//                args.ObjectContainer.RegisterInstanceAs<IUnitTestRuntimeProvider>(new XUnit2RuntimeProvider(), "SeleniumXUnit");
//        }
//    }
//}