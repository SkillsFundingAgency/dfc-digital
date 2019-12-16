using DFC.Digital.Data.Model;

using FAC = DFC.FindACourseClient.Models.ExternalInterfaceModels;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseListingViewModel
    {
        public FAC.Course Course { get; set; }

        public string LocationLabel { get; set; }

        public string ProviderLabel { get; set; }

        public string AdvancedLoanProviderLabel { get; set; }

        public string StartDateLabel { get; set; }

        public string CourseLink { get; set; }
    }
}