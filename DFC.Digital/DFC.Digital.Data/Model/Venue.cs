namespace DFC.Digital.Data.Model
{
    public class Venue
    {
        public string VenueName { get; set; }

        public Address Location { get; set; } = new Address();

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Website { get; set; }

        public string Fax { get; set; }

        public string Facilities { get; set; }
    }
}