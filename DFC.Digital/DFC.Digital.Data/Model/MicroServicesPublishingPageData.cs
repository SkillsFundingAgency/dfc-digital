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
        public Guid ContentPageId { get; set; }

        public string Category { get; set; }

        public string CanonicalName { get; set; }

        public string Title { get; set; }

        public bool IncludeInSiteMap { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public string Content { get; set; }

        public DateTime LastModified { get; set; }

        public IEnumerable<string> AlternativeNames { get; set; } = Enumerable.Empty<string>();

        public string BreadcrumbTitle { get; set; }
    }
}
