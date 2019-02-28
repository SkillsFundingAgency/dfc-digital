using System;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class TrainingCourseResultsViewModel
    {
        public IList<Course> Courses { get; set; } = new List<Course>();
        public CourseSearchSortBy CourseSearchSortBy { get; set; }

        public int ResultsCount { get; set; }

        public int CurrentPageNumber { get; set; }

        public int TotalPagesCount { get; set; }

        public int RecordsPerPage { get; set; }

        public string NextPageLabel { get; set; }

        public string PreviousPageLabel { get; set; }

        public string RecordsOnPageLabel => $"{((CurrentPageNumber - 1) * RecordsPerPage) + 1} - {((CurrentPageNumber - 1) * RecordsPerPage) + Courses.Count()}";

        public string SearchTerm { get; set; }

        public Uri ResetFilterUrl { get; set; }

        public PaginationViewModel PaginationViewModel { get; set; } = new PaginationViewModel();

        public CourseFiltersModel CourseFiltersModel { get; set; } = new CourseFiltersModel();

        public OrderByLinks OrderByLinks { get; set; } = new OrderByLinks();
        public string ActiveFilterOptions { get; set; }
    }
}