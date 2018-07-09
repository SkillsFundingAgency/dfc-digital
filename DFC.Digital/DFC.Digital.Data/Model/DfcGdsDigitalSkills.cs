using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class DfcGdsDigitalSkills : DfcGdsOnetEntity
    {
        public IEnumerable<DfcGdsToolsAndTechnology> DigitalSkillsCollection { get; set; }

        public int DigitalSkillsRank { get; set; }
    }
}