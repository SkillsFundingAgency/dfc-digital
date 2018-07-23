using System.Linq;

namespace DFC.Digital.Repository.ONET
{
    internal class DigitalSkill
    {
        public DigitalSkill()
        {
        }

        public IQueryable<DigitalToolsAndTechnology> DigitalSkillsCollection { get; internal set; }
        public int DigitalSkillsCount { get; internal set; }
    }
}