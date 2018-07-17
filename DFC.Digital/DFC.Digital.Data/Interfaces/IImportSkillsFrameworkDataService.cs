using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface IImportSkillsFrameworkDataService
    {
        FrameworkSkillsImportResponse ImportOnetSkills();

        UpdateSocOccupationalCodeResponse UpdateSocCodesOccupationalCode();

        UpdateJpDigitalSkillsResponse UpdateJobProfilesDigitalSkills();

        BuildSocMatrixResponse BuildSocMatrixData();

        UpdateJpSocSkillMatrixResponse UpdateJpSocSkillMatrix();
    }
}
