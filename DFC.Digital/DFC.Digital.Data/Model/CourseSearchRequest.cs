namespace DFC.Digital.Data.Model
{
    public class CourseSearchRequest
    {
        public string SearchTerm { get; set; }

        public int PageNumber { get; set; }

        public int RecordsPerPage { get; set; }

        public string Dfe1619Funded { get; set; }

        public string QualificationLevel { get; set; }

        public string Attendance { get; set; }

        public string StudyMode { get; set; }

        public float Distance { get; set; }

        public bool DistanceSpecified { get; set; }

        public string AttendancePattern { get; set; }

        public string ProviderKeyword { get; set; }
    }
}
