using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class CourseSearchViewModelServiceTests : MemberDataHelper
    {
        [Theory]
        [MemberData(nameof(SetupPagingTestsInput))]
        public void SetupPagingTest(TrainingCourseResultsViewModel viewModel, CourseSearchResult response, string pathQuery, int recordsPerPage, string courseSearchResultsPage, TrainingCourseResultsViewModel expectedViewModel)
        {
            //Assign
            var courseSearchConverter = new CourseSearchViewModelService();

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
            var courseSearchConverter = new CourseSearchViewModelService();

            //Act
            var result = courseSearchConverter.GetActiveFilterOptions(courseFiltersModel);

            //Assert
            result.Should().BeEquivalentTo(expectedDictionary);
        }

        [Theory]
        [MemberData(nameof(GetOrderByLinksTestsInput))]
        public void GetOrderByLinksTest(string searchUrl, CourseSearchOrderBy courseSearchSortBy, OrderByLinks expectedOrderByLinks)
        {
            //Assign
            var courseSearchConverter = new CourseSearchViewModelService();

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
            var courseSearchConverter = new CourseSearchViewModelService();

            //Act
            var result = courseSearchConverter.GetFilterSelectItems(propertyName, sourceList, value);

            //Assert
            result.Should().BeEquivalentTo(expectedSelectItems);
        }
    }
}