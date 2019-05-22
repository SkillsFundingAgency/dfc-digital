namespace DFC.Digital.Data.Model
{
    public class CourseSearchProperties
    {
        public int Page { get; set; } = 1;

        public int Count { get; set; }

        public CourseSearchOrderBy OrderBy { get; set; }

        public CourseSearchFilters Filters { get; set; } = new CourseSearchFilters();
    }
}
