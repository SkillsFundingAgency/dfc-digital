using DFC.Digital.Data.Model;
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
        public void SetupPagingTest(CourseSearchResultsViewModel viewModel, CourseSearchResult response, string pathQuery, int recordsPerPage, string courseSearchResultsPage, CourseSearchResultsViewModel expectedViewModel)
        {
            //Assign
            var courseSearchConverter = new CourseSearchViewModelService();

            //Act
            courseSearchConverter.SetupPaging(viewModel, response, pathQuery, recordsPerPage, courseSearchResultsPage);

            //Assert
            viewModel.PaginationViewModel.NextPageUrl.Should().BeEquivalentTo(expectedViewModel.PaginationViewModel.NextPageUrl);
            viewModel.PaginationViewModel.NextPageText.Should().BeEquivalentTo(expectedViewModel.PaginationViewModel.NextPageText);
            viewModel.PaginationViewModel.PreviousPageUrl.Should().BeEquivalentTo(expectedViewModel.PaginationViewModel.PreviousPageUrl);
            viewModel.PaginationViewModel.PreviousPageText.Should().BeEquivalentTo(expectedViewModel.PaginationViewModel.PreviousPageText);
            viewModel.PaginationViewModel.HasPreviousPage.Should().Be(expectedViewModel.PaginationViewModel.HasPreviousPage);
            viewModel.PaginationViewModel.HasNextPage.Should().Be(expectedViewModel.PaginationViewModel.HasNextPage);
        }

        [Theory]
        [MemberData(nameof(GetFilterSelectItemsTestInput))]
        public void GetActiveFilterOptionsTest(CourseFiltersViewModel courseFiltersModel, Dictionary<string, string> expectedDictionary)
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