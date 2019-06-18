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
        public void SetupPagingTest(CourseSearchResultsViewModel viewModel, CourseSearchResult response, string pathQuery, int recordsPerPage, CourseSearchResultsViewModel expectedViewModel)
        {
            viewModel = viewModel ?? new CourseSearchResultsViewModel();
            expectedViewModel = expectedViewModel ?? new CourseSearchResultsViewModel();

            //Assign
            var courseSearchConverter = new CourseSearchResultsViewModelBullder();

            //Act
            courseSearchConverter.SetupViewModelPaging(viewModel, response, pathQuery, recordsPerPage);

            //Assert
            viewModel.PaginationViewModel.NextPageUrl.Should().BeEquivalentTo(expectedViewModel.PaginationViewModel.NextPageUrl);
            viewModel.PaginationViewModel.NextPageText.Should().BeEquivalentTo(expectedViewModel.PaginationViewModel.NextPageText);
            viewModel.PaginationViewModel.PreviousPageUrl.Should().BeEquivalentTo(expectedViewModel.PaginationViewModel.PreviousPageUrl);
            viewModel.PaginationViewModel.PreviousPageText.Should().BeEquivalentTo(expectedViewModel.PaginationViewModel.PreviousPageText);
            viewModel.PaginationViewModel.HasPreviousPage.Should().Be(expectedViewModel.PaginationViewModel.HasPreviousPage);
            viewModel.PaginationViewModel.HasNextPage.Should().Be(expectedViewModel.PaginationViewModel.HasNextPage);
        }

        [Theory]
        [MemberData(nameof(GetOrderByLinksTestsInput))]
        public void GetOrderByLinksTest(string searchUrl, CourseSearchOrderBy courseSearchSortBy, OrderByLinks expectedOrderByLinks)
        {
            //Assign
            var courseSearchConverter = new CourseSearchResultsViewModelBullder();

            //Act
            var result = courseSearchConverter.GetOrderByLinks(searchUrl, courseSearchSortBy);

            //Assert
            result.Should().BeEquivalentTo(expectedOrderByLinks);
        }
    }
}