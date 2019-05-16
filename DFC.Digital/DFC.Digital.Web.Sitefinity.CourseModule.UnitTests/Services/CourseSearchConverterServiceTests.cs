using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class CourseSearchConverterServiceTests : HelperSearchResultsData
    {
        [Theory]
        [MemberData(nameof(BuildRedirectPathAndQueryStringTestsInput))]
        public void BuildRedirectPathAndQueryStringTest(string courseSearchResultsPage, TrainingCourseResultsViewModel trainingCourseResultsViewModel, string locationDistanceRegex, string expectedPathAndQuery)
        {
            //Assign
            var courseSearchConverter = new CourseSearchConverterService();

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
            var courseSearchConverter = new CourseSearchConverterService();

            //Act
            var result = courseSearchConverter.BuildSearchRedirectPathAndQueryString(courseSearchResultsPage, courseLandingViewModel, locationDistanceRegex);

            //Assert
            result.Should().BeEquivalentTo(expectedPathAndQuery);
        }

        [Theory]
        [MemberData(nameof(GetCourseSearchRequestTestsInput))]
        public void GetCourseSearchRequestTest(string searchTerm, int recordsPerPage, string attendance, string studyMode, string qualificationLevel, string distance, string dfe1619Funded, string pattern, string location, string sortBy, string provider, int page, CourseSearchRequest expectedCourseSearchRequest)
        {
          //Assign
          var courseSearchConverter = new CourseSearchConverterService();

          //Act
          var result = courseSearchConverter.GetCourseSearchRequest(searchTerm, recordsPerPage, attendance, studyMode, qualificationLevel, distance, dfe1619Funded, pattern, location, sortBy, provider, page);

          //Assert
          result.Should().BeEquivalentTo(expectedCourseSearchRequest);
        }

        [Theory]
        [MemberData(nameof(GetUrlEncodedStringTestsInput))]
        public void GetUrlEncodedStringTest(string input, string expectedOutput)
        {
            //Assign
            var courseSearchConverter = new CourseSearchConverterService();

            //Act
            var result = courseSearchConverter.GetUrlEncodedString(input);

            //Assert
            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public void SetupPagingTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void GetFilterSelectItemsTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void GetOrderByLinksTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void GetActiveFilterOptionsTest()
        {
            Assert.True(false, "This test needs an implementation");
        }
    }
}