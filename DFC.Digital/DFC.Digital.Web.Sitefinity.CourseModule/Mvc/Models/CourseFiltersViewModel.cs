using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseFiltersViewModel : CourseSearchFilters
    {
        public string Only1619CoursesText { get; set; }

        public string AgeSuitabilityText { get; set; }

        public string StartDateExampleText { get; set; }

        public string StartDateDay { get; set; }

        public string StartDateMonth { get; set; }

        public string StartDateYear { get; set; }
    }
}