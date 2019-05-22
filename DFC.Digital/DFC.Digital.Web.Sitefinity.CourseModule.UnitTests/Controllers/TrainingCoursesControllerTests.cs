using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class TrainingCoursesControllerTests : MemberDataHelper
    {
        private readonly ICourseSearchService fakeCourseSearchService;
        private readonly IAsyncHelper asyncHelper;
        private readonly ICourseSearchViewModelService fakeCourseSearchViewModelService;
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IBuildQueryStringService fakeBuildQueryStringService;

        public TrainingCoursesControllerTests()
        {
            asyncHelper = new AsyncHelper();
            fakeCourseSearchViewModelService = A.Fake<ICourseSearchViewModelService>(ops => ops.Strict());
            fakeBuildQueryStringService = A.Fake<IBuildQueryStringService>(ops => ops.Strict());
            fakeCourseSearchService = A.Fake<ICourseSearchService>(ops => ops.Strict());
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            SetupCalls();
        }

        [Theory]
        [MemberData(nameof(IndexTestsInput))]
        public void IndexTests(string searchTerm, string filterCourseByText, string pageTitle, string courseSearchResultsPage, string courseDetailsPage, CourseSearchResult courseSearchResponse)
        {
            // setupFakes
            A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<string>._, A<CourseSearchProperties>._)).Returns(courseSearchResponse);

            // Assign
            var controller = new CourseSearchResultsController(fakeApplicationLogger, fakeCourseSearchService, asyncHelper, fakeCourseSearchViewModelService, fakeBuildQueryStringService)
            {
                FilterCourseByText = filterCourseByText,
                PageTitle = pageTitle,
                CourseSearchResultsPage = courseSearchResultsPage,
                CourseDetailsPage = courseDetailsPage,
                LocationRegex = nameof(CourseSearchResultsController.LocationRegex),
                RecordsPerPage = 40,
                AdvancedLoanProviderLabel = nameof(CourseSearchResultsController.AdvancedLoanProviderLabel),
                LocationLabel = nameof(CourseSearchResultsController.LocationLabel),
                ProviderLabel = nameof(CourseSearchResultsController.ProviderLabel),
                StartDateLabel = nameof(CourseSearchResultsController.StartDateLabel),
                OrderByText = nameof(CourseSearchResultsController.OrderByText),
                StartDateOrderByText = nameof(CourseSearchResultsController.StartDateOrderByText),
                DistanceOrderByText = nameof(CourseSearchResultsController.DistanceOrderByText),
                RelevanceOrderByText = nameof(CourseSearchResultsController.RelevanceOrderByText),
                NoTrainingCoursesFoundText = nameof(CourseSearchResultsController.NoTrainingCoursesFoundText)
            };

            // Act
            var controllerResult = controller.WithCallTo(contrl =>
                contrl.Index(
                    searchTerm,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                false,
                1));

            // Assert
            controllerResult.ShouldRenderView("SearchResults").WithModel<CourseSearchResultsViewModel>(
                    vm =>
                    {
                        vm.PageTitle.Should().BeEquivalentTo(controller.PageTitle);
                        vm.FilterCourseByText.Should().BeEquivalentTo(controller.FilterCourseByText);
                    });

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<string>._, A<CourseSearchProperties>._)).MustHaveHappened();
                if (courseSearchResponse.Courses.Any())
                {
                    A.CallTo(() => fakeCourseSearchViewModelService.GetOrderByLinks(A<string>._, A<CourseSearchOrderBy>._)).MustHaveHappened();
                    A.CallTo(() => fakeCourseSearchViewModelService.SetupPaging(A<CourseSearchResultsViewModel>._, A<CourseSearchResult>._, A<string>._, A<int>._, A<string>._)).MustHaveHappened();
                }
               }
            else
            {
                A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<string>._, A<CourseSearchProperties>._)).MustNotHaveHappened();
                A.CallTo(() => fakeCourseSearchViewModelService.GetOrderByLinks(A<string>._, A<CourseSearchOrderBy>._)).MustNotHaveHappened();
                A.CallTo(() => fakeCourseSearchViewModelService.SetupPaging(A<CourseSearchResultsViewModel>._, A<CourseSearchResult>._, A<string>._, A<int>._, A<string>._)).MustNotHaveHappened();
            }
        }

        [Theory]
        [MemberData(nameof(IndexPostTestsInput))]
        public void IndexPostTests(string filterCourseByText, string pageTitle, string courseSearchResultsPage, string courseDetailsPage, CourseSearchResultsViewModel viewModel)
        {
            // Assign
            var controller = new CourseSearchResultsController(fakeApplicationLogger, fakeCourseSearchService, asyncHelper, fakeCourseSearchViewModelService, fakeBuildQueryStringService)
            {
                FilterCourseByText = filterCourseByText,
                PageTitle = pageTitle,
                CourseSearchResultsPage = courseSearchResultsPage,
                CourseDetailsPage = courseDetailsPage
            };

            // Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(viewModel));

            // Assert
            if (!string.IsNullOrWhiteSpace(viewModel.SearchTerm))
            {
                controllerResult.ShouldRedirectTo(
                    fakeBuildQueryStringService.BuildRedirectPathAndQueryString(controller.CourseSearchResultsPage, viewModel.SearchTerm, viewModel.CourseFiltersModel));
            }
            else
            {
                controllerResult.ShouldRenderView("SearchResults").WithModel<CourseSearchResultsViewModel>(
                    vm =>
                    {
                        vm.PageTitle.Should().BeEquivalentTo(controller.PageTitle);
                        vm.FilterCourseByText.Should().BeEquivalentTo(controller.FilterCourseByText);
                    });
            }
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeCourseSearchViewModelService.GetOrderByLinks(A<string>._, A<CourseSearchOrderBy>._)).Returns(new OrderByLinks());
            A.CallTo(() => fakeCourseSearchViewModelService.GetActiveFilterOptions(A<CourseFiltersViewModel>._)).Returns(new Dictionary<string, string>());
            A.CallTo(() => fakeCourseSearchViewModelService.GetFilterSelectItems(A<string>._, A<IEnumerable<string>>._, A<string>._)).Returns(new List<SelectItem>());
            A.CallTo(() => fakeCourseSearchViewModelService.SetupPaging(A<CourseSearchResultsViewModel>._, A<CourseSearchResult>._, A<string>._, A<int>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeBuildQueryStringService.BuildRedirectPathAndQueryString(A<string>._, A<string>._, A<CourseSearchFilters>._)).Returns(nameof(IBuildQueryStringService.BuildRedirectPathAndQueryString));
            A.CallTo(() => fakeCourseSearchViewModelService.GetActiveFilterOptions(A<CourseFiltersViewModel>._)).Returns(new Dictionary<string, string>());
        }
    }
}
