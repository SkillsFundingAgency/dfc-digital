using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISkillsFrameworkService
    {
        IEnumerable<SocCode> GetAllSocMappings();

        IEnumerable<SocCode> GetNextBatchSocMappingsForUpdate(int batchSize);

        IEnumerable<FrameworkSkill> GetAllTranslations();

        DigitalSkillsLevel GetDigitalSkillLevel(string onetOccupationalCode);

        IEnumerable<OnetAttribute> GetRelatedSkillMapping(string onetOccupationalCode);

        void ResetSocStatus(IEnumerable<SocCode> socCodes);

        void SetSocStatusCompleted(SocCode socCodes);

        void SetSocStatusSelectedForUpdate(SocCode socCodes);
    }
}