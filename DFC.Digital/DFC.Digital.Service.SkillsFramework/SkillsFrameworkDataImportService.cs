using System;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Service.SkillsFramework
{
    public class SkillsFrameworkDataImportService : IImportSkillsFrameworkDataService
    {
        private const string SummaryDetailsKey = "SummaryDetails";
        private const string ActionDetailsKey = "ActionDetails";
        private const string ErrorDetailsKey = "ErrorDetails";
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

        public FrameworkSkillsImportResponse ImportFrameworkSkills()
        {
            // this will be async once integrated
            var onetSkills = skillsFrameworkService.GetAllTranslations().ToList();
            var allOnetSkills = frameworkSkillRepository.GetFrameworkSkills().Count();
           
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allOnetSkills} translated frameworkskills in the Sitefinity");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {onetSkills.Count} skill translations to import");

            foreach (var onetSkill in onetSkills)
            {
                frameworkSkillRepository.UpsertFrameworkSkill(onetSkill);
                reportAuditRepository.CreateAudit(ActionDetailsKey, $"Added/Updated {onetSkill.Title} to repository");
            }

            return new FrameworkSkillsImportResponse {Success = true};
        }

        public UpdateSocOccupationalCodeResponse UpdateSocCodesOccupationalCode()
        {
            var allSocCodes = jobProfileSocCodeRepository.GetLiveSocCodes().ToList();
            var occupationalCodeMappings = skillsFrameworkService.GetAllSocMappings();
                      
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allSocCodes.Count} SOCs in the Sitefinity ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {occupationalCodeMappings.Count()} socOccupation Code Mappings from Framework Service");
            var updatedCount = 0;
            var totalRecordsCount = 0;
            var noActionCount = 0;

            foreach (var socCode in allSocCodes)
            {
                var occupationalCode = occupationalCodeMappings.FirstOrDefault(occmap => occmap.SOCCode.Equals(socCode.SOCCode, StringComparison.OrdinalIgnoreCase))
                    ?.ONetOccupationalCode;

                var auditMessage = $"{(totalRecordsCount++).ToString("0000")} - Got occupational code [{occupationalCode}] for SOC : {socCode.SOCCode} from SkillFramework Service ";

                //if the existing ocupational code is not the same update it.
                if (socCode.ONetOccupationalCode?.Equals(occupationalCode, StringComparison.OrdinalIgnoreCase) == false)
                {
                    socCode.ONetOccupationalCode = occupationalCode;
                    jobProfileSocCodeRepository.UpdateSocOccupationalCode(socCode);
                    auditMessage += " - Updated";
                    updatedCount++;
                }
                else
                {
                    auditMessage += " - No action as it already the same";
                    noActionCount++;
                }

                reportAuditRepository.CreateAudit(ActionDetailsKey, auditMessage);
            }
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Total number checked : {totalRecordsCount} updated : {updatedCount} no action : {noActionCount}");

            return new UpdateSocOccupationalCodeResponse {Success = true};
        }

        public UpdateJpSocSkillMatrixResponse ImportForSoc(string jobProfileSoc)
        {
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Updating Job profiles for SOC - {jobProfileSoc}");

            var soc = jobProfileSocCodeRepository.GetLiveSocCodes().FirstOrDefault(s => s.SOCCode == jobProfileSoc);
            if(soc == null)
            {
                reportAuditRepository.CreateAudit(SummaryDetailsKey, $"SOC - {jobProfileSoc} NOT found!");
            }

            var jobProfilesForSoc = jobProfileSocCodeRepository.GetLiveJobProfilesBySocCode(soc.SOCCode).ToList();
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobProfilesForSoc.Count()} profile for SOC");

            //We have job linked to the SOC
            if (jobProfilesForSoc?.Count() > 0)
            {
                //Create SOC skill matrix records  
                var occupationSkills = skillsFrameworkService.GetRelatedSkillMapping(soc.ONetOccupationalCode);
                reportAuditRepository.CreateAudit(ActionDetailsKey, $"Found {occupationSkills.Count()} Skills for {soc.ONetOccupationalCode} from SkillFramework Service");
                var rankGenerated = 1;
                foreach (var occupationSkill in occupationSkills)
                {
                    var socSkillToAdd = new SocSkillMatrix
                    {
                        Title = $"{soc.SOCCode}-{occupationSkill.Name}",
                        SocCode = soc.SOCCode,
                        Skill = occupationSkill.Name,
                        ONetRank = occupationSkill.Score,
                        Rank = rankGenerated,
                        ONetElementId = occupationSkill.Id
                    };
                    socSkillMatrixRepository.UpsertSocSkillMatrix(socSkillToAdd);
                    reportAuditRepository.CreateAudit(ActionDetailsKey, $"Added/Updated Soc Skill Matrix profile {socSkillToAdd.Title}  into socskill matrix repo");
                    rankGenerated++;
                }

                var socSkillMatrixData = jobProfileSocCodeRepository.GetSocSkillMatricesBySocCode(jobProfileSoc);

                var digitalSkillLevel = skillsFrameworkService.GetDigitalSkillLevel(soc.ONetOccupationalCode);
                var auditMessage = $"Got {digitalSkillLevel} for Occupational Code : {soc.ONetOccupationalCode} from SkillFramework Service";
                var digitSkillValue = Convert.ToInt32(digitalSkillLevel).ToString();

                foreach (var profile in jobProfilesForSoc)
                {
                    if (socSkillMatrixData.Any())
                    {
                        profile.DigitalSkillsLevel = digitSkillValue;
                        jobProfileRepository.UpdateSocSkillMatrices(profile, socSkillMatrixData.ToList());
                        reportAuditRepository.CreateAudit(ActionDetailsKey, $"Linked Job Profile {profile.UrlName} with the following socskilmatrices {string.Join(", ", socSkillMatrixData.ToList().Select(sk => sk.Title))}");
                    }
                }
            }
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Updated job profiles SOC {jobProfileSoc}");
            return new UpdateJpSocSkillMatrixResponse { Success = true };
        }
    }
}