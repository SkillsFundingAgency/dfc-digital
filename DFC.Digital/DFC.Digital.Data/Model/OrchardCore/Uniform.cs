﻿
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model.OrchardCore.Uniform
{

    public class OcUniform
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
        public DateTime PublishedUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
        [JsonIgnore]
        public string Owner { get; set; }
        [JsonIgnore]
        public string Author { get; set; }
        public Uniquetitlepart UniqueTitlePart { get; set; }
        public Titlepart TitlePart { get; set; }
        public Uniform Uniform { get; set; }
        public Graphsyncpart GraphSyncPart { get; set; }
        public Audittrailpart AuditTrailPart { get; set; }
    }

    public class Uniform
    {
        public OcDescriptionHtml Description { get; set; }
    }
}