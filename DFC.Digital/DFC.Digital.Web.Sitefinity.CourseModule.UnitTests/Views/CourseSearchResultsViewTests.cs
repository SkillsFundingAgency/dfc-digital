using ASP;
using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using RazorGenerator.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class CourseSearchResultsViewTests : MemberDataHelper
    {
        [Theory]
        [MemberData(nameof(Dfc9208CourseFiltersViewModelViewTestsInput))]
        public static void Dfc9208ActiveFiltersViewTests(CourseFiltersViewModel viewModel)
        {
            viewModel = viewModel ?? new CourseFiltersViewModel();

            // Assign
            var searchResultsView = new _MVC_Views_CourseSearchResults_ActiveFilters_cshtml();

            // Act
            var htmlDom = searchResultsView.RenderAsHtml(viewModel);

            // Assert
            if (viewModel.CourseHours != CourseHours.All)
            {
                AssertTagInnerTextValue(htmlDom, viewModel.CourseHoursDisplayName, "p");
            }

            if (viewModel.CourseType != CourseType.All)
            {
                AssertTagInnerTextValue(htmlDom, viewModel.CourseTypeDisplayName, "p");
            }

            if (viewModel.StartDate == StartDate.Anytime)
            {
                AssertTagInnerTextValueDoesNotExist(htmlDom, viewModel.ActiveFiltersStartingFromText, "span");
            }
            else
            {
                AssertTagInnerTextValue(htmlDom, viewModel.ActiveFiltersStartingFromText, "span");
            }

            if (viewModel.StartDate == StartDate.FromToday)
            {
                AssertTagInnerTextValue(htmlDom, DateTime.Now.ToString(Constants.CourseSearchFrontEndStartDateFormat), "p");
            }

            if (viewModel.StartDate == StartDate.SelectDateFrom)
            {
                AssertTagInnerTextValue(htmlDom, viewModel.StartDateFrom.ToString(Constants.CourseSearchFrontEndStartDateFormat), "p");
            }

            if (viewModel.Only1619Courses)
            {
                AssertTagInnerTextValue(htmlDom, viewModel.ActiveFiltersSuitableForText, "span");
            }

            if (!string.IsNullOrWhiteSpace(viewModel.Location))
            {
                AssertTagInnerTextValue(htmlDom, viewModel.Location, "p");
            }

            if (!string.IsNullOrWhiteSpace(viewModel.Provider))
            {
                AssertTagInnerTextValue(htmlDom, viewModel.Provider, "p");
                AssertTagInnerTextValue(htmlDom, viewModel.ActiveFiltersProvidedByText, "span");
            }

            if (viewModel.IsDistanceLocation)
            {
                AssertTagInnerTextValue(htmlDom, $"{viewModel.Distance} {viewModel.ActiveFiltersMilesText}", "p");
            }
        }

        [Theory]
        [MemberData(nameof(Dfc7055SearchResultsViewTestsInput))]
        public void Dfc7055SearchResultsViewTests(int coursesCount, string pageTitle, string noCoursesFoundText)
        {
            // Assign
            var searchResultsView = new _MVC_Views_CourseSearchResults_SearchResults_cshtml();
            var viewModel = new CourseSearchResultsViewModel
            {
                PageTitle = pageTitle,
                Courses = GetCourseListings(coursesCount, true).ToList(),
                NoTrainingCoursesFoundText = noCoursesFoundText,
                SearchForCourseNameText = nameof(CourseSearchResultsViewModel.SearchForCourseNameText)
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
            var paginationView = new _MVC_Views_CourseSearchResults_Pagination_cshtml();
            var viewModel = new PaginationViewModel
            {
                HasPreviousPage = hasPreviousPage,
                HasNextPage = hasNextPage,
                NextPageUrl = new Uri(pathQuery, UriKind.RelativeOrAbsolute),
                NextPageText = nextPageText,
                PreviousPageUrl = new Uri(pathQuery, UriKind.RelativeOrAbsolute),
                PreviousPageText = previousPageText
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
            var searchResultsView = new _MVC_Views_CourseSearchResults_OrderByLinks_cshtml();
            var viewModel = new OrderByLinks
            {
                OrderBy = courseSearchSortBy,
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

            var orderList = new List<string>
            {
                viewModel.RelevanceOrderByText,
                viewModel.StartDateOrderByText,
                viewModel.DistanceOrderByText
            };

            AssertOrderOfTextDisplayed(
                htmlDom,
                orderList);
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
            var courseDetailsView = new _MVC_Views_CourseSearchResults_CourseDetail_cshtml();
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
            if (viewModel.Course.LocationDetails != null)
            {
                AssertTagInnerTextValue(htmlDom, viewModel.LocationLabel, "span");

                if (!viewModel.Course.LocationDetails.Distance.Equals(default(float)))
                {
                    AssertTagInnerTextValue(htmlDom, "miles)", "li");
                }
            }
            else
            {
                AssertTagInnerTextValueDoesNotExist(htmlDom, viewModel.LocationLabel, "span");
            }
        }

        [Theory]
        [MemberData(nameof(Dfc9208CourseFiltersViewModelViewTestsInput))]
        public void Dfc9208FiltersViewTests(CourseFiltersViewModel viewModel)
        {
            viewModel = viewModel ?? new CourseFiltersViewModel();

            // Assign
            var searchResultsView = new _MVC_Views_CourseSearchResults_Filters_cshtml();

            // Act
            var htmlDom = searchResultsView.RenderAsHtml(viewModel);

            // Assert
            AssertTagInnerTextValue(htmlDom, viewModel.StartDateSectionText, "h2");
            AssertTagInnerTextValue(htmlDom, viewModel.CourseHoursSectionText, "h2");
            AssertTagInnerTextValue(htmlDom, viewModel.CourseTypeSectionText, "h2");
            AssertTagInnerTextValue(htmlDom, viewModel.ApplyFiltersText, "button");
            AssertElementExistsByAttributeAndValue(htmlDom, "input", "name", nameof(CourseFiltersViewModel.Location));
            AssertElementExistsByAttributeAndValue(htmlDom, "input", "name", nameof(CourseFiltersViewModel.Provider));
            AssertElementExistsByAttributeAndTypeAndValue(htmlDom, "input", "name", nameof(CourseFiltersViewModel.CourseHours), "radio");
            AssertElementIsSelectedByAttributeAndValue(htmlDom, "input", "value", viewModel.CourseHours.ToString());
            AssertElementExistsByAttributeAndTypeAndValue(htmlDom, "input", "name", nameof(CourseFiltersViewModel.StartDate), "radio");
            AssertElementIsSelectedByAttributeAndValue(htmlDom, "input", "value", viewModel.StartDate.ToString());
            AssertElementExistsByAttributeAndTypeAndValue(htmlDom, "input", "name", nameof(CourseFiltersViewModel.CourseType), "radio");
            AssertElementIsSelectedByAttributeAndValue(htmlDom, "input", "value", viewModel.CourseType.ToString());
            AssertElementExistsByAttributeAndValue(htmlDom, "input", "name", nameof(CourseFiltersViewModel.Only1619Courses));
            if (viewModel.Only1619Courses)
            {
                AssertElementIsSelectedByAttributeAndValue(htmlDom, "radio", "value", true.ToString());
            }
        }
    }
}
