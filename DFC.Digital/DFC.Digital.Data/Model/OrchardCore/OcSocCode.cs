using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model.OrchardCore
{
    public class OcSocCode
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
        public Soccode SOCCode { get; set; }
        public Graphsyncpart GraphSyncPart { get; set; }
        public Audittrailpart AuditTrailPart { get; set; }
    }

    public class Soccode
    {
        public Soccode()
        {
            ApprenticeshipStandards = new Apprenticeshipstandards();
        }
        public OcDescriptionText Description { get; set; }
        public Onetoccupationcode OnetOccupationCode { get; set; }
        [JsonIgnore]
        public ApprenticeshipstandardsSitefinity ApprenticeshipStandardsSitefinityIds { get; set; }
        public Apprenticeshipstandards ApprenticeshipStandards { get; set; }
        [JsonIgnore]
        public SOC2020 SOC2020 { get; set; }
        [JsonIgnore]
        public Soc2020extension SOC2020extension { get; set; }
    }

    public class Onetoccupationcode
    {
        public string Text { get; set; }
    }

    public class ApprenticeshipstandardsSitefinity
    {
        public List<Guid> SitefinityIds { get; set; }
    }

    public class Apprenticeshipstandards
    {
        public string[] ContentItemIds { get; set; }
    }

    public class SOC2020
    {
        public string Text { get; set; }
    }

    public class Soc2020extension
    {
        public string Text { get; set; }
    }
}
