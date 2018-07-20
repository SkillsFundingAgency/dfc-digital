using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface IImportSkillsFrameworkDataService
    {
        FrameworkSkillsImportResponse ImportFrameworkSkills();

        UpdateSocOccupationalCodeResponse UpdateSocCodesOccupationalCode();

        UpdateJpDigitalSkillsResponse UpdateJobProfilesDigitalSkills();

        BuildSocMatrixResponse BuildSocMatrixData();

        UpdateJpSocSkillMatrixResponse UpdateJpSocSkillMatrix();
    }
}
