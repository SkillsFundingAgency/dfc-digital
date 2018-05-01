using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class ImportConfiguration
    {
        public IEnumerable<string> Content { get; set; }

        public Dictionary<string, string> Mappings { get; set; }

        public string Comment { get; set; }

        public bool ShouldBePublished { get; set; }

        public bool CanUpdate { get; set; }
    }
}