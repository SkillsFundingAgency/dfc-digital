namespace DFC.Digital.Data.Model
{
    public class VenueDetails
    {
        public string VenueName { get; set; }

        public VenueAddress Location { get; set; } = new VenueAddress();

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Website { get; set; }

        public string Fax { get; set; }
    }
}