using System.Collections.Generic;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Service.SkillsFramework.Services.Contracts
{
    public interface ISkillsFrameworkService
    {
        FrameworkSkillsImportRequest GetOnetSkills();

        IDictionary<string, string> GetSocOccupationalCodeMappings();

        int GetDigitalSkillLevel(string onetOccupationalCode);

        IEnumerable<OccupationOnetSkill> GetOccupationalCodeSkills(string onetOccupationalCode);
    }
}
