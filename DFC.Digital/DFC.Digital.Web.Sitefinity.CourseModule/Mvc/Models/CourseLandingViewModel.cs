using DFC.Digital.Data.Model;
using System.Text.RegularExpressions;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseLandingViewModel : CourseSearchFilters
    {
        public string CourseNameHintText { get; set; }

        public string CourseNameLabel { get; set; }

        public string ProviderLabel { get; set; }

        public string ProviderNameHintText { get; set; }

        public string LocationLabel { get; set; }

        public string LocationHintText { get; set; }

        public string SubmitButtonText { get; set; }
    }
}