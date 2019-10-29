using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class SkillContentItem : RelatedContentItem
    {
        public Guid SocSkillMatrixId { get; set; }

        public string SocSkillMatrixTitle { get; set; }

        public string Description { get; set; }

        public string ONetElementId { get; set; }
    }
}
