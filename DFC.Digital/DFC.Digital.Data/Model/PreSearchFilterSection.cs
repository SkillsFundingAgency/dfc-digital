using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class PreSearchFilterSection
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PreSearchFilterType SectionDataType { get; set; }

        public List<PreSearchFilterOption> Options { get; set; }

        [JsonIgnore]
        public bool SingleSelectOnly { get; set; }

        public string SingleSelectedValue { get; set; }

        [JsonIgnore]
        public int PageNumber { get; set; }
    }
}