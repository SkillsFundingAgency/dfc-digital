using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class HowToBecome
    {
        public IEnumerable<RouteEntry> RouteEntries { get; set; }

        public MoreInformation FurtherInformation { get; set; }

        public ExtraInformation ExtraInformation { get; set; }

        public string IntroText { get; set; }

        public IEnumerable<Registration> Registrations { get; set; }

        public string OtherRequirements { get; set; }

        public IEnumerable<Restriction> Restrictions { get; set; }
    }
}
