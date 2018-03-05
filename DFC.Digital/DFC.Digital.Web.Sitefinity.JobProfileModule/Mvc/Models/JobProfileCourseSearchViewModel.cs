using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class JobProfileCourseSearchViewModel
    {
        public string CoursesSectionTitle { get; set; }

        public string NoTrainingCoursesText { get; set; }

        public string TrainingCoursesText { get; set; }

        public string CoursesLocationDetails { get; set; }

        public string CourseLink { get; set; }

        public IEnumerable<Course> Courses { get; set; }

        public bool CoursesAvailable => Courses != null && Courses.Any();

        public string MainSectionTitle { get; set; }
    }
}