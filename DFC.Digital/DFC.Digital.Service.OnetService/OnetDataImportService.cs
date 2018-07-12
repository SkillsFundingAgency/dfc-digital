using System;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.OnetService.Services.Contracts;

namespace DFC.Digital.Service.OnetService
{
    public class OnetDataImportService : IImportOnetDataService
    {
        private readonly IOnetService onetService;
        private readonly IOnetRepository onetRepository;
        private readonly IJobProfileSocCodeRepository jobProfileSocCodeRepository;

        public OnetDataImportService(IOnetService onetService, IOnetRepository onetRepository, IJobProfileSocCodeRepository jobProfileSocCodeRepository)
        {
            this.onetService = onetService;
            this.onetRepository = onetRepository;
            this.jobProfileSocCodeRepository = jobProfileSocCodeRepository;
        }

        public OnetSkillsImportResponse ImportOnetSkills()
        {
            var onetSkills = onetService.GetOnetSkills();

            foreach (var onetSkill in onetSkills.OnetSkillsList)
            {
                onetRepository.UpsertOnetSkill(onetSkill);
            }

            return new OnetSkillsImportResponse
            {
                Success = true
            };
        }

        public UpdateSocOccupationalCodeResponse UpdateSocCodesOccupationalCode()
        {
            var socCodes = onetService.GetUpdateSocOccupationalCodeRequest();

            foreach (var socCode in socCodes.SocCodeList)
            {
                jobProfileSocCodeRepository.UpdateSocOccupationalCode(socCode);
            }

            return new UpdateSocOccupationalCodeResponse
            {
                Success = true
            };
        }

        public UpdateJpDigitalSkillsResponse UpdateJobProfilesDigitalSkills()
        {
            throw new NotImplementedException();
        }

        public BuildSocMatrixResponse BuildSocMatrixData()
        {
            throw new NotImplementedException();
        }
    }
}
