using DFC.Digital.Data.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
