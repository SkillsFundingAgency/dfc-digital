using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model.OrchardCore
{

    public class ApprenticeshipEntryRequirement
    {
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
        public Apprenticeshipentryrequirements ApprenticeshipEntryRequirements { get; set; }
        public Graphsyncpart GraphSyncPart { get; set; }
        public Audittrailpart AuditTrailPart { get; set; }
    }

    public class Uniquetitlepart
    {
        public string Title { get; set; }
    }

    public class Titlepart
    {
        public string Title { get; set; }
    }

    public class Apprenticeshipentryrequirements
    {
        public Description Description { get; set; }
    }

    public class Description
    {
        public string Text { get; set; }
    }

    public class Graphsyncpart
    {
        public string Text { get; set; }
    }

    public class Audittrailpart
    {
        public object Comment { get; set; }
        public bool ShowComment { get; set; }
    }
}
