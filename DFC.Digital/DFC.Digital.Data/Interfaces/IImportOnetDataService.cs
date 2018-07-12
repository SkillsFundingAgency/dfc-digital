using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface IImportOnetDataService
    {
        OnetSkillsImportResponse ImportOnetSkills();

        UpdateSocOccupationalCodeResponse UpdateSocCodesOccupationalCode();

        UpdateJpDigitalSkillsResponse UpdateJobProfilesDigitalSkills();

        BuildSocMatrixResponse BuildSocMatrixData();
    }
}
