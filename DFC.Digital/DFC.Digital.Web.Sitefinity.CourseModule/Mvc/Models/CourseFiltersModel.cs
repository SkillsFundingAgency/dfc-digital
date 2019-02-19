using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseFiltersModel
    {
        public string Distance { get; set; }

        public string AgeSuitability { get; set; }

        public IEnumerable<string> StudyMode { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> AttendancePattern { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> AttendanceMode { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> QualificationLevel { get; set; } = Enumerable.Empty<string>();

        public string Location { get; set; }

        public string ProviderKeyword { get; set; }
    }
}