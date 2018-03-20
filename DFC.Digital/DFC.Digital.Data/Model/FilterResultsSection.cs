using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    //TODO - to be refactored to use Presearchfiltersection class
    public class FilterResultsSection
    {
        public string Name { get; set; }

        public ICollection<FilterResultsOption> Options { get; set; }

        public bool SingleSelectOnly { get; set; }

        public string SingleSelectedValue { get; set; }

        public string SectionDataType { get; set; }

        public string SectionDataTypes => $"{SectionDataType}s";
    }
}