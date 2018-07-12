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

        public OnetDataImportService(IOnetService onetService, IOnetRepository onetRepository)
        {
            this.onetService = onetService;
            this.onetRepository = onetRepository;
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
            throw new NotImplementedException();
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
