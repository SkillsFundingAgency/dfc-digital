using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface IImportSkillsFrameworkDataService
    {
        FrameworkSkillsImportResponse ImportFrameworkSkills();

        UpdateSocOccupationalCodeResponse UpdateSocCodesOccupationalCode();

        SkillsServiceResponse ImportForSocs(string jobProfileSocs);

        void ResetAllSocStatus();

        void ResetStartedSocStatus();

        SocMappingStatus GetSocMappingStatus();

        string GetNextBatchOfSOCsToImport(int batchsize);
    }
}
