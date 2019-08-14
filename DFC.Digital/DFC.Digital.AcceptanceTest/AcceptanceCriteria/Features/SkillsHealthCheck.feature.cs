﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.4.0.0
//      SpecFlow Generator Version:2.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace DFC.Digital.AcceptanceTest.AcceptanceCriteria.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class SkillsHealthCheckFeature : Xunit.IClassFixture<SkillsHealthCheckFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
        private string _browser;
        
#line 1 "SkillsHealthCheck.feature"
#line hidden
        
        public SkillsHealthCheckFeature(SkillsHealthCheckFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "SkillsHealthCheck", "As a Citizen, I want to be able to do skills health checks tests with the Nationa" +
                    "l Careers Service", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
var clonedScenarioInfo = new TechTalk.SpecFlow.ScenarioInfo(scenarioInfo.Title, scenarioInfo.Description, System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Union(scenarioInfo.Tags, new string[] { $"browser:{_browser}" })));
testRunner.OnScenarioInitialize(clonedScenarioInfo);
testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.TheoryAttribute(DisplayName="Complete a \'Skills Health Check\'")]
        [Xunit.TraitAttribute("FeatureTitle", "SkillsHealthCheck")]
        [Xunit.TraitAttribute("Description", "Complete a \'Skills Health Check\'")]
        [Xunit.InlineDataAttribute("Motivation", "42", "chrome", new string[] {
                "args:Motivation,42"})]
        public virtual void CompleteASkillsHealthCheck(string typeOfSkillsHealthCheck, string questionsToAnswer, string browser, string[] exampleTags)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Complete a \'Skills Health Check\'", null, exampleTags);
#line 4
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
 testRunner.Given("I navigate to the Your Assessments page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.When(string.Format("I select to start the {0} check", typeOfSkillsHealthCheck), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 7
 testRunner.Then(string.Format("I am redirected to the start page for a {0} check with {1} questions to answer", typeOfSkillsHealthCheck, questionsToAnswer), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 8
 testRunner.When(string.Format("I answer all {0} questions", questionsToAnswer), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 9
 testRunner.Then("on the last question I see the Return to Skills health check page button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 10
 testRunner.When("I click on the Return to Skills health check page button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 11
 testRunner.Then("I am taken back to the Your Assessments page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 12
 testRunner.And("I can down load my completed assessment as a PDF", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.And("I can down load my completed assessment as a Word document", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                SkillsHealthCheckFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                SkillsHealthCheckFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion

