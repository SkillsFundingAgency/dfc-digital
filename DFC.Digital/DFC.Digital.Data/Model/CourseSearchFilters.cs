using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class CourseSearchFilters
    {
        public string ProviderKeyword { get; set; }

        public string Location { get; set; }

        public string Attendance { get; set; }

        public string StudyMode { get; set; }

        public float Distance { get; set; }

        public bool DistanceSpecified { get; set; }

        public string AttendancePattern { get; set; }

        public bool Only1619Courses { get; set; }
    }
}
