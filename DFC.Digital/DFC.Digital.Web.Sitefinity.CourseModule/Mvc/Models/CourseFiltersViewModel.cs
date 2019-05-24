using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseFiltersViewModel : CourseSearchFilters
    {
        public IEnumerable<SelectItem> AgeSuitabilitySelectedList { get; set; } = Enumerable.Empty<SelectItem>();

        public IEnumerable<SelectItem> StudyModeSelectedList { get; set; } = Enumerable.Empty<SelectItem>();

        public IEnumerable<SelectItem> PatternSelectedList { get; set; } = Enumerable.Empty<SelectItem>();

        public IEnumerable<string> AttendanceMode { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<SelectItem> AttendanceSelectedList { get; set; } = Enumerable.Empty<SelectItem>();

        public IEnumerable<string> StartDate { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<SelectItem> StartDateSelectedList { get; set; } = Enumerable.Empty<SelectItem>();
    }
}