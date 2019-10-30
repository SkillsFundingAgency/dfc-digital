using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class SocCodeContentItem : RelatedContentItem
    {
        public string SOCCode { get; set; }

        public string Description { get; set; }

        public string ONetOccupationalCode { get; set; }

        public string UrlName { get; set; }

        public IEnumerable<Classification> ApprenticeshipFramework { get; set; }

        public IEnumerable<Classification> ApprenticeshipStandards { get; set; }
    }
}
