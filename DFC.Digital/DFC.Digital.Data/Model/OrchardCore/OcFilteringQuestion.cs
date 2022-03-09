using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model.OrchardCore
{

    public class OcFilteringQuestion
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
        [JsonIgnore]
        public string Owner { get; set; }
        [JsonIgnore]
        public string Author { get; set; }
        public Personalityfilteringquestion PersonalityFilteringQuestion { get; set; }
        public Graphsyncpart GraphSyncPart { get; set; }
        public Titlepart TitlePart { get; set; }
    }

    public class Personalityfilteringquestion
    {
        public Socskillsmatrix SOCSkillsMatrix { get; set; }
        public OcText Text { get; set; }
        [JsonIgnore]
        public OcText Info { get; set; }
    }

    public class Socskillsmatrix
    {
        public string[] ContentItemIds { get; set; }
    }
}
