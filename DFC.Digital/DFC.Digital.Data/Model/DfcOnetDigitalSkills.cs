using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class DfcOnetDigitalSkills : OnetEntity
    {
        public IEnumerable<DfcOnetToolsAndTechnology> DigitalSkillsCollection { get; set; }

        public int DigitalSkillsCount { get; set; }
    }
}