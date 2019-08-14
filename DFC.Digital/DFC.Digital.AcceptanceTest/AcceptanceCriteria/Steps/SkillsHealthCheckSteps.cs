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

        [Given(@"I navigate to the Your Assessments page")]
        public void GivenINavigateToTheYourAssessmentsPage()
        {
            NavigateToYourAssesmentsPage<YourAssessmentsPage>();
        }

        [When(@"I select to start the Motivation check")]
        public void WhenISelectToStartTheMotivationCheck()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I answer all (.*) questions")]
        public void WhenIAnswerAllQuestions(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I click on the Return to Skills health check page button")]
        public void WhenIClickOnTheReturnToSkillsHealthCheckPageButton()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I am redirected to the start page for a Motivation check with (.*) questions to answer")]
        public void ThenIAmRedirectedToTheStartPageForAMotivationCheckWithQuestionsToAnswer(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"on the last question I see the Return to Skills health check page button")]
        public void ThenOnTheLastQuestionISeeTheReturnToSkillsHealthCheckPageButton()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I am taken back to the Your Assessments page")]
        public void ThenIAmTakenBackToTheYourAssessmentsPage()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I can down load my completed assessment as a PDF")]
        public void ThenICanDownLoadMyCompletedAssessmentAsAPDF()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"I can down load my completed assessment as a Word document")]
        public void ThenICanDownLoadMyCompletedAssessmentAsAWordDocument()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
