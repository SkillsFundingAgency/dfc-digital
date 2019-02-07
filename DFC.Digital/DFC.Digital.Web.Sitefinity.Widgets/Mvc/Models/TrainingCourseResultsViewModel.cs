using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models
{
    public class TrainingCourseResultsViewModel
    {
        public IEnumerable<Course> Courses { get; set; } = Enumerable.Empty<Course>();
    }
}