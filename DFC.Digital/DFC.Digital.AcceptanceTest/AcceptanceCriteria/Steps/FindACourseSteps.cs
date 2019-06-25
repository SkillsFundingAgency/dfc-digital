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
    public class FindACourseSteps : BaseStep
    {
        public FindACourseSteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
        {
        }

        #region Givens
        [Given(@"that I am viewing the Find A Course landing page")]
        public void GivenThatIAmViewingTheFindACourseLandingPage()
        {
            NavigateToFindACoursePage<FaCLandingPage>();
        }

        #endregion

        #region Whens
        [When(@"I click the Find a Course link")]
        public void WhenIClickTheFindACourseLink()
        {
            GetNavigatedPage<Homepage>().ClickFindACourseLink<FaCLandingPage>()
                .SaveTo(ScenarioContext);
        }

        [When(@"I search for the course '(.*)'")]
        public void WhenISearchForTheCourse(string courseName)
        {
            var landingPage = GetNavigatedPage<FaCLandingPage>();
            landingPage.EnterCourseName(courseName);
            landingPage.SearchCourses<FaCResultsPage>()
                .SaveTo(ScenarioContext);
        }

        [When(@"I click on course result no '(.*)'")]
        public void WhenIClickOnCourseResultNo(int resultNo)
        {
            var resultsPage = GetNavigatedPage<FaCResultsPage>();
            ScenarioContext.Add("SelectedCourseText", resultsPage.SelectedCourseText(resultNo));
            resultsPage.SelectCourse<FaCCourseDetailsPage>(resultNo)
                .SaveTo(ScenarioContext);
        }

        [When(@"I search for the Course (.*), provider (.*), location (.*), show courses (.*)")]
        public void WhenISearchForTheCourseProviderLocationShowCourses(string courseName, string provider, string location, string show16to19)
        {
            var landingPage = GetNavigatedPage<FaCLandingPage>();
            landingPage.ApplyFilters<FaCResultsPage>(courseName, provider, location, show16to19)
                .SaveTo(ScenarioContext);
        }

        [When(@"I change the provider (.*) and location (.*)")]
        public void WhenIChangeTheProviderSkillsAndLocationBirmingham(string provider, string location)
        {
            var resultPage = GetNavigatedPage<FaCResultsPage>();
            resultPage.UpdateFilters(provider, location);
            resultPage.ApplyFiltersButton<FaCResultsPage>().SaveTo(ScenarioContext);
        }

        [When(@"I apply the filter hours (.*), type (.*) and start date (.*)")]
        public void WhenIApplyTheFilterHoursTypeAndStartDate(string courseHours, string courseType, string startDate)
        {
            var resultPage = GetNavigatedPage<FaCResultsPage>();
            if (!string.IsNullOrWhiteSpace(courseHours))
            {
                resultPage.SelectCourseFilter(courseHours);
            }

            if (!string.IsNullOrWhiteSpace(courseType))
            {
                resultPage.SelectCourseFilter(courseType);
            }

            if (!string.IsNullOrWhiteSpace(startDate))
            {
                resultPage.SelectCourseFilter(startDate);
            }

            resultPage.ApplyFiltersButton<FaCResultsPage>().SaveTo(ScenarioContext);
        }
        #endregion

        #region Thens
        [Then(@"I am redirected to the Find a Course landing page")]
        public void ThenIAmRedirectedToTheFindACourseLandingPage()
        {
            GetNavigatedPage<FaCLandingPage>().PageTitle.Should().Contain("Find a course");
        }

        [Then(@"I am shown course results for '(.*)'")]
        public void ThenIAmShownCourseResultsFor(string courseName)
        {
            var resultsPage = GetNavigatedPage<FaCResultsPage>();
            resultsPage.KeywordSearchText.Should().Contain(courseName);
            resultsPage.NumberOfCoursesDisplayed.Should().BeGreaterThan(0);
        }

        [Then(@"I am redirected to the correct course details page")]
        public void ThenIAmRedirectedToTheCorrectCourseDetailsPage()
        {
            ScenarioContext.TryGetValue("SelectedCourseText", out string selectedCourseText);
            var courseDetailsPage = GetNavigatedPage<FaCCourseDetailsPage>();
            courseDetailsPage.CourseDetailsTitle.Should().Contain(selectedCourseText);
            courseDetailsPage.HasQualificationSection.Should().BeTrue();
            courseDetailsPage.HasCourseDescriptionSection.Should().BeTrue();
            courseDetailsPage.HasEntryRequirementSection.Should().BeTrue();
            courseDetailsPage.HasEquipmentRequiredSection.Should().BeTrue();
            courseDetailsPage.HasAssessmentMethodSection.Should().BeTrue();
            courseDetailsPage.HasVenueSection.Should().BeTrue();
            courseDetailsPage.HasOtherDatesAndVenuesSection.Should().BeTrue();
        }

        [Then(@"the filters applied (.*), (.*), (.*)")]
        public void ThenTheFiltersAppliedBeauchampLeicesterYes(string provider, string location, string show16to19)
        {
            var resultsPage = GetNavigatedPage<FaCResultsPage>();
            if (!string.IsNullOrWhiteSpace(provider))
            {
                resultsPage.ProviderText.Should().Contain(provider);
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                resultsPage.LocationText.Should().Contain(location);
            }

            if (!string.IsNullOrWhiteSpace(show16to19) && show16to19.ToLower().Equals("yes"))
            {
                resultsPage.Show16To19CheckBoxSelected.Should().BeTrue();
            }
        }

        [Then(@"the results should be updated with the new provider (.*) and location (.*)")]
        public void ThenTheResultsShouldBeUpdatedWithTheNewProviderAndLocation(string provider, string location)
        {
            var resultsPage = GetNavigatedPage<FaCResultsPage>();
            if (!string.IsNullOrWhiteSpace(provider))
            {
                resultsPage.ProviderText.Should().Contain(provider);
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                resultsPage.LocationText.Should().Contain(location);
            }
        }

        [Then(@"I am shown 0 results with the correct messaging")]
        public void ThenIAmShownResultsWithTheCorrectMessaging()
        {
            var resultsPage = GetNavigatedPage<FaCResultsPage>();
            resultsPage.NoResultsText.Should().Contain("We didn't find any results for 'NoCourse' with the active filters you've applied. Try searching again.");
        }

        [Then(@"the following filters (.*), (.*), (.*) are selected")]
        public void ThenTheFollowingFiltersAreSelected(string courseHours, string courseType, string startDate)
        {
            var resultPage = GetNavigatedPage<FaCResultsPage>();
            if (!string.IsNullOrWhiteSpace(courseHours))
            {
                resultPage.IsCourseFilterSelected(courseHours).Should().BeTrue();
            }

            if (!string.IsNullOrWhiteSpace(courseHours))
            {
                resultPage.IsCourseFilterSelected(courseType).Should().BeTrue();
            }

            if (!string.IsNullOrWhiteSpace(courseHours))
            {
                resultPage.IsCourseFilterSelected(startDate).Should().BeTrue();
            }
        }

        #endregion
    }
}