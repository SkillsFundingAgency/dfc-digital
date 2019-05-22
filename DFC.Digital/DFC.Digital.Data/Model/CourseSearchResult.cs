using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class CourseSearchResult
    {
        public IEnumerable<Course> Courses { get; set; }

        public CourseSearchResultProperties ResultProperties { get; set; } = new CourseSearchResultProperties();
    }
}
