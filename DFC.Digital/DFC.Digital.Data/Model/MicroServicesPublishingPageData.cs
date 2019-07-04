using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class MicroServicesPublishingPageData
    {
        public Guid Id { get; set; }

        public string CanonicalName { get; set; }

        public string BreadcrumbTitle { get; set; }

        public bool IncludeInSiteMap { get; set; }

        public MetaTags MetaTags { get; set; }

        public string Content { get; set; }

        public DateTime LastReviewed { get; set; }

        public IEnumerable<string> AlternativeNames { get; set; } = Enumerable.Empty<string>();
    }
}
