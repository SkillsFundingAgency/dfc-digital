using System;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.OnetService.Services.Contracts;

namespace DFC.Digital.Service.OnetService
{
    public class SkillsFrameworkDataImportService : IImportSkillsFrameworkDataService
    {
        private const string SummaryDetailsKey = "SummaryDetails";
        private const string ActionDetailsKey = "ActionDetails";
        private readonly ISkillsFrameworkService skillsFrameworkService;
        private readonly IFrameworkSkillRepository frameworkSkillRepository;
        private readonly ISocSkillMatrixRepository socSkillMatrixRepository;
        private readonly IJobProfileSocCodeRepository jobProfileSocCodeRepository;
        private readonly IJobProfileRepository jobProfileRepository;
        private readonly IReportAuditRepository reportAuditRepository;

        public SkillsFrameworkDataImportService(ISkillsFrameworkService skillsFrameworkService, IFrameworkSkillRepository frameworkSkillRepository, IJobProfileSocCodeRepository jobProfileSocCodeRepository, IJobProfileRepository jobProfileRepository, ISocSkillMatrixRepository socSkillMatrixRepository, IReportAuditRepository reportAuditRepository)
        {
            this.skillsFrameworkService = skillsFrameworkService;
            this.frameworkSkillRepository = frameworkSkillRepository;
            this.jobProfileSocCodeRepository = jobProfileSocCodeRepository;
            this.jobProfileRepository = jobProfileRepository;
            this.reportAuditRepository = reportAuditRepository;
            this.socSkillMatrixRepository = socSkillMatrixRepository;
        }

        public FrameworkSkillsImportResponse ImportOnetSkills()
        {
            // this will be async once integrated
            var onetSkills = skillsFrameworkService.GetOnetSkills();
            var allOnetSkills = frameworkSkillRepository.GetOnetSkills().Count();
            var response = new FrameworkSkillsImportResponse();
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allOnetSkills} SkillFrameworkskills in the repo");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {onetSkills.OnetSkillsList.Count()} SkillFramework skills to import");

            foreach (var onetSkill in onetSkills.OnetSkillsList)
            {
                frameworkSkillRepository.UpsertOnetSkill(onetSkill);
                reportAuditRepository.CreateAudit(ActionDetailsKey, $"Added/Updated {onetSkill.Title} to repository");
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
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allSocCodes.Count} socs in the repo <br />");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socCodesUpdated} socs already updated with occupational codes <br />");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socCodesNotUpdated} socs not yet updated with occupational codes <br />");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Going to update {socCodesToUpdate.Count()} socs with occupational code <br />");
 
            foreach (var socCode in socCodesToUpdate)
            {
                var occupationalCode = skillsFrameworkService.GetSocOccupationalCode(socCode.SOCCode);

                reportAuditRepository.CreateAudit(ActionDetailsKey, $"Found {occupationalCode} for SocCocde : {socCode.SOCCode} from SkillFramework Service  <br /> ");

                if (!string.IsNullOrWhiteSpace(occupationalCode) &&
                    !socCode.SOCCode.Equals(occupationalCode, StringComparison.OrdinalIgnoreCase))
                {
                    socCode.ONetOccupationalCode = occupationalCode;
                    jobProfileSocCodeRepository.UpdateSocOccupationalCode(socCode);
                    reportAuditRepository.CreateAudit(ActionDetailsKey, $"Updated Soc code {socCode.SOCCode} with occupational code : {occupationalCode}  <br /> ");
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

            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allJobProfiles.Count} job profiles in the repo  <br /> ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found a total of {jobProfileswithoutDigitalSkills} job profiles without an SkillFramework Digital skill  <br /> ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobProfilesUpdated} job profiles already updated with SkillFramework Digital skill  <br /> ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Going to update {jobprofilesToUpdate.Count} job profiles with an SkillFramework Digital skill  <br /> ");

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                if (!string.IsNullOrWhiteSpace(jobProfile.ONetOccupationalCode))
                {
                    var digitalSkillLevel = skillsFrameworkService.GetDigitalSkillLevel(jobProfile.ONetOccupationalCode);

                    reportAuditRepository.CreateAudit(ActionDetailsKey, $"Found {digitalSkillLevel} for Occupational Code : {jobProfile.ONetOccupationalCode} from SkillFramework Service  <br /> ");

                    if (!string.IsNullOrWhiteSpace(digitalSkillLevel) && (string.IsNullOrWhiteSpace(jobProfile.DigitalSkillsLevel) ||
                        !jobProfile.DigitalSkillsLevel.Equals(digitalSkillLevel, StringComparison.OrdinalIgnoreCase)))
                    {
                        jobProfile.DigitalSkillsLevel = digitalSkillLevel;
                        jobProfileRepository.UpdateDigitalSkill(jobProfile);
                        reportAuditRepository.CreateAudit(ActionDetailsKey, $"Updated Job profile {jobProfile.UrlName} with digital skill level : {digitalSkillLevel}  <br /> ");
                    }
                }
            }

            response.Success = true;
            return response;
        }

        public BuildSocMatrixResponse BuildSocMatrixData()
        {
            var response = new BuildSocMatrixResponse();
            var socSkillMatrices = socSkillMatrixRepository.GetSocSkillMatrices();
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socSkillMatrices.Count()} soc skill matrices in the repo  <br /> ");

            var importedSocs = socSkillMatrices.Select(socSkil => socSkil.SocCode).Distinct().ToList();
            var allJobProfiles = jobProfileRepository.GetLiveJobProfiles().ToList();
            var jobprofilesToUpdate = allJobProfiles.Where(jp => !string.IsNullOrWhiteSpace(jp.ONetOccupationalCode) && !importedSocs.Contains(jp.SOCCode)).Take(50);
            var jobprofilesWithRelatedSocs = allJobProfiles.Count(jp => !string.IsNullOrWhiteSpace(jp.ONetOccupationalCode) && importedSocs.Contains(jp.SOCCode));
            var jobprofilesWithoutRelatedSocs = allJobProfiles.Count(jp => !string.IsNullOrWhiteSpace(jp.ONetOccupationalCode) && !importedSocs.Contains(jp.SOCCode));

            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allJobProfiles.Count} jobprofiles  in the repo <br /> ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobprofilesWithRelatedSocs} jobprofiles with skill matrices in the repo, based on soc Code  <br /> ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {string.Join(",", importedSocs)} soc codes in the skill matrix repo <br /> ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobprofilesWithoutRelatedSocs} jobprofiles without skill matrices in the repo, based on soc Code  <br /> ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Going to  use {jobprofilesToUpdate.Count()} jobprofiles to add soc skill matrices in the repo, based on soc Code  <br /> ");

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                if (!string.IsNullOrWhiteSpace(jobProfile.ONetOccupationalCode))
                {
                    var occupationSkills = skillsFrameworkService.GetOccupationalCodeSkills(jobProfile.ONetOccupationalCode);

                    reportAuditRepository.CreateAudit(ActionDetailsKey, $"Found {string.Join(",", occupationSkills.Select(oc => oc.Title))} skills : for occupation code {jobProfile.ONetOccupationalCode} from SkillFramework Service  <br /> ");
                        var rankGenerated = 1;
                    foreach (var occupationSkill in occupationSkills)
                    {
                        socSkillMatrixRepository.UpsertSocSkillMatrix(new SocSkillMatrix
                        {
                            Title = $"{jobProfile.SOCCode}-{occupationSkill.Title}",
                            SocCode = jobProfile.SOCCode,
                            Skill = occupationSkill.Title,
                            ONetRank = occupationSkill.OnetRank,
                            Rank = rankGenerated
                        });
                        reportAuditRepository.CreateAudit(ActionDetailsKey, $"Added/Updated Sco Skill Matrix profile {jobProfile.SOCCode}-{occupationSkill.Title}  into socskill matrix repo  <br /> ");
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

            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allJobProfiles.Count} jobprofiles  in the repo <br /> ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobprofilesWithoutSkills} jobprofiles without related skill matrices in the repo  <br /> ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobprofilesWithSkills} jobprofiles with related skill matrices in the repo  <br /> ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Going to update {jobprofilesToUpdate.Count()} jobprofiles with related skill matrices in the repo  <br /> ");

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                var socSkillMatrixData = jobProfileSocCodeRepository.GetSocSkillMatricesBySocCode(jobProfile.SOCCode);

                reportAuditRepository.CreateAudit(ActionDetailsKey, $"Found {string.Join(",", socSkillMatrixData.Select(oc => oc.Title))} soc skills : for job profile soccode {jobProfile.SOCCode} from SkillFramework repository  <br /> ");
                if (socSkillMatrixData.Any())
                {
                    jobProfileRepository.UpdateSocSkillMatrices(jobProfile, socSkillMatrixData.ToList());
                    reportAuditRepository.CreateAudit(ActionDetailsKey, $"Updated Job Profile {jobProfile.UrlName} with the following socskilmatrices {string.Join(",", socSkillMatrixData.ToList().Select(sk => sk.Title))}  <br /> ");
                }
            }

            response.Success = true;
            return response;
        }
    }
}