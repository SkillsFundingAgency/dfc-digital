using ASP;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using RazorGenerator.Testing;
using System;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class TrainingCoursesViewTests : MemberDataHelper
    {
        [Theory]
        [MemberData(nameof(Dfc7055SearchResultsViewTestsInput))]
        public void Dfc7055SearchResultsViewTests(int coursesCount, string pageTitle, string noCoursesFoundText)
        {
            // Assign
            var searchResultsView = new _MVC_Views_TrainingCourses_SearchResults_cshtml();
            var viewModel = new CourseSearchResultsViewModel
            {
                PageTitle = pageTitle,
                Courses = GetCourseListings(coursesCount, true).ToList(),
                NoTrainingCoursesFoundText = noCoursesFoundText
            };

            // Act
            var htmlDom = searchResultsView.RenderAsHtml(viewModel);

            // Assert
            AssertTagInnerTextValue(htmlDom, pageTitle, "h1");
            if (coursesCount > 0)
            {
                AssertTagInnerTextValueDoesNotExist(htmlDom, noCoursesFoundText, "h3");
                AssertExistsElementIdWithInnerHtml(htmlDom, "results-summary");
            }
            else
            {
                AssertTagInnerTextValue(htmlDom, noCoursesFoundText, "h3");
                AssertElementDoesNotExistsById(htmlDom, "results-summary");
            }
        }

        [Theory]
        [MemberData(nameof(Dfc7055PaginationViewTestsInput))]
        public void Dfc7055PaginationViewTests(bool hasNextPage, bool hasPreviousPage, string nextPageText, string previousPageText, string pathQuery)
        {
            // Assign
            var paginationView = new _MVC_Views_TrainingCourses_Pagination_cshtml();
            var viewModel = new PaginationViewModel
            {
                HasPreviousPage = hasPreviousPage,
                HasNextPage = hasNextPage,
                NextPageUrl = new Uri(pathQuery, UriKind.RelativeOrAbsolute),
                NextPageUrlText = nextPageText,
                PreviousPageUrl = new Uri(pathQuery, UriKind.RelativeOrAbsolute),
                PreviousPageUrlText = previousPageText
            };

            // Act
            var htmlDom = paginationView.RenderAsHtml(viewModel);

            // Assert
            AssertElementExistsByTagAndClassName(htmlDom, "ul", "previous-next-navigation");
            if (hasNextPage)
            {
                AssertTagInnerTextValue(htmlDom, nextPageText, "span");
                AssertElementExistsByAttributeAndValue(htmlDom, "a", "href", viewModel.NextPageUrl.OriginalString);
            }

            if (hasPreviousPage)
            {
                AssertTagInnerTextValue(htmlDom, previousPageText, "span");
                AssertElementExistsByAttributeAndValue(htmlDom, "a", "href", viewModel.PreviousPageUrl.OriginalString);
            }
        }

        [Theory]
        [InlineData(CourseSearchOrderBy.Distance)]
        [InlineData(CourseSearchOrderBy.StartDate)]
        [InlineData(CourseSearchOrderBy.Relevance)]
        public void Dfc7055OrderLinksViewTests(CourseSearchOrderBy courseSearchSortBy)
        {
            // Assign
            var searchResultsView = new _MVC_Views_TrainingCourses_OrderByLinks_cshtml();
            var viewModel = new OrderByLinks
            {
                CourseSearchSortBy = courseSearchSortBy,
                StartDateOrderByText = nameof(OrderByLinks.StartDateOrderByText),
                DistanceOrderByText = nameof(OrderByLinks.DistanceOrderByText),
                RelevanceOrderByText = nameof(OrderByLinks.RelevanceOrderByText)
            };

            // Act
            var htmlDom = searchResultsView.RenderAsHtml(viewModel);

            // Assert
            switch (courseSearchSortBy)
            {
                case CourseSearchOrderBy.Relevance:
                    AssertTagInnerTextValue(htmlDom, viewModel.RelevanceOrderByText, "span");
                    break;
                case CourseSearchOrderBy.Distance:
                    AssertTagInnerTextValue(htmlDom, viewModel.DistanceOrderByText, "span");
                    break;
                default:
                case CourseSearchOrderBy.StartDate:
                    AssertTagInnerTextValue(htmlDom, viewModel.StartDateOrderByText, "span");
                    break;
            }
        }

        [Theory]
        [MemberData(nameof(Dfc7055CourseDetailsViewTestsInput))]
        public void Dfc7055CourseDetailsViewTests(
            Course course,
            string providerLabel,
            string advancedLoanProviderLabel,
            string locationLabel,
            string startDateLabel)
        {
            // Assign
            var courseDetailsView = new _MVC_Views_TrainingCourses_CourseDetail_cshtml();
            var viewModel = new CourseListingViewModel
            {
                Course = course,
                ProviderLabel = providerLabel,
                AdvancedLoanProviderLabel = advancedLoanProviderLabel,
                LocationLabel = locationLabel,
                StartDateLabel = startDateLabel
            };

            // Act
            var htmlDom = courseDetailsView.RenderAsHtml(viewModel);

            // Assert
            AssertTagInnerTextValue(htmlDom, viewModel.AdvancedLoanProviderLabel, "span");
            AssertTagInnerTextValue(htmlDom, viewModel.ProviderLabel, "span");
            if (!string.IsNullOrWhiteSpace(viewModel.Course.Location))
            {
                AssertTagInnerTextValue(htmlDom, viewModel.LocationLabel, "span");
            }
            else
            {
                AssertTagInnerTextValueDoesNotExist(htmlDom, viewModel.LocationLabel, "span");
            }

            if (viewModel.Course.QualificationLevel.ToLowerInvariant().Contains("unknown"))
            {
                AssertTagInnerTextValueDoesNotExist(htmlDom, viewModel.Course.QualificationLevel, "li");
            }
            else
            {
                AssertTagInnerTextValue(htmlDom, viewModel.Course.QualificationLevel, "li");
            }
        }
    }
}
