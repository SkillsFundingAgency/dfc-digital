using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Utils;

namespace Seleno.BrowserStack
{
    public class SelenoXUnit2TestGeneratorProvider : XUnit2TestGeneratorProvider, IUnitTestGeneratorProvider
    {
        private const string FactAttribute = "Xunit.FactAttribute";
        private const string TheoryAttribute = "Xunit.TheoryAttribute";
        private const string InlineDataAttribute = "Xunit.InlineDataAttribute";

        public SelenoXUnit2TestGeneratorProvider(CodeDomHelper codeDomHelper) : base(codeDomHelper)
        {
            Browsers = DfcConfigurationManager.Get<string>("browsers").Split(',').Select(c => c.Trim()).ToList();
        }

        private List<string> Browsers { get; }

        public new void SetTestMethod(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string friendlyTestName)
        {
            base.SetTestMethod(generationContext, testMethod, friendlyTestName);

            var factAttr = testMethod.CustomAttributes.OfType<CodeAttributeDeclaration>()
                .FirstOrDefault(codeAttributeDeclaration => codeAttributeDeclaration.Name == FactAttribute);

            if (factAttr != null)
            {
                testMethod.CustomAttributes.Remove(factAttr);
            }

            CodeDomHelper.AddAttribute(testMethod, TheoryAttribute, new CodeAttributeArgument("DisplayName", new CodePrimitiveExpression(friendlyTestName)));
            SetBrowsers(testMethod);

            testMethod.Parameters.Add(new CodeParameterDeclarationExpression(
                new CodeTypeReference(new CodeTypeParameter(typeof(string).ToString())), "browser"));
            testMethod.Statements.Add(AssignBrowser());
        }

        public override void SetRowTest(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle)
        {
            base.SetRowTest(generationContext, testMethod, scenarioTitle);
            var parameterCount = testMethod.Parameters.Count;
            testMethod.Parameters.Insert(parameterCount - 1, new CodeParameterDeclarationExpression(new CodeTypeReference(new CodeTypeParameter(typeof(string).ToString())), "browser"));
            testMethod.Statements.Insert(0, AssignBrowser());
        }

        public override void SetRow(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, IEnumerable<string> arguments, IEnumerable<string> tags, bool isIgnored)
        {
            foreach (var item in Browsers)
            {
                base.SetRow(generationContext, testMethod, arguments.Concat(new[] { item }), tags.Concat(new[] { $"args:{string.Join(",", arguments)}" }), isIgnored);
            }
        }

        public override void FinalizeTestClass(TestClassGenerationContext generationContext)
        {
            base.FinalizeTestClass(generationContext);

            generationContext?.ScenarioInitializeMethod.Statements.Clear();
            generationContext?.ScenarioInitializeMethod.Statements.Add(new CodeSnippetStatement(
                "var clonedScenarioInfo = new TechTalk.SpecFlow.ScenarioInfo(scenarioInfo.Title, System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Union(scenarioInfo.Tags, new string[] { $\"browser:{_browser}\" })));"));
            generationContext?.ScenarioInitializeMethod.Statements.Add(
                new CodeSnippetStatement("testRunner.OnScenarioStart(clonedScenarioInfo);"));
            generationContext?.ScenarioInitializeMethod.Statements.Add(new CodeSnippetStatement(
                "testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);"));
        }

        protected override void SetTestConstructor(
            TestClassGenerationContext generationContext,
            CodeConstructor ctorMethod)
        {
            base.SetTestConstructor(generationContext, ctorMethod);
            generationContext?.TestClass.Members.Add(new CodeMemberField(typeof(string), "_browser"));
        }

        private static CodeStatement AssignBrowser()
        {
            return new CodeAssignStatement(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_browser"),
                new CodeVariableReferenceExpression("browser"));
        }

        private void SetBrowsers(CodeMemberMethod testMethod)
        {
            foreach (var item in Browsers)
            {
                CodeDomHelper.AddAttribute(testMethod, InlineDataAttribute, item);
            }
        }
    }
}