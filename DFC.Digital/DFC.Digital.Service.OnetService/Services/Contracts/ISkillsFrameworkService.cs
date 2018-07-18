using System.Collections.Generic;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Service.OnetService.Services.Contracts
{
    public interface ISkillsFrameworkService
    {
        FrameworkSkillsImportRequest GetOnetSkills();

        string GetSocOccupationalCode(string socCode);

        string GetDigitalSkillLevel(string onetOccupationalCode);

        IEnumerable<OccupationOnetSkill> GetOccupationalCodeSkills(string onetOccupationalCode);
    }
}
