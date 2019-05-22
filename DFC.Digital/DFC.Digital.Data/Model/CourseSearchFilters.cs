using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Data.Model
{
    public class CourseSearchFilters
    {
        public string ProviderKeyword { get; set; }

        public string Location { get; set; }

        public IEnumerable<string> Attendance { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> StudyMode { get; set; } = Enumerable.Empty<string>();

        public float Distance { get; set; }

        public bool DistanceSpecified { get; set; }

        public IEnumerable<string> AttendancePattern { get; set; } = Enumerable.Empty<string>();

        public bool Only1619Courses { get; set; }
    }
}
