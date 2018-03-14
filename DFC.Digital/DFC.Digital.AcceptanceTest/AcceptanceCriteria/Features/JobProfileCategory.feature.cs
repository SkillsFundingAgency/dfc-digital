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
    public partial class JobProfileCategoryFeature : Xunit.IClassFixture<JobProfileCategoryFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
        private string _browser;
        
#line 1 "JobProfileCategory.feature"
#line hidden
        
        public JobProfileCategoryFeature(JobProfileCategoryFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "JobProfileCategory", "As a Citizen, I want to be able to view Job Categories So that I can see related " +
                    "jobs in the field", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [Xunit.TraitAttribute("FeatureTitle", "JobProfileCategory")]
        [Xunit.TraitAttribute("Description", "[DFC-1342 - A3] Job Category Page displays the correct breadcrumb and links to th" +
            "e Homepage")]
        [Xunit.TheoryAttribute(DisplayName="[DFC-1342 - A3] Job Category Page displays the correct breadcrumb and links to th" +
            "e Homepage")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        public virtual void DFC_1342_A3JobCategoryPageDisplaysTheCorrectBreadcrumbAndLinksToTheHomepage(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("[DFC-1342 - A3] Job Category Page displays the correct breadcrumb and links to th" +
                    "e Homepage", ((string[])(null)));
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
 testRunner.Given("I am viewing the \'Managerial\' category page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.Then("the correct \'Job category\' breadcrumb is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 8
 testRunner.When("I click the Find a Career breadcrumb on \'Job category\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 9
 testRunner.Then("I am redirected to the homepage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "JobProfileCategory")]
        [Xunit.TraitAttribute("Description", "Navigate between different categories")]
        [Xunit.TheoryAttribute(DisplayName="Navigate between different categories")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        [Xunit.TraitAttribute("Category", "EndToEnd")]
        public virtual void NavigateBetweenDifferentCategories(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Navigate between different categories", new string[] {
                        "EndToEnd"});
#line 13
this.ScenarioSetup(scenarioInfo);
#line 14
 testRunner.Given("I am viewing the \'Managerial\' category page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 15
 testRunner.Then("display a list of job profiles", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 16
 testRunner.And("display the other job categories section", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
 testRunner.And("the \'Managerial\' category should not be in the other job categories section", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
 testRunner.When("I click on \'Beauty and wellbeing\' under other categories", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 20
 testRunner.Then("display the correct category title", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 21
 testRunner.And("the \'Beauty and wellbeing\' category should not be in the other job categories sec" +
                    "tion", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
 testRunner.When("I click on \'Computing, technology and digital\' under other categories", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 24
 testRunner.Then("display the correct category title", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 25
 testRunner.And("the \'Computing, technology and digital\' category should not be in the other job c" +
                    "ategories section", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.When("I click on the no \'1\' job profile on the categories page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
 testRunner.Then("display the correct job profile page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "JobProfileCategory")]
        [Xunit.TraitAttribute("Description", "Job Profile Category End to End Test")]
        [Xunit.TheoryAttribute(DisplayName="Job Profile Category End to End Test")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        [Xunit.TraitAttribute("Category", "EndToEnd")]
        public virtual void JobProfileCategoryEndToEndTest(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Job Profile Category End to End Test", new string[] {
                        "EndToEnd"});
#line 31
this.ScenarioSetup(scenarioInfo);
#line 32
 testRunner.Given("that I am viewing the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 33
 testRunner.Then("display the \"Explore by job category\" text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 34
 testRunner.And("display a list of job profile categories", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.When("I click on job profile category no \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 37
 testRunner.Then("display the job profile category page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 39
 testRunner.When("I click on the no \'2\' job profile on the categories page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 40
 testRunner.Then("display the correct job profile page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "JobProfileCategory")]
        [Xunit.TraitAttribute("Description", "[DFC-1765 - Signpost banner on JC page is displayed and redirects to the BAU JP L" +
            "anding Page")]
        [Xunit.TheoryAttribute(DisplayName="[DFC-1765 - Signpost banner on JC page is displayed and redirects to the BAU JP L" +
            "anding Page")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        public virtual void DFC_1765_SignpostBannerOnJCPageIsDisplayedAndRedirectsToTheBAUJPLandingPage(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("[DFC-1765 - Signpost banner on JC page is displayed and redirects to the BAU JP L" +
                    "anding Page", ((string[])(null)));
#line 42
this.ScenarioSetup(scenarioInfo);
#line 43
 testRunner.Given("I am viewing the \'Managerial\' category page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 44
 testRunner.Then("display a list of job profiles", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 45
 testRunner.And("the \'category\' page signpost banner is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
 testRunner.When("I click on the \'category\' page banner link", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 48
 testRunner.Then("I am redirected to the BAU Job Profile landing page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.2.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                JobProfileCategoryFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                JobProfileCategoryFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
