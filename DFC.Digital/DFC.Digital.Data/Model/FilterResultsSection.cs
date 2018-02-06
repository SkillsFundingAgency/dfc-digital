using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class FilterResultsSection
    {
        public string Name { get; set; }

        public List<FilterResultsOption> Options { get; set; }

        public bool SingleSelectOnly { get; set; }

        public string SingleSelectedValue { get; set; }

        public string SectionDataType { get; set; }

        public string SectionDataTypes => $"{SectionDataType}s";
    }
}