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
        private readonly IWebAppContext fakeWebAppContext;
        private readonly IMapper mapperCfg;

        public CourseSearchResultsControllerTests()
        {
            asyncHelper = new AsyncHelper();
            fakeCourseSearchViewModelService = A.Fake<ICourseSearchResultsViewModelBullder>(ops => ops.Strict());
            fakeWebAppContext = A.Fake<IWebAppContext>();
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
        public void IndexTests(CourseFiltersViewModel searchFilter, string resetFilterText, string pageTitle, string courseSearchResultsPage, string courseDetailsPage, CourseSearchOrderBy orderBy, CourseSearchResult courseSearchResponse)
        {
            courseSearchResponse = courseSearchResponse ?? new CourseSearchResult();
            searchFilter = searchFilter ?? new CourseFiltersViewModel();

            // setupFakes
            A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<CourseSearchProperties>._)).Returns(courseSearchResponse);

            var searchProperties = new CourseSearchProperties
            {
                OrderedBy = orderBy
            };

            // Assign
            var controller = new CourseSearchResultsController(fakeApplicationLogger, fakeCourseSearchService, asyncHelper, fakeCourseSearchViewModelService, fakeWebAppContext, mapperCfg)
            {
                ResetFilterText = resetFilterText,
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
                        vm.ResetFiltersText.Should().BeEquivalentTo(controller.ResetFilterText);
                        vm.ResetFilterUrl.OriginalString.Should().NotBeEmpty();
                    });

            if (!string.IsNullOrWhiteSpace(searchFilter.SearchTerm))
            {
                A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<CourseSearchProperties>._)).MustHaveHappened();
                if (courseSearchResponse.Courses.Any())
                {
                    A.CallTo(() => fakeCourseSearchViewModelService.GetOrderByLinks(A<string>._, A<CourseSearchOrderBy>._)).MustHaveHappened();
                    A.CallTo(() => fakeCourseSearchViewModelService.SetupViewModelPaging(A<CourseSearchResultsViewModel>._, A<CourseSearchResult>._, A<string>._, A<int>._)).MustHaveHappened();
                }
            }
            else
            {
                A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<CourseSearchProperties>._)).MustNotHaveHappened();
                A.CallTo(() => fakeCourseSearchViewModelService.GetOrderByLinks(A<string>._, A<CourseSearchOrderBy>._)).MustNotHaveHappened();
                A.CallTo(() => fakeCourseSearchViewModelService.SetupViewModelPaging(A<CourseSearchResultsViewModel>._, A<CourseSearchResult>._, A<string>._, A<int>._)).MustNotHaveHappened();
            }
        }

        [Fact]
        public void OrderByDistanceWithNoPostcodeTest()
        {
            var courseSearchResponse = new CourseSearchResult
            {
                Courses = GetCourses(2),
                ResultProperties = new CourseSearchResultProperties
                {
                    Page = 1,
                    TotalPages = 1,
                    TotalResultCount = 2
                }
            };

            var searchFilter = new CourseFiltersViewModel() { Postcode = null, SearchTerm = "AnySearchTerm" };

            // setupFakes
            A.CallTo(() => fakeCourseSearchService.SearchCoursesAsync(A<CourseSearchProperties>._)).Returns(courseSearchResponse);

            var searchProperties = new CourseSearchProperties
            {
                OrderedBy = CourseSearchOrderBy.Distance
            };

            // Assign
            var controller = new CourseSearchResultsController(fakeApplicationLogger, fakeCourseSearchService, asyncHelper, fakeCourseSearchViewModelService, fakeWebAppContext, mapperCfg)
            {
            };

            // Act
            var controllerResult = controller.WithCallTo(contrl => contrl.Index(searchFilter, searchProperties));

            // Assert
            controllerResult.ShouldRenderView("SearchResults").WithModel<CourseSearchResultsViewModel>();
            A.CallTo(() => fakeCourseSearchViewModelService.GetOrderByLinks(A<string>._, CourseSearchOrderBy.Distance)).MustHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeCourseSearchViewModelService.GetOrderByLinks(A<string>._, A<CourseSearchOrderBy>._)).Returns(new OrderByLinks());
            A.CallTo(() => fakeCourseSearchViewModelService.SetupViewModelPaging(A<CourseSearchResultsViewModel>._, A<CourseSearchResult>._, A<string>._, A<int>._)).DoesNothing();
        }
    }
}
