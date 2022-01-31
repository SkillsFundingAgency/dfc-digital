﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model.OrchardCore
{

    public class OcJobProfileCategory
    {
        [JsonIgnore]
        public Guid SitefinityId { get; set; }
        public string ContentItemId { get; set; }
        [JsonIgnore]
        public string ContentItemVersionId { get; set; }
        public string ContentType { get; set; }
        public string DisplayText { get; set; }
        public bool Latest { get; set; }
        public bool Published { get; set; }
        public DateTime ModifiedUtc { get; set; }
        [JsonIgnore]
        public DateTime PublishedUtc { get; set; }
        [JsonIgnore]
        public DateTime CreatedUtc { get; set; }
        [JsonIgnore]
        public string Owner { get; set; }
        [JsonIgnore]
        public string Author { get; set; }
        public Uniquetitlepart UniqueTitlePart { get; set; }
        public Titlepart TitlePart { get; set; }
        public Jobprofilecategory JobProfileCategory { get; set; }
        public Pagelocationpart PageLocationPart { get; set; }
        public Graphsyncpart GraphSyncPart { get; set; }
        public Audittrailpart AuditTrailPart { get; set; }
    }

    public class Jobprofilecategory
    {
        public OcDescriptionText Description { get; set; }
    }

    public class Pagelocationpart
    {
        public string UrlName { get; set; }
        public bool DefaultPageForLocation { get; set; }
        public object RedirectLocations { get; set; }
        public string FullUrl { get; set; }
    }
}