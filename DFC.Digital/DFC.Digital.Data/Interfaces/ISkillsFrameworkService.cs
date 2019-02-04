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

        IEnumerable<OnetSkill> GetRelatedSkillMapping(string onetOccupationalCode);

        void ResetAllSocStatus();

        void ResetStartedSocStatus();

        void SetSocStatusCompleted(SocCode socCode);

        void SetSocStatusSelectedForUpdate(SocCode socCode);

        SocMappingStatus GetSocMappingStatus();

        void AddNewSOCMappings(IEnumerable<SocCode> newSocCodes);
    }
}