using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class RestrictionsOtherRequirements
    {
        public string OtherRequirements { get; set; }

        public IEnumerable<Restriction> Restrictions { get; set; } = Enumerable.Empty<Restriction>();

        public string SectionTitle { get; set; }

        public string SectionIntro { get; set; }
    }
}