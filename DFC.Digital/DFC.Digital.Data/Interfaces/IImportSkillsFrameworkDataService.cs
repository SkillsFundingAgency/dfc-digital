using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IImportSkillsFrameworkDataService
    {
        FrameworkSkillsImportResponse ImportFrameworkSkills();

        UpdateSocOccupationalCodeResponse UpdateSocCodesOccupationalCode();

        SkillsServiceResponse ImportForSocs(string jobProfileSocs);

        void ImportForSingleSoc(string jobProfileSoc);

        void ResetAllSocStatus();

        void ResetStartedSocStatus();

        SocMappingStatus GetSocMappingStatus();

        string GetNextBatchOfSOCsToImport(int batchsize);

        IList<SocSkillMatrix> CreateSocSkillsMatrixRecords(SocCode soc);

        UpdateSocOccupationalCodeResponse ExportNewONetMappings();
    }
}
