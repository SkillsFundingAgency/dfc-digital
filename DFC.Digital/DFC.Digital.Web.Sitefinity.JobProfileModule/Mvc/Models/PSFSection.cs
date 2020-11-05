using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class PsfSection
    {
        public string Name { get; set; }

        public string SectionDataType { get; set; }

        [JsonIgnore]
        public string Description { get; set; }

        public List<PsfOption> Options { get; set; }

        [JsonIgnore]
        public bool SingleSelectOnly { get; set; }

        public string SingleSelectedValue { get; set; }

        [JsonIgnore]
        public string NextPageUrl { get; set; }

        [JsonIgnore]
        public string PreviousPageUrl { get; set; }

        [JsonIgnore]
        public int PageNumber { get; set; }

        [JsonIgnore]
        public int TotalNumberOfPages { get; set; }

        [JsonIgnore]
        public bool EnableAccordion { get; set; }

        [JsonIgnore]
        public string GroupFieldsBy { get; set; }

        [JsonIgnore]
        public string PSFCategory { get; set; }
    }
}