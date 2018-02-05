using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}