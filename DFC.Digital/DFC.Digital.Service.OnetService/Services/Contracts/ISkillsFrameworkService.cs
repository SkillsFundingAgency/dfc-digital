using System.Collections.Generic;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Service.SkillsFrameworkData
{
    public interface ISkillsFrameworkService
    {
       IEnumerable<FrameworkSkill> GetOnetSkills();

        IDictionary<string, string> GetSocOccupationalCodeMappings();

        int GetDigitalSkillLevel(string onetOccupationalCode);

        IEnumerable<OccupationOnetSkill> GetOccupationalCodeSkills(string onetOccupationalCode);
    }
}
