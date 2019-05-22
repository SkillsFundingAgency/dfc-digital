using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Services.Tests
{
    public class BuildQueryStringServiceTests : MemberDataHelper
    {
        [Theory]
        [MemberData(nameof(BuildRedirectPathAndQueryStringTestsInput))]
        public void BuildRedirectPathAndQueryStringTest(string courseSearchResultsPage, CourseSearchResultsViewModel trainingCourseResultsViewModel, string expectedPathAndQuery)
        {
            //Assign
            var courseSearchConverter = new BuildQueryStringService();

            //Act
            var result = courseSearchConverter.BuildRedirectPathAndQueryString(courseSearchResultsPage, trainingCourseResultsViewModel.SearchTerm, trainingCourseResultsViewModel.CourseFiltersModel);

            //Assert
            result.Should().BeEquivalentTo(expectedPathAndQuery);
        }

        [Theory]
        [MemberData(nameof(BuildSearchRedirectPathAndQueryStringTestsInput))]
        public void BuildSearchRedirectPathAndQueryStringTest(string courseSearchResultsPage, CourseLandingViewModel courseLandingViewModel, string expectedPathAndQuery)
        {
            //Assign
            var courseSearchConverter = new BuildQueryStringService();

            //Act
            var result = courseSearchConverter.BuildRedirectPathAndQueryString(courseSearchResultsPage, courseLandingViewModel.SearchTerm, courseLandingViewModel);

            //Assert
            result.Should().BeEquivalentTo(expectedPathAndQuery);
        }
    }
}