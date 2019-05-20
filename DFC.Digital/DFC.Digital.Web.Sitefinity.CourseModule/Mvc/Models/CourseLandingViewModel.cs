using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models
{
    public class CourseLandingViewModel
    {
        public string SearchTerm { get; set; }

        public string StrippedSearchTerm { get; set; }

        public string Location { get; set; }

        public string QualificationLevel { get; set; }

        public string Distance { get; set; }

        public string ProviderKeyword { get; set; }

        public string CourseNameHintText { get; set; }

        public string CourseNameLabel { get; set; }

        public string ProviderLabel { get; set; }

        public string ProviderNameHintText { get; set; }

        public string LocationLabel { get; set; }

        public string LocationHintText { get; set; }

        public string QualificationLevelHint { get; set; }

        public string QualificationLevelLabel { get; set; }

        public string Dfe1619FundedText { get; set; }

        public string Dfe1619Funded { get; set; }

        public string LocationRegex { get; set; }
    }
}