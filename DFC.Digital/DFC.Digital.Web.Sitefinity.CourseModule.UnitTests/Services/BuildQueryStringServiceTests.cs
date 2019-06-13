using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Services.Tests
{
    public class BuildQueryStringServiceTests : MemberDataHelper
    {
        [Theory]
        [MemberData(nameof(BuildRedirectPathAndQueryStringTestsInput))]
        public static void BuildRedirectBuildPathAndQueryStringTest(string courseSearchResultsPage, CourseSearchResultsViewModel trainingCourseResultsViewModel, string expectedPathAndQuery)
        {
            trainingCourseResultsViewModel = trainingCourseResultsViewModel ?? new CourseSearchResultsViewModel();

            //Assign
            var buildQueryStringService = new QueryStringBuilder();

            //Act
            var result = buildQueryStringService.BuildPathAndQueryString(courseSearchResultsPage, trainingCourseResultsViewModel.CourseFiltersModel);

            //Assert
            result.Should().BeEquivalentTo(expectedPathAndQuery);
        }

        [Theory]
        [MemberData(nameof(BuildSearchRedirectPathAndQueryStringTestsInput))]
        public static void BuildSearchRedirectBuildPathAndQueryStringTest(string courseSearchResultsPage, CourseLandingViewModel courseLandingViewModel, string expectedPathAndQuery)
        {
            courseLandingViewModel = courseLandingViewModel ?? new CourseLandingViewModel();

            //Assign
            var buildQueryStringService = new QueryStringBuilder();

            //Act
            var result = buildQueryStringService.BuildPathAndQueryString(courseSearchResultsPage, courseLandingViewModel);

            //Assert
            result.Should().BeEquivalentTo(expectedPathAndQuery);
        }
    }
}