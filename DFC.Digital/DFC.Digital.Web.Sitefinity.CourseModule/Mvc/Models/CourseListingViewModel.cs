﻿using Course = DFC.FindACourseClient.Models.ExternalInterfaceModels.Course;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseListingViewModel
    {
        public Course Course { get; set; }

        public string CourseLink { get; set; }

        public string LocationLabel { get; set; }

        public string ProviderLabel { get; set; }

        public string AdvancedLoanProviderLabel { get; set; }

        public string StartDateLabel { get; set; }
    }
}