using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class SearchProperties
    {
        public int Count { get; set; } = 10;

        public int Page { get; set; } = 1;

        public bool UseRawSearchTerm { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "No need for empty collection, this is optional field.")]
        public IList<string> SearchFields { get; set; }

        public IList<string> OrderByFields { get; set; }

        public string FilterBy { get; set; }

        public int ExactMatchCount { get; set; }
    }
}