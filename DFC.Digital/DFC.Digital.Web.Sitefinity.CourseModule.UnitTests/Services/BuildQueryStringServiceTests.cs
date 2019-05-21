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
        public void BuildRedirectPathAndQueryStringTest(string courseSearchResultsPage, TrainingCourseResultsViewModel trainingCourseResultsViewModel, string locationDistanceRegex, string expectedPathAndQuery)
        {
            //Assign
            var courseSearchConverter = new BuildQueryStringService();

            //Act
            var result = courseSearchConverter.BuildRedirectPathAndQueryString(courseSearchResultsPage, trainingCourseResultsViewModel, locationDistanceRegex);

            //Assert
            result.Should().BeEquivalentTo(expectedPathAndQuery);
        }

        [Theory]
        [MemberData(nameof(BuildSearchRedirectPathAndQueryStringTestsInput))]
        public void BuildSearchRedirectPathAndQueryStringTest(string courseSearchResultsPage, CourseLandingViewModel courseLandingViewModel, string locationDistanceRegex, string expectedPathAndQuery)
        {
            //Assign
            var courseSearchConverter = new BuildQueryStringService();

            //Act
            var result = courseSearchConverter.BuildSearchRedirectPathAndQueryString(courseSearchResultsPage, courseLandingViewModel, locationDistanceRegex);

            //Assert
            result.Should().BeEquivalentTo(expectedPathAndQuery);
        }

        [Theory]
        [MemberData(nameof(GetUrlEncodedStringTestsInput))]
        public void GetUrlEncodedStringTest(string input, string expectedOutput)
        {
            //Assign
            var courseSearchConverter = new BuildQueryStringService();

            //Act
            var result = courseSearchConverter.GetUrlEncodedString(input);

            //Assert
            result.Should().BeEquivalentTo(expectedOutput);
        }
    }
}