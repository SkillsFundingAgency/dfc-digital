using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
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
        private readonly ICourseSearchConverter fakeCourseSearchConverter;
        private readonly IApplicationLogger fakeApplicationLogger;

        public TrainingCoursesControllerTests()
        {
            asyncHelper = new AsyncHelper();
            fakeCourseSearchConverter = A.Fake<ICourseSearchConverter>(ops => ops.Strict());
            fakeCourseSearchService = A.Fake<ICourseSearchService>(ops => ops.Strict());
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            SetupCalls();
        }

        [Theory]
        [MemberData(nameof(IndexTestsInput))]
        public void IndexTests(string searchTerm, string filterCourseByText, string pageTitle, string courseSearchResultsPage, string courseDetailsPage, CourseSearchResponse courseSearchResponse)
        {
            // setupFakes
            A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<CourseSearchRequest>._)).Returns(courseSearchResponse);

            // Assign
            var controller = new TrainingCoursesController(fakeApplicationLogger, fakeCourseSearchService, asyncHelper, fakeCourseSearchConverter)
            {
                FilterCourseByText = filterCourseByText,
                PageTitle = pageTitle,
                CourseSearchResultsPage = courseSearchResultsPage,
                CourseDetailsPage = courseDetailsPage,
                LocationRegex = nameof(TrainingCoursesController.LocationRegex),
                AttendanceModesSource = nameof(TrainingCoursesController.AttendanceModesSource),
                AttendancePatternModesSource = nameof(TrainingCoursesController.AttendanceModesSource),
                AgeSuitabilitySource = nameof(TrainingCoursesController.AgeSuitabilitySource),
                DistanceSource = nameof(TrainingCoursesController.DistanceSource),
                StartDateSource = nameof(TrainingCoursesController.StartDateSource),
                QualificationLevelSource = nameof(TrainingCoursesController.QualificationLevelSource),
                StudyModesSource = nameof(TrainingCoursesController.StudyModesSource),
                RecordsPerPage = 40
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
                string.Empty,
                string.Empty,
                1));

            // Assert
            controllerResult.ShouldRenderView("SearchResults").WithModel<TrainingCourseResultsViewModel>(
                    vm =>
                    {
                        vm.PageTitle.Should().BeEquivalentTo(controller.PageTitle);
                        vm.FilterCourseByText.Should().BeEquivalentTo(controller.FilterCourseByText);
                    });

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<CourseSearchRequest>._)).MustHaveHappened();
                if (courseSearchResponse.Courses.Any())
                {
                    A.CallTo(() => fakeCourseSearchConverter.GetOrderByLinks(A<string>._, A<CourseSearchSortBy>._)).MustHaveHappened();
                    A.CallTo(() => fakeCourseSearchConverter.SetupPaging(A<TrainingCourseResultsViewModel>._, A<CourseSearchResponse>._, A<string>._, A<int>._, A<string>._)).MustHaveHappened();
                }

                A.CallTo(() => fakeCourseSearchConverter.GetActiveFilterOptions(A<CourseFiltersModel>._)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<CourseSearchRequest>._)).MustNotHaveHappened();
                A.CallTo(() => fakeCourseSearchConverter.GetOrderByLinks(A<string>._, A<CourseSearchSortBy>._)).MustNotHaveHappened();
                A.CallTo(() => fakeCourseSearchConverter.GetActiveFilterOptions(A<CourseFiltersModel>._)).MustNotHaveHappened();
                A.CallTo(() => fakeCourseSearchConverter.SetupPaging(A<TrainingCourseResultsViewModel>._, A<CourseSearchResponse>._, A<string>._, A<int>._, A<string>._)).MustNotHaveHappened();
            }
        }

        [Theory]
        [MemberData(nameof(IndexPostTestsInput))]
        public void IndexPostTests(string filterCourseByText, string pageTitle, string courseSearchResultsPage, string courseDetailsPage, TrainingCourseResultsViewModel viewModel)
        {
            // Assign
            var controller = new TrainingCoursesController(fakeApplicationLogger, fakeCourseSearchService, asyncHelper, fakeCourseSearchConverter)
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
                    fakeCourseSearchConverter.BuildRedirectPathAndQueryString(controller.CourseSearchResultsPage, viewModel, controller.LocationRegex));
            }
            else
            {
                controllerResult.ShouldRenderView("SearchResults").WithModel<TrainingCourseResultsViewModel>(
                    vm =>
                    {
                        vm.PageTitle.Should().BeEquivalentTo(controller.PageTitle);
                        vm.FilterCourseByText.Should().BeEquivalentTo(controller.FilterCourseByText);
                    });
            }
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeCourseSearchConverter.GetOrderByLinks(A<string>._, A<CourseSearchSortBy>._)).Returns(new OrderByLinks());
            A.CallTo(() => fakeCourseSearchConverter.GetActiveFilterOptions(A<CourseFiltersModel>._)).Returns(new Dictionary<string, string>());
            A.CallTo(() => fakeCourseSearchConverter.GetFilterSelectItems(A<string>._, A<IEnumerable<string>>._, A<string>._)).Returns(new List<SelectItem>());

            A.CallTo(() => fakeCourseSearchConverter.SetupPaging(A<TrainingCourseResultsViewModel>._, A<CourseSearchResponse>._, A<string>._, A<int>._, A<string>._)).DoesNothing();
            A.CallTo(() => fakeCourseSearchConverter.BuildRedirectPathAndQueryString(A<string>._, A<TrainingCourseResultsViewModel>._, A<string>._)).Returns(nameof(ICourseSearchConverter.BuildRedirectPathAndQueryString));
            A.CallTo(() => fakeCourseSearchConverter.GetActiveFilterOptions(A<CourseFiltersModel>._)).Returns(new Dictionary<string, string>());
            A.CallTo(() => fakeCourseSearchConverter.GetCourseSearchRequest(A<string>._, A<int>._, A<string>._, A<string>._, A<string>._, A<string>._, A<string>._, A<string>._, A<string>._, A<string>._, A<string>._, A<int>._)).Returns(new CourseSearchRequest());
        }
    }
}
