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
    public partial class ContactUsFeature : Xunit.IClassFixture<ContactUsFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
        private string _browser;
        
#line 1 "ContactUs.feature"
#line hidden
        
        public ContactUsFeature(ContactUsFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ContactUs", "As a Citizen, I want to be able to use the various methods to get in contact with" +
                    " the National Careers Service", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [Xunit.TheoryAttribute(DisplayName="Submit a \'Contact An Adviser\' form")]
        [Xunit.TraitAttribute("FeatureTitle", "ContactUs")]
        [Xunit.TraitAttribute("Description", "Submit a \'Contact An Adviser\' form")]
        [Xunit.InlineDataAttribute("Funding", "contact an Adviser Form", "Automated Test", "Automated Test", "automatedtestesfa@mailinator.com", "automatedtestesfa@mailinator.com", "20/11/2010", "CV3 5FE", "chrome", new string[] {
                "args:Funding,contact an Adviser Form,Automated Test,Automated Test,automatedteste" +
                    "sfa@mailinator.com,automatedtestesfa@mailinator.com,20/11/2010,CV3 5FE"})]
        public virtual void SubmitAContactAnAdviserForm(string contactReason, string adviserQuery, string firstname, string lastname, string emailAddress, string confirmationEmail, string dOB, string postCode, string browser, string[] exampleTags)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Submit a \'Contact An Adviser\' form", null, exampleTags);
#line 4
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
 testRunner.Given("I navigate to the Contact us select option page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.When("I select the \'contact-adviser\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 7
  testRunner.And("I press continue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
 testRunner.Then("I am redirected to the first \'adviser\' contact form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 9
 testRunner.When(string.Format("I complete the first form with {0} option and {1} query", contactReason, adviserQuery), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
  testRunner.And("I press continue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.Then("I am redirected to the second \'adviser\' contact form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 12
 testRunner.When(string.Format("I complete the details form with the details {0}, {1}, {2}, {3}, {4}, {5}", firstname, lastname, emailAddress, confirmationEmail, dOB, postCode), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 13
  testRunner.And("I press send", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
 testRunner.Then("I am redirected to the confirmation page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="Submit a \'Give Feedback\' form")]
        [Xunit.TraitAttribute("FeatureTitle", "ContactUs")]
        [Xunit.TraitAttribute("Description", "Submit a \'Give Feedback\' form")]
        [Xunit.InlineDataAttribute("Funding", "Give Feedback Forn", "Automated Test", "Automated Test", "automatedtestesfa@mailinator.com", "automatedtestesfa@mailinator.com", "yes", "chrome", new string[] {
                "args:Funding,Give Feedback Forn,Automated Test,Automated Test,automatedtestesfa@m" +
                    "ailinator.com,automatedtestesfa@mailinator.com,yes"})]
        [Xunit.InlineDataAttribute("Funding", "Give Feedback Forn", "Automated Test", "Automated Test", "automatedtestesfa@mailinator.com", "automatedtestesfa@mailinator.com", "no", "chrome", new string[] {
                "args:Funding,Give Feedback Forn,Automated Test,Automated Test,automatedtestesfa@m" +
                    "ailinator.com,automatedtestesfa@mailinator.com,no"})]
        public virtual void SubmitAGiveFeedbackForm(string contactReason, string adviserQuery, string firstname, string lastname, string emailAddress, string confirmationEmail, string contact, string browser, string[] exampleTags)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Submit a \'Give Feedback\' form", null, exampleTags);
#line 20
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 21
 testRunner.Given("I navigate to the Contact us select option page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 22
 testRunner.When("I select the \'give-feedback\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
  testRunner.And("I press continue on feecback form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
 testRunner.Then("I am redirected to the first \'feedback\' contact form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 25
 testRunner.When(string.Format("I complete the first feedback form with {0} option and {1} query", contactReason, adviserQuery), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 26
  testRunner.And("I press continue on feecback form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.Then("I am redirected to the second \'feedback\' contact form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 28
 testRunner.When(string.Format("I complete the give feedback details form with the details {0}, {1}, {2}, {3}, {4" +
                        "}", firstname, lastname, emailAddress, confirmationEmail, contact), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 29
  testRunner.And("I press send", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
 testRunner.Then("I am redirected to the confirmation page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="Submit a \'Technical Feedback\' form")]
        [Xunit.TraitAttribute("FeatureTitle", "ContactUs")]
        [Xunit.TraitAttribute("Description", "Submit a \'Technical Feedback\' form")]
        [Xunit.InlineDataAttribute("Give Feedback Forn", "Automated Test", "Automated Test", "automatedtestesfa@mailinator.com", "automatedtestesfa@mailinator.com", "yes", "chrome", new string[] {
                "args:Give Feedback Forn,Automated Test,Automated Test,automatedtestesfa@mailinato" +
                    "r.com,automatedtestesfa@mailinator.com,yes"})]
        [Xunit.InlineDataAttribute("Give Feedback Forn", "Automated Test", "Automated Test", "automatedtestesfa@mailinator.com", "automatedtestesfa@mailinator.com", "no", "chrome", new string[] {
                "args:Give Feedback Forn,Automated Test,Automated Test,automatedtestesfa@mailinato" +
                    "r.com,automatedtestesfa@mailinator.com,no"})]
        public virtual void SubmitATechnicalFeedbackForm(string adviserQuery, string firstname, string lastname, string emailAddress, string confirmationEmail, string contact, string browser, string[] exampleTags)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Submit a \'Technical Feedback\' form", null, exampleTags);
#line 37
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 38
 testRunner.Given("I navigate to the Contact us select option page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 39
 testRunner.When("I select the \'technical-issue\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 40
  testRunner.And("I press continue on tecnical form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
 testRunner.Then("I am redirected to the first \'technical\' contact form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 42
 testRunner.When(string.Format("I complete the first technical form with {0} query", adviserQuery), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 43
  testRunner.And("I press continue on tecnical form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.Then("I am redirected to the second \'technical\' contact form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 45
 testRunner.When(string.Format("I complete the give technical details form with the details {0}, {1}, {2}, {3}, {" +
                        "4}", firstname, lastname, emailAddress, confirmationEmail, contact), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 46
  testRunner.And("I press send", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
 testRunner.Then("I am redirected to the confirmation page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="Error message checks on Contact forms")]
        [Xunit.TraitAttribute("FeatureTitle", "ContactUs")]
        [Xunit.TraitAttribute("Description", "Error message checks on Contact forms")]
        [Xunit.InlineDataAttribute("Automated Test", "Automated Test", "automatedtestesfa@mailinator.com", "automatedtestesfa@mailinator.com", "20/11/2010", "CV3 5FE", "01/01/2000", "chrome", new string[] {
                "args:Automated Test,Automated Test,automatedtestesfa@mailinator.com,automatedtest" +
                    "esfa@mailinator.com,20/11/2010,CV3 5FE,01/01/2000"})]
        public virtual void ErrorMessageChecksOnContactForms(string firstname, string lastname, string emailAddress, string confirmationEmail, string dOB, string postCode, string dOB2, string browser, string[] exampleTags)
        {
            this._browser = browser;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Error message checks on Contact forms", null, exampleTags);
#line 54
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 55
 testRunner.Given("I navigate to the Contact us select option page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 56
 testRunner.When("I select the \'contact-adviser\' option", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 57
  testRunner.And("I press continue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
 testRunner.Then("I am redirected to the first \'adviser\' contact form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 59
 testRunner.When("I press continue with nothing selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 60
 testRunner.Then("an error message is displayed on the first form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 61
 testRunner.When("I complete the first form with Courses option and Error Validation Test query", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 62
  testRunner.And("I press continue", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
 testRunner.Then("I am redirected to the second \'adviser\' contact form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 64
 testRunner.When("I press continue with nothing selected", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 65
 testRunner.Then("an error message is displayed on the second form", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 66
 testRunner.When(string.Format("I complete the details form with the details {0}, {1}, {2}, {3}, {4}, {5}", firstname, lastname, emailAddress, confirmationEmail, dOB, postCode), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 67
  testRunner.And("I press send to generate an error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
 testRunner.Then("a date of birth error is displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 69
 testRunner.When(string.Format("I complete the details form with the details {0}, {1}, {2}, {3}, {4}, {5}", firstname, lastname, emailAddress, confirmationEmail, dOB2, postCode), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 70
  testRunner.And("I press send", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
 testRunner.Then("I am redirected to the confirmation page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                ContactUsFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ContactUsFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion

