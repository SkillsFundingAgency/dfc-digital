using Seleno.BrowserStack;
using TechTalk.SpecFlow.Generator.Interfaces;
using TechTalk.SpecFlow.Generator.Plugins;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Utils;

[assembly: GeneratorPlugin(typeof(GeneratorPlugin))]

namespace Seleno.BrowserStack
{
    public class GeneratorPlugin : IGeneratorPlugin
    {
        public void Initialize(GeneratorPluginEvents generatorPluginEvents, GeneratorPluginParameters generatorPluginParameters)
        {
            if (generatorPluginEvents != null)
            {
                generatorPluginEvents.CustomizeDependencies += GeneratorPluginEvents_CustomizeDependencies;
            }
        }

        private void GeneratorPluginEvents_CustomizeDependencies(object sender, CustomizeDependenciesEventArgs args)
        {
            var settings = args.ObjectContainer.Resolve<ProjectSettings>();
            var codeDomHelper = args.ObjectContainer.Resolve<CodeDomHelper>(settings.ProjectPlatformSettings.Language);
            var testGenerator = new SelenoXUnit2TestGeneratorProvider(codeDomHelper);
            args.ObjectContainer.RegisterInstanceAs<IUnitTestGeneratorProvider>(testGenerator);
        }
    }
}