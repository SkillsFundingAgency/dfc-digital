namespace DFC.Digital.Data.Model
{
    public class CourseDetails : Course
    {
        public VenueDetails VenueDetails { get; set; }

        public ProviderDetails ProviderDetails { get; set; }

        public string Description { get; set; }

        public string EntryRequirements { get; set; }

        public string QualificationName { get; set; }
    }
}
