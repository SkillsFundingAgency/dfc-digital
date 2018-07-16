using System;
using System.Linq;
using System.Text;
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
            // this will be async once integrated
            var onetSkills = onetService.GetOnetSkills();

            var response = new OnetSkillsImportResponse();
            var details = new StringBuilder();
            response.SummaryDetails = $"Found {onetSkills.OnetSkillsList.Count()} onet skills to import";

            foreach (var onetSkill in onetSkills.OnetSkillsList)
            {
                onetRepository.UpsertOnetSkill(onetSkill);
                details.AppendLine($"Added/Updated {onetSkill.Title} to repository <br /> ");
            }

            response.ActionDetails = details.ToString();
            response.Success = true;
            return response;
        }

        public UpdateSocOccupationalCodeResponse UpdateSocCodesOccupationalCode()
        {
            var socCodesToUpdate = jobProfileSocCodeRepository.GetLiveSocCodes().Where(soc => string.IsNullOrWhiteSpace(soc.ONetOccupationalCode)).Take(50);

            var socCodesUpdated = jobProfileSocCodeRepository.GetLiveSocCodes().Count(soc => !string.IsNullOrWhiteSpace(soc.ONetOccupationalCode));

            var response = new UpdateSocOccupationalCodeResponse();
            var details = new StringBuilder();
            var summaryDetails = new StringBuilder();
            summaryDetails.AppendLine($"Found {socCodesUpdated} socs already update with occupational codes <br />");
            summaryDetails.AppendLine($"Found {socCodesToUpdate.Count()} socs to update <br />");

            foreach (var socCode in socCodesToUpdate)
            {
                var occupationalCode = onetService.GetSocOccupationalCode(socCode.SOCCode);

                details.AppendLine($"Found {occupationalCode} for SocCocde : {socCode.SOCCode} from Onet Service  <br /> ");

                if (!string.IsNullOrWhiteSpace(occupationalCode) &&
                    !socCode.SOCCode.Equals(occupationalCode, StringComparison.OrdinalIgnoreCase))
                {
                    socCode.ONetOccupationalCode = occupationalCode;
                    jobProfileSocCodeRepository.UpdateSocOccupationalCode(socCode);
                    details.AppendLine($"Updated Soc code {socCode.SOCCode} with occupational code : {occupationalCode}  <br /> ");
                }
            }

            response.SummaryDetails = summaryDetails.ToString();
            response.ActionDetails = details.ToString();
            response.Success = true;
            return response;
        }

        public UpdateJpDigitalSkillsResponse UpdateJobProfilesDigitalSkills()
        {
            var jobprofilesToUpdate = jobProfileRepository.GetLiveJobProfiles().Where(jp => string.IsNullOrWhiteSpace(jp.DigitalSkillsLevel) && !string.IsNullOrWhiteSpace(jp.SOCCode)).Take(50);

            var jobProfilesUpdated = jobProfileRepository.GetLiveJobProfiles().Count(jp => !string.IsNullOrWhiteSpace(jp.DigitalSkillsLevel));

            var response = new UpdateJpDigitalSkillsResponse();
            var details = new StringBuilder();
            var summaryDetails = new StringBuilder();
            summaryDetails.AppendLine($"Found {jobprofilesToUpdate.Count()} job profiles to update with Onet Dgital skill  <br /> ");
            summaryDetails.AppendLine($"Found {jobProfilesUpdated} job profiles already updated with Onet Dgital skill  <br /> ");

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                if (!string.IsNullOrWhiteSpace(jobProfile.ONetOccupationalCode))
                {
                    var digitalSkillLevel = onetService.GetDigitalSkillLevel(jobProfile.ONetOccupationalCode);

                    details.AppendLine($"Found {digitalSkillLevel} for Occupational Code : {jobProfile.ONetOccupationalCode} from Onet Service  <br /> ");

                    if (!string.IsNullOrWhiteSpace(digitalSkillLevel) && (string.IsNullOrWhiteSpace(jobProfile.DigitalSkillsLevel) ||
                        !jobProfile.DigitalSkillsLevel.Equals(digitalSkillLevel, StringComparison.OrdinalIgnoreCase)))
                    {
                        jobProfile.DigitalSkillsLevel = digitalSkillLevel;
                        jobProfileRepository.UpdateDigitalSkill(jobProfile);
                        details.AppendLine($"Updated Job profile {jobProfile.UrlName} with digital skill level : {digitalSkillLevel}  <br /> ");
                    }
                }
            }

            response.SummaryDetails = summaryDetails.ToString();
            response.ActionDetails = details.ToString();
            response.Success = true;
            return response;
        }

        public BuildSocMatrixResponse BuildSocMatrixData()
        {
            var response = new BuildSocMatrixResponse();
            var details = new StringBuilder();
            var summaryDetails = new StringBuilder();
            var socSkillMatrices = onetRepository.GetSocSkillMatrices();
            summaryDetails.AppendLine($"Found {socSkillMatrices.Count()} soc skill matrices in the repo  <br /> ");

            var importedSocs = socSkillMatrices.Select(socSkil => socSkil.SocCode).ToList();

            var jobprofilesToUpdate = jobProfileRepository.GetLiveJobProfiles().Where(jp => !importedSocs.Contains(jp.SOCCode)).Take(50);
            summaryDetails.AppendLine($"Found {jobprofilesToUpdate.Count()} jobprofiles without skill matrices in the repo, based on soc Code  <br /> ");

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                if (!string.IsNullOrWhiteSpace(jobProfile.ONetOccupationalCode))
                {
                    var occupationSkills = onetService.GetOccupationalCodeSkills(jobProfile.ONetOccupationalCode);
                    details.AppendLine($"Found {string.Join(",", occupationSkills.Select(oc => oc.Title))} skills : for occupation code {jobProfile.ONetOccupationalCode} from Onet Service  <br /> ");
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
                        details.AppendLine($"Added/Updated Sco Skill Matrix profile {jobProfile.SOCCode}-{occupationSkill.Title}  into socskill matrix repo  <br /> ");
                        rankGenerated++;
                    }
                }
            }

            response.ActionDetails = details.ToString();
            response.Success = true;
            return response;
        }

        public UpdateJpSocSkillMatrixResponse UpdateJpSocSkillMatrix()
        {
            var response = new UpdateJpSocSkillMatrixResponse();
            var details = new StringBuilder();
            var summaryDetails = new StringBuilder();
            var jobprofilesToUpdate = jobProfileRepository.GetLiveJobProfiles().Where(jp => !jp.HasRelatedSocSkillMatrices).Take(50);
            var jobprofilesWithSkills = jobProfileRepository.GetLiveJobProfiles().Count(jp => jp.HasRelatedSocSkillMatrices);
            summaryDetails.AppendLine($"Found {jobprofilesToUpdate.Count()} jobprofiles without related skill matrices in the repo  <br /> ");
            summaryDetails.AppendLine($"Found {jobprofilesWithSkills} jobprofiles with related skill matrices in the repo  <br /> ");

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                var socSkillMatrixData = jobProfileSocCodeRepository.GetSocSkillMatricesBySocCode(jobProfile.SOCCode);

                details.AppendLine($"Found {string.Join(",", socSkillMatrixData.Select(oc => oc.Title))} soc skills : for job profile soccode {jobProfile.SOCCode} from Onet repository  <br /> ");

                if (socSkillMatrixData.Any())
                {
                    jobProfileRepository.UpdateSocSkillMatrices(jobProfile, socSkillMatrixData.ToList());
                    details.AppendLine($"Updated Job Profile {jobProfile.UrlName} with the following socskilmatrices {string.Join(",", socSkillMatrixData.ToList().Select(sk => sk.Title))}  <br /> ");
                }
            }

            response.ActionDetails = details.ToString();
            response.Success = true;
            return response;
        }
    }
}