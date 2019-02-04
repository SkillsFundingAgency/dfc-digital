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
    public partial class JobProfileSearchFeature : Xunit.IClassFixture<JobProfileSearchFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
        private string _browser;
        
#line 1 "JobProfileSearch.feature"
#line hidden
        
        public JobProfileSearchFeature(JobProfileSearchFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "JobProfileSearch", "As a Citizen, I want to be able to search for job profiles So that I can view spe" +
                    "cific results", ProgrammingLanguage.CSharp, ((string[])(null)));
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
var clonedScenarioInfo = new TechTalk.SpecFlow.ScenarioInfo(scenarioInfo.Title, System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Union(scenarioInfo.Tags, new string[] { $"browser:{_browser}" })));
testRunner.OnScenarioStart(clonedScenarioInfo);
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
        
        [Xunit.TheoryAttribute(DisplayName="[DFC-165 - A1] When the search result item title is clicked then the user is dire" +
            "cted to the specific job profile page")]
        [Xunit.TraitAttribute("FeatureTitle", "JobProfileSearch")]
        [Xunit.TraitAttribute("Description", "[DFC-165 - A1] When the search result item title is clicked then the user is dire" +
            "cted to the specific job profile page")]
        [Xunit.InlineDataAttribute("Dental Nurse", "#{browsers}#", new string[] {
                "args:Dental Nurse"})]
        [Xunit.InlineDataAttribute("Adult nurse", "#{browsers}#", new string[] {
                "args:Adult nurse"})]
        public virtual void DFC_165_A1WhenTheSearchResultItemTitleIsClickedThenTheUserIsDirectedToTheSpecificJobProfilePage(string searchTerm, string browser, string[] exampleTags)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("[DFC-165 - A1] When the search result item title is clicked then the user is dire" +
                    "cted to the specific job profile page", null, exampleTags);
#line 5
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
 testRunner.Given("that I am viewing the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
 testRunner.When(string.Format("I search using \'{0}\'", searchTerm), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 8
 testRunner.And("I click on result title no \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
 testRunner.Then("I am redirected to the correct job profile page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "JobProfileSearch")]
        [Xunit.TraitAttribute("Description", "[DFC-340 - A1] Performing a search with text within escaped characters that will " +
            "return a result")]
        [Xunit.TheoryAttribute(DisplayName="[DFC-340 - A1] Performing a search with text within escaped characters that will " +
            "return a result")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        public virtual void DFC_340_A1PerformingASearchWithTextWithinEscapedCharactersThatWillReturnAResult(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("[DFC-340 - A1] Performing a search with text within escaped characters that will " +
                    "return a result", null, ((string[])(null)));
#line 15
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 16
 testRunner.Given("that I am viewing the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 17
 testRunner.When("I search using \'<Police Officer>\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 18
 testRunner.Then("the first result is \'shown\' on the page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "JobProfileSearch")]
        [Xunit.TraitAttribute("Description", "Perform a search that will return 0 results")]
        [Xunit.TheoryAttribute(DisplayName="Perform a search that will return 0 results")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        public virtual void PerformASearchThatWillReturn0Results(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Perform a search that will return 0 results", null, ((string[])(null)));
#line 20
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 21
 testRunner.Given("that I am viewing the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 22
 testRunner.When("I search using \'return0results\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
 testRunner.Then("the no results message is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "JobProfileSearch")]
        [Xunit.TraitAttribute("Description", "[DFC-1342 - A1] Search Page displays the correct breadcrumb and links to the Home" +
            "page")]
        [Xunit.TheoryAttribute(DisplayName="[DFC-1342 - A1] Search Page displays the correct breadcrumb and links to the Home" +
            "page")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        public virtual void DFC_1342_A1SearchPageDisplaysTheCorrectBreadcrumbAndLinksToTheHomepage(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("[DFC-1342 - A1] Search Page displays the correct breadcrumb and links to the Home" +
                    "page", null, ((string[])(null)));
#line 25
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 26
 testRunner.Given("that I am viewing the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 27
 testRunner.When("I search using \'analyst\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
 testRunner.Then("the first result is \'shown\' on the page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 29
 testRunner.And("the correct \'Search results\' breadcrumb is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
 testRunner.When("I click the Explore career breadcrumb on \'Search results\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
 testRunner.Then("I am redirected to the homepage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "JobProfileSearch")]
        [Xunit.TraitAttribute("Description", "Search End To End Test")]
        [Xunit.TheoryAttribute(DisplayName="Search End To End Test")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        [Xunit.TraitAttribute("Category", "EndToEnd")]
        public virtual void SearchEndToEndTest(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search End To End Test", null, new string[] {
                        "EndToEnd"});
#line 35
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 36
 testRunner.Given("that I am viewing the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 37
 testRunner.When("I search using \'Nurse\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
 testRunner.Then("the first result is \'shown\' on the page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 40
 testRunner.When("I click on result title no \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 41
 testRunner.Then("I am redirected to the correct job profile page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 42
 testRunner.And("the correct sections should be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.When("I click the Explore careers link", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 45
 testRunner.And("I search using \'manager\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
 testRunner.Then("the first result is \'shown\' on the page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 48
 testRunner.When("I click on result title no \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 49
 testRunner.Then("I am redirected to the correct job profile page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 50
 testRunner.And("the correct sections should be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "JobProfileSearch")]
        [Xunit.TraitAttribute("Description", "[DFC-1496 - A1] Starting a search displays Auto Suggest, and when selected, popul" +
            "ates the searchbox")]
        [Xunit.TheoryAttribute(DisplayName="[DFC-1496 - A1] Starting a search displays Auto Suggest, and when selected, popul" +
            "ates the searchbox")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        public virtual void DFC_1496_A1StartingASearchDisplaysAutoSuggestAndWhenSelectedPopulatesTheSearchbox(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("[DFC-1496 - A1] Starting a search displays Auto Suggest, and when selected, popul" +
                    "ates the searchbox", null, ((string[])(null)));
#line 52
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 53
 testRunner.Given("that I am viewing the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 54
 testRunner.When("I enter the term \'Te\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 55
 testRunner.Then("the suggested results should appear", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 56
 testRunner.When("I select suggested result no \'1\' and search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 57
 testRunner.Then("the first result is \'shown\' on the page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 58
 testRunner.And("the search box should populate with the selected result", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "JobProfileSearch")]
        [Xunit.TraitAttribute("Description", "[DFC-1494 - A1] Performing incorrectly spelled search suggests a Did You Mean opt" +
            "ion")]
        [Xunit.TheoryAttribute(DisplayName="[DFC-1494 - A1] Performing incorrectly spelled search suggests a Did You Mean opt" +
            "ion")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        public virtual void DFC_1494_A1PerformingIncorrectlySpelledSearchSuggestsADidYouMeanOption(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("[DFC-1494 - A1] Performing incorrectly spelled search suggests a Did You Mean opt" +
                    "ion", null, ((string[])(null)));
#line 60
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 61
 testRunner.Given("that I am viewing the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 62
 testRunner.When("I search using \'nusre\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 63
 testRunner.Then("search should display the Did You Mean text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 64
 testRunner.When("I click the suggested text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 65
 testRunner.Then("I am redirected to the correct search results page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 66
 testRunner.And("search should not display the Did You Mean text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TraitAttribute("FeatureTitle", "JobProfileSearch")]
        [Xunit.TraitAttribute("Description", "[DFC-1495 - A1] Performing a search displays Job categories and clicking one take" +
            "s you to the categories page")]
        [Xunit.TheoryAttribute(DisplayName="[DFC-1495 - A1] Performing a search displays Job categories and clicking one take" +
            "s you to the categories page")]
        [Xunit.InlineDataAttribute("#{browsers}#")]
        public virtual void DFC_1495_A1PerformingASearchDisplaysJobCategoriesAndClickingOneTakesYouToTheCategoriesPage(string browser)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("[DFC-1495 - A1] Performing a search displays Job categories and clicking one take" +
                    "s you to the categories page", null, ((string[])(null)));
#line 68
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 69
 testRunner.Given("that I am viewing the Home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 70
 testRunner.When("I search using \'manager\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 71
 testRunner.Then("the first result is \'shown\' on the page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 72
 testRunner.And("the results display the Found In category text", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
 testRunner.When("I click the first category link on result no \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 74
 testRunner.Then("I am redirected to the correct job category page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                JobProfileSearchFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                JobProfileSearchFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
