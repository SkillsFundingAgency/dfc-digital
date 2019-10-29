using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class SocSkillMatrixContentItem : RelatedContentItem
    {
        public string SocSkillMatrixTitle { get; set; }

        public string Contextualised { get; set; }

        public string ONetAttributeType { get; set; }

        public decimal? Rank { get; set; }

        public decimal? ONetRank { get; set; }

        public FrameworkSkillItem RelatedSkill { get; set; }

        public IEnumerable<RelatedSocCodeItem> RelatedSOC { get; set; }
    }
}
