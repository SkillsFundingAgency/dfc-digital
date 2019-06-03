using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class CourseDetails : Course
    {
        public VenueDetails VenueDetails { get; set; }

        public ProviderDetails ProviderDetails { get; set; }

        public string Description { get; set; }

        public string EntryRequirements { get; set; }

        public string QualificationName { get; set; }

        public string AssessmentMethod { get; set; }

        public string Cost { get; set; }

        public string EquipmentRequired { get; set; }

        public string BackToResultsUrl { get; set; }

        public List<OtherDatesAndVenues> OtherDatesAndVenues { get; set; }

        public string NoCourseDescriptionMessage { get; set; }

        public string NoEntryRequirementsAvailableMessage { get; set; }

        public string NoEquipmentRequiredMessage { get; set; }

        public string NoAssessmentMethodAvailableMessage { get; set; }

        public string NoVenueAvailableMessage { get; set; }

        public string NoOtherDateOrVenueAvailableMessage { get; set; }
    }
}
