using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class DfcGdsDigitalSkills : DfcGdsOnetEntity
    {
        public ICollection<DfcGdsToolsAndTechnology> DigitalSkillsCollection { get; set; }

        public int DigitalSkillsRank { get; set; }
    }
}