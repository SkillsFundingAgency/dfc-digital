using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DFC.Digital.Data.Model
{
    public class SocSkillMatrixItem : IDigitalDataModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Contextualised { get; set; }

        public string ONetAttributeType { get; set; }

        public decimal? Rank { get; set; }

        public decimal? ONetRank { get; set; }

        public IEnumerable<FrameworkSkillItem> RelatedSkill { get; set; }

        public IEnumerable<RelatedSocCodeItem> RelatedSOC { get; set; }
    }
}
