namespace DFC.Digital.Data.Model
{
    public class CourseSearchProperties
    {
        public int PageNumber { get; set; }

        public int RecordsPerPage { get; set; }

        public CourseSearchOrderBy CourseSearchSortBy { get; set; }
    }
}
