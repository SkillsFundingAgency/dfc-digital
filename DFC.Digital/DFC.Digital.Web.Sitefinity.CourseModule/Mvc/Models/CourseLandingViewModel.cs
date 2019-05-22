using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models
{
    public class CourseLandingViewModel : CourseSearchFilters
    {
        public string SearchTerm { get; set; }

        public string CourseNameHintText { get; set; }

        public string CourseNameLabel { get; set; }

        public string ProviderLabel { get; set; }

        public string ProviderNameHintText { get; set; }

        public string LocationLabel { get; set; }

        public string LocationHintText { get; set; }

        public string Dfe1619FundedText { get; set; }
    }
}