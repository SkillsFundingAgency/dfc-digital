using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests.Helpers
{
    public class HelperSearchResultsData
    {
        private static readonly TrainingCourseResultsViewModel ValidCourseResultsViewModel = new TrainingCourseResultsViewModel
        {
            SearchTerm = nameof(TrainingCourseResultsViewModel.SearchTerm)
        };

        private static readonly TrainingCourseResultsViewModel InvalidCourseResultsViewModel = new TrainingCourseResultsViewModel
        {
            SearchTerm = string.Empty
        };

        private static readonly CourseSearchResponse ValidCourseSearchResponse = new CourseSearchResponse
        {
            Courses = new List<Course> { new Course { Title = nameof(Course.Title) } }
        };

        private static readonly CourseSearchResponse ZeroResultsCourseSearchResponse = new CourseSearchResponse
        {
            Courses = new List<Course>()
        };

        public static IEnumerable<object[]> IndexPostTestsInput()
        {
            yield return new object[]
            {
                nameof(TrainingCoursesController.FilterCourseByText),
                nameof(TrainingCoursesController.PageTitle),
                nameof(TrainingCoursesController.CourseSearchResultsPage),
                nameof(TrainingCoursesController.CourseDetailsPage),
                ValidCourseResultsViewModel
            };
            yield return new object[]
            {
                nameof(TrainingCoursesController.FilterCourseByText),
                nameof(TrainingCoursesController.PageTitle),
                nameof(TrainingCoursesController.CourseSearchResultsPage),
                nameof(TrainingCoursesController.CourseDetailsPage),
                ValidCourseResultsViewModel
            };
            yield return new object[]
            {
                nameof(TrainingCoursesController.FilterCourseByText),
                nameof(TrainingCoursesController.PageTitle),
                nameof(TrainingCoursesController.CourseSearchResultsPage),
                nameof(TrainingCoursesController.CourseDetailsPage),
                InvalidCourseResultsViewModel
            };
        }

        public static IEnumerable<object[]> IndexTestsInput()
        {
            yield return new object[]
            {
                nameof(TrainingCourseResultsViewModel.SearchTerm),
                nameof(TrainingCoursesController.FilterCourseByText),
                nameof(TrainingCoursesController.PageTitle),
                nameof(TrainingCoursesController.CourseSearchResultsPage),
                nameof(TrainingCoursesController.CourseDetailsPage),
                ValidCourseSearchResponse
            };

            yield return new object[]
            {
                nameof(TrainingCourseResultsViewModel.SearchTerm),
                nameof(TrainingCoursesController.FilterCourseByText),
                nameof(TrainingCoursesController.PageTitle),
                nameof(TrainingCoursesController.CourseSearchResultsPage),
                nameof(TrainingCoursesController.CourseDetailsPage),
                ZeroResultsCourseSearchResponse
            };

            yield return new object[]
            {
                null,
                nameof(TrainingCoursesController.FilterCourseByText),
                nameof(TrainingCoursesController.PageTitle),
                nameof(TrainingCoursesController.CourseSearchResultsPage),
                nameof(TrainingCoursesController.CourseDetailsPage),
                ZeroResultsCourseSearchResponse
            };
        }
    }
}
