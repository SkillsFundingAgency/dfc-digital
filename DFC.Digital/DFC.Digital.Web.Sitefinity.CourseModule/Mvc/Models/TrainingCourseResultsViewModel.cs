using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class TrainingCourseResultsViewModel
    {
        public IList<CourseListingViewModel> Courses { get; set; } = new List<CourseListingViewModel>();

        public CourseSearchOrderBy CourseSearchSortBy { get; set; }

        public int ResultsCount { get; set; }

        public int CurrentPageNumber { get; set; }

        public int TotalPagesCount { get; set; }

        public int RecordsPerPage { get; set; }

        public string NextPageLabel { get; set; }

        public string PreviousPageLabel { get; set; }

        public string RecordsOnPageLabel => $"{((CurrentPageNumber - 1) * RecordsPerPage) + 1} - {((CurrentPageNumber - 1) * RecordsPerPage) + Courses.Count}";

        public string SearchTerm { get; set; }

        public Uri ResetFilterUrl { get; set; }

        public PaginationViewModel PaginationViewModel { get; set; } = new PaginationViewModel();

        public CourseFiltersModel CourseFiltersModel { get; set; } = new CourseFiltersModel();

        public OrderByLinks OrderByLinks { get; set; } = new OrderByLinks();

        public IDictionary<string, string> ActiveFilterOptions { get; set; } = new ConcurrentDictionary<string, string>();

        public string PageTitle { get; set; }

        public string FilterCourseByText { get; set; }

        public string NoTrainingCoursesFoundText { get; set; }
    }
}