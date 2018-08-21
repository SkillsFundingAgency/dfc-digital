using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class RouteEntry
    {
        public RouteEntryType RouteName { get; set; }

        public IEnumerable<EntryRequirement> EntryRequirements { get; set; }

        public IEnumerable<MoreInformationLink> MoreInformationLinks { get; set; }

        public TimedLinks MoreInformationCmLinks { get; set; }

        public TimedLinks MoreInformationDmLinks { get; set; }

        public string RouteSubjects { get; set; }

        public string FurtherRouteInformation { get; set; }

        public string RouteRequirement { get; set; }
    }
}