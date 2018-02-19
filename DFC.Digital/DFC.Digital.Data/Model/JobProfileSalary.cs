using DFC.Digital.Data.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class JobProfileSalary : IDigitalDataModel
    {
        [JsonProperty("median")]
        public double Median { get; set; }

        [JsonProperty("deciles")]
        public IDictionary<int, decimal> Deciles { get; set; }
    }
}
