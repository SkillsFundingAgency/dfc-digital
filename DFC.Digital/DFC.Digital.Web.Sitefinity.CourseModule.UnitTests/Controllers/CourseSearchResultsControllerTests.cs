using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers;
using FakeItEasy;
using FluentAssertions;
using System.Linq;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class CourseSearchResultsControllerTests : MemberDataHelper
    {
        private readonly ICourseSearchService fakeCourseSearchService;
        private readonly IAsyncHelper asyncHelper;
        private readonly ICourseSearchResultsViewModelBullder fakeCourseSearchViewModelService;
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly IQueryStringBuilder<CourseSearchFilters> fakeBuildQueryStringService;
        private readonly IMapper mapperCfg;

        public CourseSearchResultsControllerTests()
        {
            asyncHelper = new AsyncHelper();
            fakeCourseSearchViewModelService = A.Fake<ICourseSearchResultsViewModelBullder>(ops => ops.Strict());
            fakeBuildQueryStringService = A.Fake<IQueryStringBuilder<CourseSearchFilters>>(ops => ops.Strict());
            fakeCourseSearchService = A.Fake<ICourseSearchService>(ops => ops.Strict());
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            SetupCalls();
            mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CourseAutomapperProfile>();
            }).CreateMapper();
        }

        [Theory]
        [MemberData(nameof(IndexTestsInput))]
        public void IndexTests(CourseSearchFilters searchFilter, string filterCourseByText, string pageTitle, string courseSearchResultsPage, string courseDetailsPage, CourseSearchOrderBy orderBy, CourseSearchResult courseSearchResponse)
        {
            courseSearchResponse = courseSearchResponse ?? new CourseSearchResult();
            searchFilter = searchFilter ?? new CourseSearchFilters();

            // setupFakes
            A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<string>._, A<CourseSearchProperties>._)).Returns(courseSearchResponse);

            var searchProperties = new CourseSearchProperties
            {
                OrderedBy = orderBy
            };

            // Assign
            var controller = new CourseSearchResultsController(fakeApplicationLogger, fakeCourseSearchService, asyncHelper, fakeCourseSearchViewModelService, fakeBuildQueryStringService, mapperCfg)
            {
                FilterCourseByText = filterCourseByText,
                PageTitle = pageTitle,
                CourseSearchResultsPage = courseSearchResultsPage,
                CourseDetailsPage = courseDetailsPage,
                RecordsPerPage = 40,
                AdvancedLoanProviderLabel = nameof(CourseSearchResultsController.AdvancedLoanProviderLabel),
                LocationLabel = nameof(CourseSearchResultsController.LocationLabel),
                ProviderLabel = nameof(CourseSearchResultsController.ProviderLabel),
                StartDateLabel = nameof(CourseSearchResultsController.StartDateLabel),
                OrderByText = nameof(CourseSearchResultsController.OrderByText),
                StartDateOrderByText = nameof(CourseSearchResultsController.StartDateOrderByText),
                DistanceOrderByText = nameof(CourseSearchResultsController.DistanceOrderByText),
                RelevanceOrderByText = nameof(CourseSearchResultsController.RelevanceOrderByText),
                NoTrainingCoursesFoundText = nameof(CourseSearchResultsController.NoTrainingCoursesFoundText),
                ApplyFiltersText = nameof(CourseSearchResultsController.ApplyFiltersText),
                CourseTypeSectionText = nameof(CourseSearchResultsController.CourseTypeSectionText),
                CourseHoursSectionText = nameof(CourseSearchResultsController.CourseHoursSectionText),
                StartDateSectionText = nameof(CourseSearchResultsController.StartDateSectionText),
                SearchForCourseNameText = nameof(CourseSearchResultsController.SearchForCourseNameText),
                WithinText = nameof(CourseSearchResultsController.WithinText),
                ResetFilterText = nameof(CourseSearchResultsController.ResetFilterText),
                Only1619CoursesText = nameof(CourseSearchResultsController.Only1619CoursesText),
                StartDateExampleText = nameof(CourseSearchResultsController.StartDateExampleText)
            };

            // Act
            var controllerResult = controller.WithCallTo(contrl =>
                contrl.Index(
                    searchFilter,
                    searchProperties));

            // Assert
            controllerResult.ShouldRenderView("SearchResults").WithModel<CourseSearchResultsViewModel>(
                    vm =>
                    {
                        vm.PageTitle.Should().BeEquivalentTo(controller.PageTitle);
                        vm.FilterCourseByText.Should().BeEquivalentTo(controller.FilterCourseByText);
                    });

            if (!string.IsNullOrWhiteSpace(searchFilter.SearchTerm))
            {
                A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<string>._, A<CourseSearchProperties>._)).MustHaveHappened();
                if (courseSearchResponse.Courses.Any())
                {
                    A.CallTo(() => fakeCourseSearchViewModelService.GetOrderByLinks(A<string>._, A<CourseSearchOrderBy>._)).MustHaveHappened();
                    A.CallTo(() => fakeCourseSearchViewModelService.SetupViewModelPaging(A<CourseSearchResultsViewModel>._, A<CourseSearchResult>._, A<string>._, A<int>._)).MustHaveHappened();
                }
               }
            else
            {
                A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<string>._, A<CourseSearchProperties>._)).MustNotHaveHappened();
                A.CallTo(() => fakeCourseSearchViewModelService.GetOrderByLinks(A<string>._, A<CourseSearchOrderBy>._)).MustNotHaveHappened();
                A.CallTo(() => fakeCourseSearchViewModelService.SetupViewModelPaging(A<CourseSearchResultsViewModel>._, A<CourseSearchResult>._, A<string>._, A<int>._)).MustNotHaveHappened();
            }
        }

        [Theory]
        [MemberData(nameof(IndexPostModelTestsInput))]
        public void IndexPostTests(string filterCourseByText, string pageTitle, string courseSearchResultsPage, string courseDetailsPage, CourseFiltersViewModel viewModel)
        {
            // Assign
            var controller = new CourseSearchResultsController(fakeApplicationLogger, fakeCourseSearchService, asyncHelper, fakeCourseSearchViewModelService, fakeBuildQueryStringService, mapperCfg)
            {
                FilterCourseByText = filterCourseByText,
                PageTitle = pageTitle,
                CourseSearchResultsPage = courseSearchResultsPage,
                CourseDetailsPage = courseDetailsPage
            };

            // Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(viewModel));

            // Assert
            controllerResult.ShouldRedirectTo(
                fakeBuildQueryStringService.BuildPathAndQueryString(controller.CourseSearchResultsPage, viewModel));
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeCourseSearchViewModelService.GetOrderByLinks(A<string>._, A<CourseSearchOrderBy>._)).Returns(new OrderByLinks());
            A.CallTo(() => fakeCourseSearchViewModelService.SetupViewModelPaging(A<CourseSearchResultsViewModel>._, A<CourseSearchResult>._, A<string>._, A<int>._)).DoesNothing();
            A.CallTo(() => fakeBuildQueryStringService.BuildPathAndQueryString(A<string>._, A<CourseSearchFilters>._)).Returns(nameof(IQueryStringBuilder<CourseSearchFilters>.BuildPathAndQueryString));
        }
    }
}
