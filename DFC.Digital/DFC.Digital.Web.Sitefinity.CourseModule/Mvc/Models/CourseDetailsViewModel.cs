using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseDetailsViewModel
    {
        public string FindACoursePage { get; set; }

        public string QualificationDetailsLabel { get; set; }

        public string CourseDescriptionLabel { get; set; }

        public string NoCourseDescriptionMessage { get; set; }

        public string EntryRequirementsLabel { get; set; }

        public string NoEntryRequirementsAvailableMessage { get; set; }

        public string EquipmentRequiredLabel { get; set; }

        public string NoEquipmentRequiredMessage { get; set; }

        public string AssessmentMethodLabel { get; set; }

        public string NoAssessmentMethodAvailableMessage { get; set; }

        public string VenueLabel { get; set; }

        public string OtherDatesAndVenuesLabel { get; set; }

        public string NoOtherDateOrVenueAvailableMessage { get; set; }

        public string ReferralPath { get; set; }

        public string ProviderLabel { get; set; }

        public string EmployerSatisfactionLabel { get; set; }

        public string LearnerSatisfactionLabel { get; set; }

        public string ProviderPerformanceLabel { get; set; }

        public string CourseDetailsPage { get; set; }

        public CourseDetails CourseDetails { get; set; } = new CourseDetails();

        public string ContactAdviserSection { get; set; }
    }
}