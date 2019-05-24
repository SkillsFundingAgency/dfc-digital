namespace DFC.Digital.Data.Model
{
    public class CourseSearchProperties
    {
        public int Page { get; set; } = 1;

        public int Count { get; set; } = 20;

        public CourseSearchOrderBy OrderBy { get; set; } = CourseSearchOrderBy.Relevance;

        public CourseSearchFilters Filters { get; set; } = new CourseSearchFilters();
    }
}
