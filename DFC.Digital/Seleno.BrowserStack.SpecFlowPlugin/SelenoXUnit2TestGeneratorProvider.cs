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
        private const string FACTATTRIBUTE = "Xunit.FactAttribute";

        //internal const string THEORY_ATTRIBUTE = "Xunit.Extensions.TheoryAttribute";

        //private const string INLINEDATA_ATTRIBUTE = "Xunit.Extensions.InlineDataAttribute";
        private const string SKIPREASON = "Ignored";

        private const string TRAITATTRIBUTE = "Xunit.TraitAttribute";
        private const string IUSEFIXTUREINTERFACE = "Xunit.IUseFixture";
        private const string CATEGORYPROPERTYNAME = "Category";

        private const string THEORYATTRIBUTE = "Xunit.TheoryAttribute";
        private const string INLINEDATAATTRIBUTE = "Xunit.InlineDataAttribute";
        private const string ICLASSFIXTUREINTERFACE = "Xunit.IClassFixture";
        private const string OUTPUTINTERFACE = "Xunit.Abstractions.ITestOutputHelper";
        private const string OUTPUTINTERFACEPARAMETERNAME = "testOutputHelper";
        private const string OUTPUTINTERFACEFIELDNAME = "_testOutputHelper";
        private const string FIXTUREDATAPARAMETERNAME = "fixtureData";
        private const string IBROWSER = "Seleno.BrowserStack.Config.IBrowser";

        public SelenoXUnit2TestGeneratorProvider(CodeDomHelper codeDomHelper) : base(codeDomHelper)
        {
            Browsers = DfcConfigurationManager.Get<string>("browsers").Split(',').Select(c => c.Trim()).ToList();
        }

        public List<string> Browsers { get; } = new List<string> {"chrome", "firefox"};

        public new void SetTestMethod(TestClassGenerationContext generationContext, CodeMemberMethod testMethod,
            string friendlyTestName)
        {
            base.SetTestMethod(generationContext, testMethod, friendlyTestName);

            var factAttr = testMethod.CustomAttributes.OfType<CodeAttributeDeclaration>()
                .FirstOrDefault(codeAttributeDeclaration => codeAttributeDeclaration.Name == FACTATTRIBUTE);

            if (factAttr != null)
                testMethod.CustomAttributes.Remove(factAttr);

            CodeDomHelper.AddAttribute(testMethod, THEORYATTRIBUTE,
                new CodeAttributeArgument("DisplayName", new CodePrimitiveExpression(friendlyTestName)));
            SetBrowsers(testMethod);

            testMethod.Parameters.Add(new CodeParameterDeclarationExpression(
                new CodeTypeReference(new CodeTypeParameter(typeof(string).ToString())), "browser"));
            testMethod.Statements.Add(AssignBrowser());
        }

        public override void SetRowTest(TestClassGenerationContext generationContext, CodeMemberMethod testMethod,
            string scenarioTitle)
        {
            base.SetRowTest(generationContext, testMethod, scenarioTitle);
            var parameterCount = testMethod.Parameters.Count;
            testMethod.Parameters.Insert(parameterCount - 1,
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference(new CodeTypeParameter(typeof(string).ToString())), "browser"));
            testMethod.Statements.Insert(0, AssignBrowser());
        }

        public override void SetRow(TestClassGenerationContext generationContext, CodeMemberMethod testMethod,
            IEnumerable<string> arguments, IEnumerable<string> tags, bool isIgnored)
        {
            foreach (var item in Browsers)
                base.SetRow(generationContext, testMethod, arguments.Concat(new[] {item}),
                    tags.Concat(new[] {$"args:{string.Join(",", arguments)}"}), isIgnored);
        }

        public override void FinalizeTestClass(TestClassGenerationContext generationContext)
        {
            base.FinalizeTestClass(generationContext);

            generationContext.ScenarioInitializeMethod.Statements.Clear();
            generationContext.ScenarioInitializeMethod.Statements.Add(new CodeSnippetStatement(
                "var clonedScenarioInfo = new TechTalk.SpecFlow.ScenarioInfo(scenarioInfo.Title, System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Union(scenarioInfo.Tags, new string[] { $\"browser:{_browser}\" })));"));
            generationContext.ScenarioInitializeMethod.Statements.Add(
                new CodeSnippetStatement("testRunner.OnScenarioStart(clonedScenarioInfo);"));
            generationContext.ScenarioInitializeMethod.Statements.Add(new CodeSnippetStatement(
                "testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);"));
        }

        protected override void SetTestConstructor(TestClassGenerationContext generationContext,
            CodeConstructor ctorMethod)
        {
            base.SetTestConstructor(generationContext, ctorMethod);
            generationContext.TestClass.Members.Add(new CodeMemberField(typeof(string), "_browser"));
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
                CodeDomHelper.AddAttribute(testMethod, INLINEDATAATTRIBUTE, item);
        }
    }
}