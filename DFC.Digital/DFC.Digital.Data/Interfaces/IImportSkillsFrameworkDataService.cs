using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface IImportSkillsFrameworkDataService
    {
        FrameworkSkillsImportResponse ImportFrameworkSkills();

        UpdateSocOccupationalCodeResponse UpdateSocCodesOccupationalCode();

        UpdateJpSocSkillMatrixResponse ImportForSoc(string jobProfileSoc);

        void ResetAllSocStatus();
    }
}
