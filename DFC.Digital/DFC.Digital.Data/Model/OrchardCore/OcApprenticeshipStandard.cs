﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model.OrchardCore
{

    public class OcApprenticeshipStandard
    {
        [JsonIgnore]
        public Guid SitefinityId { get; set; }
        public string ContentItemId { get; set; }
        public string ContentItemVersionId { get; set; }
        public string ContentType { get; set; }
        public string DisplayText { get; set; }
        public bool Latest { get; set; }
        public bool Published { get; set; }
        public DateTime ModifiedUtc { get; set; }
        public DateTime PublishedUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string Owner { get; set; }
        public string Author { get; set; }
        public Uniquetitlepart UniqueTitlePart { get; set; }
        public Titlepart TitlePart { get; set; }
        public Apprenticeshipstandard ApprenticeshipStandard { get; set; }
        public Graphsyncpart GraphSyncPart { get; set; }
        public Audittrailpart AuditTrailPart { get; set; }
    }

    public class Apprenticeshipstandard
    {
        public OcDescriptionText Description { get; set; }
        public Larscode LARScode { get; set; }
    }

    public class Larscode
    {
        public string Text { get; set; }
    }
}
