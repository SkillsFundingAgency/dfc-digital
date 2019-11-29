using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class RelatedContentItem
    {
        public Guid Id { get; set; }

        public Guid LiveId { get; set; }

        public string Title { get; set; }

        public Guid JobProfileId { get; set; }

        public string JobProfileTitle { get; set; }

        public string Status { get; set; }

        public string WorkflowStatus { get; set; }

        public string EventAction { get; set; }
    }
}
