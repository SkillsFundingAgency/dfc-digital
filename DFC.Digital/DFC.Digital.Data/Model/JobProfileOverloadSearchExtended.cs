using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class JobProfileOverloadSearchExtended : JobProfile
    {
        public IEnumerable<string> JobProfileSpecialism { get; set; }

        public IEnumerable<string> HiddenAlternativeTitle { get; set; }

        public IEnumerable<string> UniversityEntryRequirements { get; set; }

        public IEnumerable<string> CollegeEntryRequirements { get; set; }

        public IEnumerable<string> ApprenticeshipEntryRequirements { get; set; }

        public IEnumerable<string> JobProfileCategories { get; set; }

        public IEnumerable<string> HowToBecomeDataRegistrations { get; set; }
    }
}
