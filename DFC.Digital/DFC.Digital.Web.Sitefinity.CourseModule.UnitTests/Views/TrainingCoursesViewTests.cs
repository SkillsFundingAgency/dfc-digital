using ASP;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using RazorGenerator.Testing;
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
            var viewModel = new TrainingCourseResultsViewModel
            {
                PageTitle = pageTitle,
                Courses = GetCourses(coursesCount, true).ToList(),
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
        [MemberData(nameof(Dfc7055SearchResultsViewTestsInput))]
        public void Dfc7055PaginationViewTests(int coursesCount, string pageTitle, string noCoursesFoundText)
        {
            // Assign
            var searchResultsView = new _MVC_Views_TrainingCourses_SearchResults_cshtml();
            var viewModel = new TrainingCourseResultsViewModel
            {
                PageTitle = pageTitle,
                Courses = GetCourses(coursesCount, true).ToList(),
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
        [InlineData(CourseSearchSortBy.Distance)]
        [InlineData(CourseSearchSortBy.StartDate)]
        [InlineData(CourseSearchSortBy.Relevance)]
        public void Dfc7055OrderLinksViewTests(CourseSearchSortBy courseSearchSortBy)
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
                case CourseSearchSortBy.Relevance:
                    AssertTagInnerTextValue(htmlDom, viewModel.RelevanceOrderByText, "span");
                    break;
                case CourseSearchSortBy.Distance:
                    AssertTagInnerTextValue(htmlDom, viewModel.DistanceOrderByText, "span");
                    break;
                default:
                case CourseSearchSortBy.StartDate:
                    AssertTagInnerTextValue(htmlDom, viewModel.StartDateOrderByText, "span");
                    break;
            }
        }

        [Theory]
        [MemberData(nameof(Dfc7055SearchResultsViewTestsInput))]
        public void Dfc7055CourseDetailsViewTests(int coursesCount, string pageTitle, string noCoursesFoundText)
        {
            // Assign
            var searchResultsView = new _MVC_Views_TrainingCourses_SearchResults_cshtml();
            var viewModel = new TrainingCourseResultsViewModel
            {
                PageTitle = pageTitle,
                Courses = GetCourses(coursesCount, true).ToList(),
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
    }
}
