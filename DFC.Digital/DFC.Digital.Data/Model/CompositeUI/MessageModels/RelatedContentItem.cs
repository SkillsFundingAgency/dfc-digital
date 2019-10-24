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

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid JobProfileId { get; set; }

        public string JobProfileTitle { get; set; }

        public string Url { get; set; }

        public bool IsNegative { get; set; }
    }
}
