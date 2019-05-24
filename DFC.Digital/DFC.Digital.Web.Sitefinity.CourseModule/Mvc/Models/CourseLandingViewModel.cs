using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models
{
    public class CourseLandingViewModel
    {
        [AllowHtml]
        public string CourseName { get; set; }

        public string StrippedCourseName { get; set; }

        [AllowHtml]
        public string Location { get; set; }

        public string StrippedLocation { get; set; }

        public string QualificationLevel { get; set; }

        public string Distance { get; set; }

        [AllowHtml]
        public string ProviderKeyword { get; set; }

        public string StrippedProviderKeyword { get; set; }

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