using Newtonsoft.Json;

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

        public string PSFCategory { get; set; }
    }
}