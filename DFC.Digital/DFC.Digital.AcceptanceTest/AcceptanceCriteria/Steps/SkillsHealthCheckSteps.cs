using DFC.Digital.AcceptanceTest.Infrastructure;
using DFC.Digital.AcceptanceTest.Infrastructure.Pages;
using FluentAssertions;
using System;
using System.Collections.Generic;
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

        [Given(@"I click on Start a new Skills Health Check button")]
        public void GivenIClickOnStartANewSkillsHealthCheckButton()
        {
            GetNavigatedPage<SkillsHealthCheckHomePage>().ClickStartANewSkillsHealthCheck<YourAssesmentsPage>().SaveTo(ScenarioContext);
        }

        [Then(@"I am directed to the Your Assessments List page")]
        public void ThenIAmDirectedToTheYourAssessmentsListPage()
        {
            GetNavigatedPage<YourAssesmentsPage>().PageTitle.Should().Contain("Skills Health Check");
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

        [Then(@"I am redirected to the start page for a (.*) check with (.*) questions to answer")]
        public void ThenIAmRedirectedToTheStartPageForASkillsHealthCheckCheckWithQuestionsToAnswer(string typeOfSkillsHealthCheck, int numberOfQuestions)
        {
            GetNavigatedPage<SkillsHealthCheckPage>().PageTitle.ToUpperInvariant().Should().Contain(typeOfSkillsHealthCheck.ToUpperInvariant());
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
            ScenarioContext.Current.Pending();
        }
    }
}
