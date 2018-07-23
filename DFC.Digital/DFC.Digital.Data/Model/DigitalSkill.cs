using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class DigitalSkill
    {
        public IEnumerable<DigitalToolsAndTechnology> DigitalSkillsCollection { get; set; }

        public int DigitalSkillsCount { get; set; }
    }
}