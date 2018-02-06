using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
