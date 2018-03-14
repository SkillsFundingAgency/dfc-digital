﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.2.0.0
//      SpecFlow Generator Version:2.2.0.0
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
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.2.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class HomePageFeature : Xunit.IClassFixture<HomePageFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
        private string _browser;
        
#line 1 "HomePage.feature"
#line hidden
        
        public HomePageFeature(HomePageFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "HomePage", "As a Citizen I want to be able to view the Homepage So that I can be signposted t" +
                    "o search", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
var clonedScenarioInfo = new TechTalk.SpecFlow.ScenarioInfo(scenarioInfo.Title, System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Union(scenarioInfo.Tags, new string[] { $"browser:{_browser}" })));
testRunner.OnScenarioStart(clonedScenarioInfo);
testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "HomePage")]
        [Xunit.TraitAttribute("Description", "[DFC-274 - A2] Job profile category page")]
        [Xunit.TheoryAttribute(DisplayName="[DFC-274 - A2] Job profile category page")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        public virtual void DFC_274_A2JobProfileCategoryPage(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("[DFC-274 - A2] Job profile category page", ((string[])(null)));
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
 testRunner.Given("that I am viewing the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
 testRunner.When("I click on job profile category no \'5\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 8
 testRunner.Then("display the job profile category page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "HomePage")]
        [Xunit.TraitAttribute("Description", "Check the cookie page loads")]
        [Xunit.TheoryAttribute(DisplayName="Check the cookie page loads")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        [Xunit.TraitAttribute("Category", "EndToEnd")]
        public virtual void CheckTheCookiePageLoads(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Check the cookie page loads", new string[] {
                        "EndToEnd"});
#line 12
this.ScenarioSetup(scenarioInfo);
#line 13
 testRunner.Given("that I am viewing the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 14
 testRunner.When("I click on the Cookies link", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 15
 testRunner.Then("I am redirected to the cookies page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "HomePage")]
        [Xunit.TraitAttribute("Description", "404 Page is displayed when navigating to a profile that doesn\'t exist")]
        [Xunit.TheoryAttribute(DisplayName="404 Page is displayed when navigating to a profile that doesn\'t exist")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        [Xunit.TraitAttribute("Category", "EndToEnd")]
        public virtual void _404PageIsDisplayedWhenNavigatingToAProfileThatDoesntExist(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("404 Page is displayed when navigating to a profile that doesn\'t exist", new string[] {
                        "EndToEnd"});
#line 18
this.ScenarioSetup(scenarioInfo);
#line 19
 testRunner.Given("that I am viewing the \'profile-does-not-exist\' job profile page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 20
 testRunner.Then("I am redirected to the 404 page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 22
 testRunner.When("I click on the Back To Homepage link", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
 testRunner.Then("I am redirected to the homepage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 24
 testRunner.And("display the \"Explore by job category\" text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
 testRunner.And("display a list of job profile categories", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.2.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                HomePageFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                HomePageFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
