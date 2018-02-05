using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class PsfOption
    {
        [JsonIgnore]
        public string Id { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        [JsonIgnore]
        public string Description { get; set; }

        public bool IsSelected { get; set; }

        public bool ClearOtherOptionsIfSelected { get; set; }

        public string OptionKey { get; set; }
    }
}