using DFC.Digital.AcceptanceTest.Infrastructure;
using DFC.Digital.AcceptanceTest.Infrastructure.Pages;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.AcceptanceCriteria.Steps
{
    [Binding]
    public class SkillsHealthCheckSteps : BaseStep
    {
        public SkillsHealthCheckSteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
        {
        }

        [Given(@"I navigate to the Skills Health Check Home page")]
        public void GivenINavigateToTheSkillsHealthCheckHomePage()
        {
            NavigateToYourAssesmentsPage<SkillsHealthCheckHomePage>();
        }

        [Given(@"I click on the SignIn button")]
        [When(@"I click on the SignIn button")]
        public void IClickOnTheSignInButton()
        {
            GetNavigatedPage<SkillsHealthCheckHomePage>().ClickSignIn<SignInPage>().SaveTo(ScenarioContext);
        }

        [Then(@"I can see my completed (.*) check")]
        public void ThenICanSeeMyCompletedChecks(string typeOfSkillsHealthCheck)
        {
            GetNavigatedPage<YourAssesmentsPage>().PageTitle.Should().Contain("Skills Health Check");
            GetNavigatedPage<YourAssesmentsPage>().FirstCompletedRowName.Should().Contain(typeOfSkillsHealthCheck);
        }

        [Then(@"I am directed to the SignIn Page")]
        public void ThenIAmDirectedToTheSignInPage()
        {
            GetNavigatedPage<SignInPage>().PageTitle.Should().Contain("Sign in");
        }

        [Then(@"I am directed to the Your Assessments List page")]
        public void ThenIAmDirectedToTheYourAssessmentsListPage()
        {
            GetNavigatedPage<YourAssesmentsPage>().PageTitle.Should().Contain("Skills Health Check");
        }

        [Then(@"I am redirected to the start page for a (.*) check with (.*) questions to answer")]
        public void ThenIAmRedirectedToTheStartPageForASkillsHealthCheckCheckWithQuestionsToAnswer(string checkTitle, int numberOfQuestions)
        {
            GetNavigatedPage<SkillsHealthCheckPage>().PageTitle.ToUpperInvariant().Should().Contain(checkTitle.ToUpperInvariant());
            GetNavigatedPage<SkillsHealthCheckPage>().PageTitle.Should().Contain($"Question 1 out of {numberOfQuestions}");
        }

        [Then(@"on the last question I see the Return to Skills health check page button")]
        public void ThenOnTheLastQuestionISeeTheReturnToSkillsHealthCheckPageButton()
        {
            GetNavigatedPage<SkillsHealthCheckPage>().ActionButtonText.Should().Contain("Return to Skills health check page");
        }

        [Then(@"I am taken back to the Your Assessments page")]
        public void ThenIAmTakenBackToTheYourAssessmentsPage()
        {
            GetNavigatedPage<YourAssesmentsPage>().PageTitle.Should().Contain("Skills Health Check");
        }

        [Then(@"I can down load my completed assessment as a PDF")]
        public void ThenICanDownLoadMyCompletedAssessmentAsAPDF()
        {
            GetNavigatedPage<YourAssesmentsPage>().DownLoadSkillsHeathCheckReport("Pdf");
        }

        [Then(@"I can down load my completed assessment as a Word document")]
        public void ThenICanDownLoadMyCompletedAssessmentAsAWordDocument()
        {
            GetNavigatedPage<YourAssesmentsPage>().DownLoadSkillsHeathCheckReport("Word");
        }

        [Then(@"I am directed back to the Skills Health Check Home page")]
        public void ThenIAmDirectedBackToTheSkillsHealthCheckHomePage()
        {
            GetNavigatedPage<SkillsHealthCheckHomePage>().PageTitle.Should().Contain("Skills Health Check");
        }

        [Then(@"I am now signed in")]
        public void ThenIAmNowSignedIn()
        {
            GetNavigatedPage<SkillsHealthCheckHomePage>().SignOutLinkText.Should().Be("Sign out");
        }

        [When(@"I select to start the (.*) check")]
        public void WhenISelectToStartANewSkilsHealthCheckCheck(string typeOfSkillsHealthCheck)
        {
            GetNavigatedPage<YourAssesmentsPage>().StartASkillsHealthCheck<SkillsHealthCheckPage>(typeOfSkillsHealthCheck).SaveTo(ScenarioContext);
        }

        [When(@"I answer all (.*) questions")]
        public void WhenIAnswerAllQuestions(int numberOfQuestions)
        {
            var skillsHealthCheckPage = GetNavigatedPage<SkillsHealthCheckPage>();

            //Answer questions untill we get to the last one
            for (int ii = 1; ii < numberOfQuestions; ii++)
            {
                skillsHealthCheckPage.AnswerQuestion(1);
            }
        }

        [When(@"I click on the Return to Skills health check page button")]
        public void WhenIClickOnTheReturnToSkillsHealthCheckPageButton()
        {
            GetNavigatedPage<SkillsHealthCheckPage>().AnswerQuestion(1);
        }

        [When(@"I sign in with an existing user name and password")]
        public void WhenISignInWithAnExistingUserNameAndPassword()
        {
            GetNavigatedPage<SignInPage>().Login<SkillsHealthCheckHomePage>(ConfigurationManager.AppSettings["existingUserName"], ConfigurationManager.AppSettings["existingUserPassword"]).SaveTo(ScenarioContext);
        }

        [When(@"I click on Start a new Skills Health Check button")]
        public void WhenIClickOnStartANewSkillsHealthCheckButton()
        {
            GetNavigatedPage<SkillsHealthCheckHomePage>().ClickStartANewSkillsHealthCheck<YourAssesmentsPage>().SaveTo(ScenarioContext);
        }

        [When(@"I click on the Your Account link")]
        public void WhenIClickOnTheYourAccountLink()
        {
            GetNavigatedPage<YourAssesmentsPage>().ClickYourAccountLink<YourAccountHomePage>().SaveTo(ScenarioContext);
        }

        [Then(@"I am redirected to the Your Account Home page")]
        public void ThenIAmRedirectedToTheYourAccountHomePage()
        {
            GetNavigatedPage<YourAccountHomePage>().PageTitle.Should().Contain("Your account");
        }

        [Then(@"I can see the saved Skills health check")]
        public void ThenICanSeeTheSavedSkillsHealthCheck()
        {
            GetNavigatedPage<YourAccountHomePage>().SkillsHealthChecksFirstRowFirstColumn.Should().Contain("Started");
        }

        [When(@"I delete my Skills health check")]
        public void WhenIDeleteMySkillsHealthCheck()
        {
            GetNavigatedPage<YourAccountHomePage>().ClickDeleteLink<YourAccountHomePage>().SaveTo(ScenarioContext);
        }

        [Then(@"I have no saved Skills health checks")]
        public void ThenIHaveNoSavedSkillsHealthChecks()
        {
            GetNavigatedPage<YourAccountHomePage>().SkillsHealthChecksFirstRowFirstColumn.Should().Be("No skills health checks found.");
        }

        [Then(@"I am shown the section I have already started a Skills Health Check")]
        public void ThenIAmShownTheSectionIHaveAlreadyStartedASkillsHealthCheck()
        {
            GetNavigatedPage<SkillsHealthCheckHomePage>().SectionHeading.Should().Be("You've already started a Skills Health Check");
        }

        [When(@"I click on the Show my Skills Health Check documents")]
        public void WhenIClickOnTheShowMySkillsHealthCheckDocuments()
        {
            GetNavigatedPage<SkillsHealthCheckHomePage>().ClickShowMySkillHealthCheck<YourAccountHomePage>().SaveTo(ScenarioContext);
        }

        [When(@"I click the View link")]
        public void WhenIClickTheViewLink()
        {
            GetNavigatedPage<YourAccountHomePage>().ClickViewLink<YourAssesmentsPage>().SaveTo(ScenarioContext);
        }

        [Then(@"I am asked to confirm the delete")]
        public void ThenIAmAskedToConfirmTheDelete()
        {
            GetNavigatedPage<YourAccountHomePage>().PageSectionTitle.Should().Be("You've asked to delete a document");
        }

        [When(@"I confirm the delete")]
        public void WhenIConfirmTheDelete()
        {
            GetNavigatedPage<YourAccountHomePage>().ClickDeleteButton<YourAccountHomePage>().SaveTo(ScenarioContext);
        }

        [When(@"I click on Sign out")]
        public void WhenIClickOnSignOut()
        {
            GetNavigatedPage<YourAccountHomePage>().ClickSignOutLink<Homepage>().SaveTo(ScenarioContext);
        }

        [Then(@"I am signed out of my account")]
        public void ThenIAmSignedOutOfMyAccount()
        {
            GetNavigatedPage<Homepage>().Title.Should().Contain("Explore careers");
        }
    }
}
