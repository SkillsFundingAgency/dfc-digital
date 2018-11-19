using Newtonsoft.Json;
using System.Linq;

namespace DFC.Digital.Data.Model
{
    public class SocCodeReport : CmsReportItem
    {
        public string SOCCode { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public IQueryable<TaxonReport> Frameworks { get; set; }

        [JsonIgnore]
        public IQueryable<TaxonReport> Standards { get; set; }
    }
}
