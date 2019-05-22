using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseSearchResultsViewModel : CourseSearchProperties
    {
        public string SearchTerm { get; set; }

        public string PageTitle { get; set; }

        public string FilterCourseByText { get; set; }

        public string NoTrainingCoursesFoundText { get; set; }

        public IList<CourseListingViewModel> Courses { get; set; } = new List<CourseListingViewModel>();

        public CourseSearchResultProperties ResultProperties { get; set; } = new CourseSearchResultProperties();

        public string RecordsOnPageLabel => $"{((Page - 1) * Count) + 1} - {((Page - 1) * Count) + Courses.Count}";

        public Uri ResetFilterUrl { get; set; }

        public PaginationViewModel PaginationViewModel { get; set; } = new PaginationViewModel();

        public CourseFiltersViewModel CourseFiltersModel { get; set; } = new CourseFiltersViewModel();

        public OrderByLinks OrderByLinks { get; set; } = new OrderByLinks();

        public IDictionary<string, string> ActiveFilterOptions { get; set; } = new ConcurrentDictionary<string, string>();
    }
}