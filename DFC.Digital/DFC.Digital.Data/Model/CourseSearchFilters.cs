using System;
using System.Text.RegularExpressions;

namespace DFC.Digital.Data.Model
{
    public class CourseSearchFilters
    {
        public string SearchTerm { get; set; }

        public DateTime StartDateFrom { get; set; }

        public string Provider { get; set; }

        public string Postcode { get; set; }

        public float Distance { get; set; } = 10f;

        public string Town { get; set; }

        public bool DistanceSpecified { get; set; }

        public StartDate StartDate { get; set; } = StartDate.Anytime;

        public CourseHours CourseHours { get; set; } = CourseHours.All;

        public CourseType CourseType { get; set; } = CourseType.All;

        public bool IsValidStartDateFrom =>
            StartDate == StartDate.SelectDateFrom && !StartDateFrom.Equals(DateTime.MinValue);
    }
}
