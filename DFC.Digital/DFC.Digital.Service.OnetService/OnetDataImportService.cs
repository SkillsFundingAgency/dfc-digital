using System;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.OnetService.Services.Contracts;

namespace DFC.Digital.Service.OnetService
{
    public class OnetDataImportService : IImportOnetDataService
    {
        private const string SummaryDetailsKey = "SummaryDetails";
        private const string ActionDetailsKey = "ActionDetails";
        private readonly IOnetService onetService;
        private readonly IOnetRepository onetRepository;
        private readonly IJobProfileSocCodeRepository jobProfileSocCodeRepository;
        private readonly IJobProfileRepository jobProfileRepository;
        private readonly IReportAuditRepository reportAuditRepository;

        public OnetDataImportService(IOnetService onetService, IOnetRepository onetRepository, IJobProfileSocCodeRepository jobProfileSocCodeRepository, IJobProfileRepository jobProfileRepository, IReportAuditRepository reportAuditRepository)
        {
            //CodeReview: Please do not use ONET anywhere other than the actual repo by dinesh to pull the data out of onet.db
            this.onetService = onetService;
            
            //CodeReview: Please re-name this to sitefinity content type specific repo.
            this.onetRepository = onetRepository;
            this.jobProfileSocCodeRepository = jobProfileSocCodeRepository;
            this.jobProfileRepository = jobProfileRepository;
            this.reportAuditRepository = reportAuditRepository;
        }

        public OnetSkillsImportResponse ImportOnetSkills()
        {
            // this will be async once integrated
            var onetSkills = onetService.GetOnetSkills();
            var allOnetSkills = onetRepository.GetOnetSkills().Count();
            var response = new OnetSkillsImportResponse();
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {allOnetSkills} onet skills in the repo"));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {onetSkills.OnetSkillsList.Count()} onet skills to import"));

            foreach (var onetSkill in onetSkills.OnetSkillsList)
            {
                onetRepository.UpsertOnetSkill(onetSkill);
                reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(ActionDetailsKey, $"Added/Updated {onetSkill.Title} to repository"));
            }

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
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {allSocCodes.Count} socs in the repo <br />"));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {socCodesUpdated} socs already updated with occupational codes <br />"));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {socCodesNotUpdated} socs not yet updated with occupational codes <br />"));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Going to update {socCodesToUpdate.Count()} socs with occupational code <br />"));
 
            foreach (var socCode in socCodesToUpdate)
            {
                var occupationalCode = onetService.GetSocOccupationalCode(socCode.SOCCode);

                reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(ActionDetailsKey, $"Found {occupationalCode} for SocCocde : {socCode.SOCCode} from Onet Service  <br /> "));

                if (!string.IsNullOrWhiteSpace(occupationalCode) &&
                    !socCode.SOCCode.Equals(occupationalCode, StringComparison.OrdinalIgnoreCase))
                {
                    socCode.ONetOccupationalCode = occupationalCode;
                    jobProfileSocCodeRepository.UpdateSocOccupationalCode(socCode);
                    reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(ActionDetailsKey, $"Updated Soc code {socCode.SOCCode} with occupational code : {occupationalCode}  <br /> "));
                }
            }

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

            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {allJobProfiles.Count} job profiles in the repo  <br /> "));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found a total of {jobProfileswithoutDigitalSkills} job profiles without an Onet Digital skill  <br /> "));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {jobProfilesUpdated} job profiles already updated with Onet Digital skill  <br /> "));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Going to update {jobprofilesToUpdate.Count} job profiles with an Onet Digital skill  <br /> "));

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                if (!string.IsNullOrWhiteSpace(jobProfile.ONetOccupationalCode))
                {
                    var digitalSkillLevel = onetService.GetDigitalSkillLevel(jobProfile.ONetOccupationalCode);

                    reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(ActionDetailsKey, $"Found {digitalSkillLevel} for Occupational Code : {jobProfile.ONetOccupationalCode} from Onet Service  <br /> "));

                    if (!string.IsNullOrWhiteSpace(digitalSkillLevel) && (string.IsNullOrWhiteSpace(jobProfile.DigitalSkillsLevel) ||
                        !jobProfile.DigitalSkillsLevel.Equals(digitalSkillLevel, StringComparison.OrdinalIgnoreCase)))
                    {
                        jobProfile.DigitalSkillsLevel = digitalSkillLevel;
                        jobProfileRepository.UpdateDigitalSkill(jobProfile);
                        reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(ActionDetailsKey, $"Updated Job profile {jobProfile.UrlName} with digital skill level : {digitalSkillLevel}  <br /> "));
                    }
                }
            }

            response.Success = true;
            return response;
        }

        public BuildSocMatrixResponse BuildSocMatrixData()
        {
            var response = new BuildSocMatrixResponse();
            var socSkillMatrices = onetRepository.GetSocSkillMatrices();
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {socSkillMatrices.Count()} soc skill matrices in the repo  <br /> "));

            var importedSocs = socSkillMatrices.Select(socSkil => socSkil.SocCode).Distinct().ToList();
            var allJobProfiles = jobProfileRepository.GetLiveJobProfiles().ToList();
            var jobprofilesToUpdate = allJobProfiles.Where(jp => !string.IsNullOrWhiteSpace(jp.ONetOccupationalCode) && !importedSocs.Contains(jp.SOCCode)).Take(50);
            var jobprofilesWithRelatedSocs = allJobProfiles.Count(jp => !string.IsNullOrWhiteSpace(jp.ONetOccupationalCode) && importedSocs.Contains(jp.SOCCode));
            var jobprofilesWithoutRelatedSocs = allJobProfiles.Count(jp => !string.IsNullOrWhiteSpace(jp.ONetOccupationalCode) && !importedSocs.Contains(jp.SOCCode));

            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {allJobProfiles.Count} jobprofiles  in the repo <br /> "));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {jobprofilesWithRelatedSocs} jobprofiles with skill matrices in the repo, based on soc Code  <br /> "));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {string.Join(",", importedSocs)} soc codes in the skill matrix repo <br /> "));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {jobprofilesWithoutRelatedSocs} jobprofiles without skill matrices in the repo, based on soc Code  <br /> "));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Going to  use {jobprofilesToUpdate.Count()} jobprofiles to add soc skill matrices in the repo, based on soc Code  <br /> "));

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                if (!string.IsNullOrWhiteSpace(jobProfile.ONetOccupationalCode))
                {
                    var occupationSkills = onetService.GetOccupationalCodeSkills(jobProfile.ONetOccupationalCode);

                    reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(ActionDetailsKey, $"Found {string.Join(",", occupationSkills.Select(oc => oc.Title))} skills : for occupation code {jobProfile.ONetOccupationalCode} from Onet Service  <br /> "));
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
                        reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(ActionDetailsKey, $"Added/Updated Sco Skill Matrix profile {jobProfile.SOCCode}-{occupationSkill.Title}  into socskill matrix repo  <br /> "));
                        rankGenerated++;
                    }
                }
            }

            response.Success = true;
            return response;
        }

        public UpdateJpSocSkillMatrixResponse UpdateJpSocSkillMatrix()
        {
            var response = new UpdateJpSocSkillMatrixResponse();
            var allJobProfiles = jobProfileRepository.GetLiveJobProfiles().ToList();
            var jobprofilesToUpdate = allJobProfiles.Where(jp => !jp.HasRelatedSocSkillMatrices).Take(50);
            var jobprofilesWithSkills = allJobProfiles.Count(jp => jp.HasRelatedSocSkillMatrices);
            var jobprofilesWithoutSkills = allJobProfiles.Count(jp => !jp.HasRelatedSocSkillMatrices);

            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {allJobProfiles.Count} jobprofiles  in the repo <br /> "));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {jobprofilesWithoutSkills} jobprofiles without related skill matrices in the repo  <br /> "));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Found {jobprofilesWithSkills} jobprofiles with related skill matrices in the repo  <br /> "));
            reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(SummaryDetailsKey, $"Going to update {jobprofilesToUpdate.Count()} jobprofiles with related skill matrices in the repo  <br /> "));

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                var socSkillMatrixData = jobProfileSocCodeRepository.GetSocSkillMatricesBySocCode(jobProfile.SOCCode);

                reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(ActionDetailsKey, $"Found {string.Join(",", socSkillMatrixData.Select(oc => oc.Title))} soc skills : for job profile soccode {jobProfile.SOCCode} from Onet repository  <br /> "));
                if (socSkillMatrixData.Any())
                {
                    jobProfileRepository.UpdateSocSkillMatrices(jobProfile, socSkillMatrixData.ToList());
                    reportAuditRepository.CreateAudit(new KeyValuePair<string, object>(ActionDetailsKey, $"Updated Job Profile {jobProfile.UrlName} with the following socskilmatrices {string.Join(",", socSkillMatrixData.ToList().Select(sk => sk.Title))}  <br /> "));
                }
            }

            response.Success = true;
            return response;
        }
    }
}