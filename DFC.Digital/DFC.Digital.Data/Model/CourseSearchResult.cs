using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class CourseSearchResult
    {
        public int TotalResultCount { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        public int CurrentPage { get; set; }

        public CourseSearchOrderBy CourseSearchSortBy { get; set; }
    }
}
