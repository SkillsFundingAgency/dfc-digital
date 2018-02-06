using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Data.Model
{
    public class JobProfileSectionFilter
    {
        public string SectionCaption { get; set; }

        public string TitleMember { get; set; }

        public string ContentFieldMember { get; set; }

        public IEnumerable<string> SubFilters { get; set; } = Enumerable.Empty<string>();
    }
}