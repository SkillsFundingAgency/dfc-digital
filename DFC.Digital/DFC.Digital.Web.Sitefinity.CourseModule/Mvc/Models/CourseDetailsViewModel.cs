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

        public string NoWhatYoullLearnAvailableMessage { get; set; }

        public string NoHowYoullLearnAvailableMessage { get; set; }

        public string NoNextStepsAvailableMessage { get; set; }

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

        public string AttendancePatternLabel { get; set; }

        public string PriceLabel { get; set; }

        public string StartDateLabel { get; set; }

        public string QualificationLevelLabel { get; set; }

        public string QualificationNameLabel { get; set; }

        public string AwardingOrganisationLabel { get; set; }

        public string SubjectCategoryLabel { get; set; }

        public string CourseWebpageLinkLabel { get; set; }

        public string CourseTypeLabel { get; set; }

        public string AdditionalPriceLabel { get; set; }

        public string FundingInformationLabel { get; set; }

        public string SupportingFacilitiesLabel { get; set; }

        public string FundingInformationText { get; set; }

        public string FundingInformationLink { get; set; }

        public string LanguageOfInstructionLabel { get; set; }

        public string CourseDetailsLabel { get; set; }

        public string WhoThisCourseIsForLabel { get; set; }

        public string WhatYoullLearn { get; set; }

        public string HowYoullLearn { get; set; }

        public string WhatYoullNeedToBring { get; set; }

        public string HowYoullBeAssessed { get; set; }

        public string NextSteps { get; set; }

        public string LocationLabel { get; set; }

        public string CourseHoursLabel { get; set; }
    }
}