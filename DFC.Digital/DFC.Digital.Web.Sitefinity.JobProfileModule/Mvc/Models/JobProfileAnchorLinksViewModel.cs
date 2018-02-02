using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class JobProfileAnchorLinksViewModel
    {
        public IEnumerable<AnchorLink> AnchorLinks { get; set; } = Enumerable.Empty<AnchorLink>();
    }
}