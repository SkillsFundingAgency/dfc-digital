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

            var allOnetSkills = onetRepository.GetOnetSkills().Count();

            var response = new OnetSkillsImportResponse();
            var details = new StringBuilder();
            var summaryDetails = new StringBuilder();
            summaryDetails.AppendLine($"Found {allOnetSkills} onet skills in the repo <br />");
            summaryDetails.AppendLine($"Found {onetSkills.OnetSkillsList.Count()} onet skills to import <br />");

            foreach (var onetSkill in onetSkills.OnetSkillsList)
            {
                onetRepository.UpsertOnetSkill(onetSkill);
                details.AppendLine($"Added/Updated {onetSkill.Title} to repository <br /> ");
            }

            response.SummaryDetails = summaryDetails.ToString();
            response.ActionDetails = details.ToString();
            response.Success = true;
            return response;
        }

        public UpdateSocOccupationalCodeResponse UpdateSocCodesOccupationalCode()
        {
            var allSocCodes = jobProfileSocCodeRepository.GetLiveSocCodes().ToList();
            var socCodesToUpdate = allSocCodes.Where(soc => string.IsNullOrWhiteSpace(soc.ONetOccupationalCode)).Take(50);
            var socCodesNotUpdated = allSocCodes.Count(soc => string.IsNullOrWhiteSpace(soc.ONetOccupationalCode));
            var socCodesUpdated = allSocCodes.Count(soc => !string.IsNullOrWhiteSpace(soc.ONetOccupationalCode));

            var response = new UpdateSocOccupationalCodeResponse();
            var details = new StringBuilder();
            var summaryDetails = new StringBuilder();
            summaryDetails.AppendLine($"Found {allSocCodes.Count} socs in the repo <br />");
            summaryDetails.AppendLine($"Found {socCodesUpdated} socs already updated with occupational codes <br />");
            summaryDetails.AppendLine($"Found {socCodesNotUpdated} socs not yet updated with occupational codes <br />");
            summaryDetails.AppendLine($"Going to update {socCodesToUpdate.Count()} socs with occupational code <br />");

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

            var allJobProfiles = jobProfileRepository.GetLiveJobProfiles().ToList();
            var jobprofilesToUpdate = allJobProfiles.Where(jp => string.IsNullOrWhiteSpace(jp.DigitalSkillsLevel) && !string.IsNullOrWhiteSpace(jp.ONetOccupationalCode)).Take(50).ToList();
            var jobProfilesUpdated = allJobProfiles.Count(jp => !string.IsNullOrWhiteSpace(jp.DigitalSkillsLevel));
            var jobProfileswithoutDigitalSkills = allJobProfiles.Count(jp => string.IsNullOrWhiteSpace(jp.DigitalSkillsLevel));

            var response = new UpdateJpDigitalSkillsResponse();
            var details = new StringBuilder();
            var summaryDetails = new StringBuilder();
            summaryDetails.AppendLine($"Found {allJobProfiles.Count} job profiles in the repo  <br /> ");
            summaryDetails.AppendLine($"Found a total of {jobProfileswithoutDigitalSkills} job profiles without an Onet Digital skill  <br /> ");
            summaryDetails.AppendLine($"Found {jobProfilesUpdated} job profiles already updated with Onet Digital skill  <br /> ");
            summaryDetails.AppendLine($"Going to update {jobprofilesToUpdate.Count()} job profiles with an Onet Digital skill  <br /> ");

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

            var importedSocs = socSkillMatrices.Select(socSkil => socSkil.SocCode).Distinct().ToList();
            var allJobProfiles = jobProfileRepository.GetLiveJobProfiles().ToList();
            var jobprofilesToUpdate = allJobProfiles.Where(jp => !string.IsNullOrWhiteSpace(jp.ONetOccupationalCode) && !importedSocs.Contains(jp.SOCCode)).Take(50);
            var jobprofilesWithRelatedSocs = allJobProfiles.Count(jp => !string.IsNullOrWhiteSpace(jp.ONetOccupationalCode) && importedSocs.Contains(jp.SOCCode));
            var jobprofilesWithoutRelatedSocs = allJobProfiles.Count(jp => !string.IsNullOrWhiteSpace(jp.ONetOccupationalCode) && !importedSocs.Contains(jp.SOCCode));
            summaryDetails.AppendLine($"Found {allJobProfiles.Count} jobprofiles  in the repo <br /> ");
            summaryDetails.AppendLine($"Found {jobprofilesWithRelatedSocs} jobprofiles with skill matrices in the repo, based on soc Code  <br /> ");
            summaryDetails.AppendLine($"Found {string.Join(",", importedSocs)} soc codes in the skill matrix repo <br /> ");
            summaryDetails.AppendLine($"Found {jobprofilesWithoutRelatedSocs} jobprofiles without skill matrices in the repo, based on soc Code  <br /> ");
            summaryDetails.AppendLine($"Going to  use {jobprofilesToUpdate.Count()} jobprofiles to add soc skill matrices in the repo, based on soc Code  <br /> ");

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

            response.SummaryDetails = summaryDetails.ToString();
            response.ActionDetails = details.ToString();
            response.Success = true;
            return response;
        }

        public UpdateJpSocSkillMatrixResponse UpdateJpSocSkillMatrix()
        {
            var response = new UpdateJpSocSkillMatrixResponse();
            var details = new StringBuilder();
            var summaryDetails = new StringBuilder();
            var allJobProfiles = jobProfileRepository.GetLiveJobProfiles().ToList();
            var jobprofilesToUpdate = allJobProfiles.Where(jp => !jp.HasRelatedSocSkillMatrices).Take(50);
            var jobprofilesWithSkills = allJobProfiles.Count(jp => jp.HasRelatedSocSkillMatrices);
            var jobprofilesWithoutSkills = allJobProfiles.Count(jp => !jp.HasRelatedSocSkillMatrices);
            summaryDetails.AppendLine($"Found {allJobProfiles.Count} jobprofiles  in the repo <br /> ");
            summaryDetails.AppendLine($"Found {jobprofilesWithoutSkills} jobprofiles without related skill matrices in the repo  <br /> ");
            summaryDetails.AppendLine($"Found {jobprofilesWithSkills} jobprofiles with related skill matrices in the repo  <br /> ");
            summaryDetails.AppendLine($"Going to update {jobprofilesToUpdate.Count()} jobprofiles with related skill matrices in the repo  <br /> ");

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

            response.SummaryDetails = summaryDetails.ToString();
            response.ActionDetails = details.ToString();
            response.Success = true;
            return response;
        }
    }
}