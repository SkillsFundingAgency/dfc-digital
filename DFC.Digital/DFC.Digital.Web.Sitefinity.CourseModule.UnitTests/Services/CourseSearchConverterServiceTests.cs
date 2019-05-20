using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class CourseSearchConverterServiceTests : MemberDataHelper
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

        [Theory]
        [MemberData(nameof(SetupPagingTestsInput))]
        public void SetupPagingTest(TrainingCourseResultsViewModel viewModel, CourseSearchResponse response, string pathQuery, int recordsPerPage, string courseSearchResultsPage, TrainingCourseResultsViewModel expectedViewModel)
        {
            //Assign
            var courseSearchConverter = new CourseSearchConverterService();

            //Act
            courseSearchConverter.SetupPaging(viewModel, response, pathQuery, recordsPerPage, courseSearchResultsPage);

            //Assert
            viewModel.PaginationViewModel.NextPageUrl.Should().BeEquivalentTo(expectedViewModel.PaginationViewModel.NextPageUrl);
            viewModel.PaginationViewModel.NextPageUrlText.Should().BeEquivalentTo(expectedViewModel.PaginationViewModel.NextPageUrlText);
            viewModel.PaginationViewModel.PreviousPageUrl.Should().BeEquivalentTo(expectedViewModel.PaginationViewModel.PreviousPageUrl);
            viewModel.PaginationViewModel.PreviousPageUrlText.Should().BeEquivalentTo(expectedViewModel.PaginationViewModel.PreviousPageUrlText);
            viewModel.PaginationViewModel.HasPreviousPage.Should().Be(expectedViewModel.PaginationViewModel.HasPreviousPage);
            viewModel.PaginationViewModel.HasNextPage.Should().Be(expectedViewModel.PaginationViewModel.HasNextPage);
        }

        [Theory]
        [MemberData(nameof(GetFilterSelectItemsTestInput))]
        public void GetActiveFilterOptionsTest(CourseFiltersModel courseFiltersModel, Dictionary<string, string> expectedDictionary)
        {
            //Assign
            var courseSearchConverter = new CourseSearchConverterService();

            //Act
            var result = courseSearchConverter.GetActiveFilterOptions(courseFiltersModel);

            //Assert
            result.Should().BeEquivalentTo(expectedDictionary);
        }

        [Theory]
        [MemberData(nameof(GetOrderByLinksTestsInput))]
        public void GetOrderByLinksTest(string searchUrl, CourseSearchSortBy courseSearchSortBy, OrderByLinks expectedOrderByLinks)
        {
            //Assign
            var courseSearchConverter = new CourseSearchConverterService();

            //Act
            var result = courseSearchConverter.GetOrderByLinks(searchUrl, courseSearchSortBy);

            //Assert
            result.Should().BeEquivalentTo(expectedOrderByLinks);
        }

        [Theory]
        [MemberData(nameof(GetFilterSelectItemsTestsInput))]
        public void GetFilterSelectItemsTest(string propertyName, IEnumerable<string> sourceList, string value, IEnumerable<SelectItem> expectedSelectItems)
        {
            //Assign
            var courseSearchConverter = new CourseSearchConverterService();

            //Act
            var result = courseSearchConverter.GetFilterSelectItems(propertyName, sourceList, value);

            //Assert
            result.Should().BeEquivalentTo(expectedSelectItems);
        }
    }
}