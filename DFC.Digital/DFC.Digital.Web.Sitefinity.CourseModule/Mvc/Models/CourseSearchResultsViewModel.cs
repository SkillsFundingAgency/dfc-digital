using DFC.Digital.Data.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseSearchResultsViewModel : CourseSearchProperties
    {
        #region Page Text and labels
        public string PageTitle { get; set; }

        public string FilterCourseByText { get; set; }

        public string SearchForCourseNameText { get; set; }

        public string NoTrainingCoursesFoundText { get; set; }

        #endregion

        #region Filter Display
        public string ResetFiltersText { get; set; }

        public Uri ResetFilterUrl { get; set; }

        public CourseFiltersViewModel CourseFiltersModel { get; set; } = new CourseFiltersViewModel();

        #endregion

        #region Results Display

        public IList<CourseListingViewModel> Courses { get; set; } = new List<CourseListingViewModel>();

        public CourseSearchResultProperties ResultProperties { get; set; } = new CourseSearchResultProperties();

        public PaginationViewModel PaginationViewModel { get; set; } = new PaginationViewModel();

        public OrderByLinks OrderByLinks { get; set; } = new OrderByLinks();

        #endregion

    }
}