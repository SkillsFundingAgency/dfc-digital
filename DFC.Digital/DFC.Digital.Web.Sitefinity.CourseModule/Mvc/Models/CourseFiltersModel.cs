using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseFiltersModel
    {
        public string Distance { get; set; }

        public IEnumerable<SelectItem> DistanceSelectedList { get; set; } = Enumerable.Empty<SelectItem>();

        public string AgeSuitability { get; set; }

        public IEnumerable<SelectItem> AgeSuitabilitySelectedList { get; set; } = Enumerable.Empty<SelectItem>();

        public IEnumerable<string> StudyMode { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<SelectItem> StudyModeSelectedList { get; set; } = Enumerable.Empty<SelectItem>();

        public IEnumerable<string> AttendancePattern { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<SelectItem> PatternSelectedList { get; set; } = Enumerable.Empty<SelectItem>();

        public IEnumerable<string> AttendanceMode { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<SelectItem> AttendanceSelectedList { get; set; } = Enumerable.Empty<SelectItem>();

        public IEnumerable<string> QualificationLevel { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<SelectItem> QualificationSelectedList { get; set; } = Enumerable.Empty<SelectItem>();

        public IEnumerable<string> StartDate { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<SelectItem> StartDateSelectedList { get; set; } = Enumerable.Empty<SelectItem>();

        public string Location { get; set; }

        public string ProviderKeyword { get; set; }
    }
}