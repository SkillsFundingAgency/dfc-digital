using System;
using System.Linq;
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
        private readonly IJobProfileRepository jobProfileRepository;


        public OnetDataImportService(IOnetService onetService, IOnetRepository onetRepository, IJobProfileSocCodeRepository jobProfileSocCodeRepository, IJobProfileRepository jobProfileRepository)
        {
            this.onetService = onetService;
            this.onetRepository = onetRepository;
            this.jobProfileSocCodeRepository = jobProfileSocCodeRepository;
            this.jobProfileRepository = jobProfileRepository;
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
            var socCodesToUpdate = jobProfileSocCodeRepository.GetLiveSocCodes().ToList().Take(2);

            foreach (var socCode in socCodesToUpdate)
            {
                var occupationalCode = onetService.GetSocOccupationalCode(socCode.SOCCode);

                if (!string.IsNullOrWhiteSpace(occupationalCode) &&
                    !socCode.SOCCode.Equals(occupationalCode, StringComparison.OrdinalIgnoreCase))
                {
                    socCode.ONetOccupationalCode = occupationalCode;
                    jobProfileSocCodeRepository.UpdateSocOccupationalCode(socCode);
                }
            }

            return new UpdateSocOccupationalCodeResponse
            {
                Success = true
            };
        }

        public UpdateJpDigitalSkillsResponse UpdateJobProfilesDigitalSkills()
        {
            var jobprofilesToUpdate = jobProfileRepository.GetLiveJobProfiles().ToList().Take(2);

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                if (!string.IsNullOrWhiteSpace(jobProfile.ONetOccupationalCode))
                {
                    var digitalSkillLevel = onetService.GetDigitalSkillLevel(jobProfile.ONetOccupationalCode);

                    if (!string.IsNullOrWhiteSpace(digitalSkillLevel) && (string.IsNullOrWhiteSpace(jobProfile.DigitalSkillsLevel) ||
                        !jobProfile.DigitalSkillsLevel.Equals(digitalSkillLevel, StringComparison.OrdinalIgnoreCase)))
                    {
                        jobProfile.DigitalSkillsLevel = digitalSkillLevel;
                        jobProfileRepository.UpdateDigitalSkill(jobProfile);
                    }
                }
            }

            return new UpdateJpDigitalSkillsResponse
            {
                Success = true
            };
        }

        public BuildSocMatrixResponse BuildSocMatrixData()
        {
            var jobprofilesToUpdate = jobProfileRepository.GetLiveJobProfiles().ToList().Take(2);

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                if (!string.IsNullOrWhiteSpace(jobProfile.ONetOccupationalCode))
                {
                    var occupationSkills = onetService.GetOccupationalCodeSkills(jobProfile.ONetOccupationalCode);
                    var rankGenerated = 1;
                    foreach (var occupationSkill in occupationSkills)
                    {
                        onetRepository.UpsertSocSkillMatrix(new SocSkillMatrix
                        {
                            Title = $"{jobProfile.SOCCode}-{occupationSkill.Title}",
                            SocCode = jobProfile.SOCCode,
                            Skill = occupationSkill.Title,
                            ONetRank = occupationSkill.OnetRank,
                            Rank = rankGenerated
                        });

                        rankGenerated++;
                    }
                }
            }

            return new BuildSocMatrixResponse
            {
                Success = true
            };
        }

        public UpdateJpSocSkillMatrixResponse UpdateJpSocSkillMatrix()
        {
            var jobprofilesToUpdate = jobProfileRepository.GetLiveJobProfiles().ToList().Take(2);

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                var socSkillMatrixData = onetRepository.GetSocSkillMatricesBySocCode(jobProfile.SOCCode);
               
                if (socSkillMatrixData.Any())
                {
                    jobProfileRepository.UpdateSocSkillMatrices(jobProfile, socSkillMatrixData);
                }
            }

            return new UpdateJpSocSkillMatrixResponse
            {
                Success = true
            };
        }
    }
}
