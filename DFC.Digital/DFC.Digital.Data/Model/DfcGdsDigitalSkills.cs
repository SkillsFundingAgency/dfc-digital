using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class DfcGdsDigitalSkills : DfcGdsOnetEntity
    {
        public IEnumerable<DfcGdsToolsAndTechnology> DigitalSkillsCollection { get; set; }

        public int DigitalSkillsCount { get; set; }
    }
}