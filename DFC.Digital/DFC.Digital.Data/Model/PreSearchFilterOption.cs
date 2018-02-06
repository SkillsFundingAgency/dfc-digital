using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class PreSearchFilterOption
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
