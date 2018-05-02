using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class PagedOdataResult<T>
        where T : class, new()
    {
        [JsonProperty("@odata.context")]
        public string MetaData { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string NextLink { get; set; }

        [JsonProperty("value")]
        public IEnumerable<T> Value { get; set; }

        public bool HasNextPage => !string.IsNullOrEmpty(NextLink);
    }
}
