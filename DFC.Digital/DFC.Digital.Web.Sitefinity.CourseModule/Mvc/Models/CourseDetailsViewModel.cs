using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseDetailsViewModel
    {
        public string FindACoursePage { get; set; }

        public CourseDetails CourseDetails { get; set; } = new CourseDetails();
    }
}