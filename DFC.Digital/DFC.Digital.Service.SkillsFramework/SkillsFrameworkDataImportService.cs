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

        public UpdateJpDigitalSkillsResponse UpdateJobProfilesDigitalSkills()
        {
            var jobProfiles = jobProfileRepository.GetLiveJobProfiles().ToList();
            var totalJobProfilesCount = jobProfiles.Count();
            var profilesWithOccupationalCode = jobProfiles.Where(j => !string.IsNullOrWhiteSpace(j.ONetOccupationalCode)).ToList();
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {totalJobProfilesCount} job profiles  - {profilesWithOccupationalCode.Count} have an occupational code in Sitefinity");

            var updatedCount = 0;
            var totalRecordsCount = 0;
            var noActionCount = 0;

            foreach (var jobProfile in profilesWithOccupationalCode)
            {
                    var digitalSkillLevel = skillsFrameworkService.GetDigitalSkillLevel(jobProfile.ONetOccupationalCode);

                    var auditMessage = $"{(totalRecordsCount++).ToString("0000")} - Got {digitalSkillLevel} for Occupational Code : {jobProfile.ONetOccupationalCode} job profile {jobProfile.UrlName}  from SkillFramework Service";

                    var digitSkillValue = Convert.ToInt32(digitalSkillLevel).ToString();
                    if (jobProfile.DigitalSkillsLevel?.Equals(digitSkillValue, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        auditMessage += " - No action as it already the same";
                        noActionCount++;
                    }
                    else
                    {
                        jobProfile.DigitalSkillsLevel = digitSkillValue;
                        jobProfileRepository.UpdateDigitalSkill(jobProfile);
                        auditMessage += " - Updated";
                        updatedCount++;
                    }
                    reportAuditRepository.CreateAudit(ActionDetailsKey, auditMessage);
            }
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Total number checked : {totalRecordsCount} updated : {updatedCount} no action : {noActionCount}");

            return new UpdateJpDigitalSkillsResponse {Success = true};
        }

        public BuildSocMatrixResponse BuildSocMatrixData()
        {
            var socSkillMatrices = socSkillMatrixRepository.GetSocSkillMatrices().ToList();
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socSkillMatrices.Count} SOC skill matrix records in Sitefinity");

            var importedSocs = socSkillMatrices.Select(socSkil => socSkil.Title.ToLowerInvariant()).Distinct().ToList();
            var importSocCount = socSkillMatrices.Select(socSkil => socSkil.SocCode.ToLowerInvariant()).ToList();
            var allSocCodes = jobProfileSocCodeRepository.GetLiveSocCodes().ToList();
            var socCodesToUpdate = allSocCodes.Where(sc => !string.IsNullOrWhiteSpace(sc.ONetOccupationalCode) && !string.IsNullOrWhiteSpace(sc.SOCCode) && importSocCount.Count(x => x.Equals(sc.SOCCode)) < 5).Take(50).ToList();
            var socCodesWithRelatedSocSkills = allSocCodes.Count(sc => !string.IsNullOrWhiteSpace(sc.ONetOccupationalCode) && importSocCount.Count(x => x.Equals(sc.SOCCode)) >= 5);
            var socCodesWithoutRelatedSocs = allSocCodes.Count(sc => !string.IsNullOrWhiteSpace(sc.ONetOccupationalCode) && importSocCount.Count(x => x.Equals(sc.SOCCode)) < 5);
            var socCodesWithoutOcCode = allSocCodes.Where(sc => string.IsNullOrWhiteSpace(sc.ONetOccupationalCode));

            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allSocCodes.Count()} SOC records in the Sitefinity");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socCodesWithRelatedSocSkills} SOC records with 5 or more skill matrices in Sitefinity");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {string.Join(", ", importedSocs)} SOC records in the skill matrix in Sitefinity ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socCodesWithoutRelatedSocs} SOC records without 5 skill matrices in Sitefinity");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socCodesWithoutOcCode.Count()} SOC records without coccupational codes with the following urlnames {string.Join(", ", socCodesWithoutOcCode.Select(jp => jp.UrlName))}");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Going to  import for  {socCodesToUpdate.Count()} SOCs");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Going to import for these SOCs {string.Join(", ", socCodesToUpdate.Select(sc => sc.SOCCode))}");

            var addedSocs = new List<string>();
            foreach (var socCode in socCodesToUpdate)
            {
               
                if (!string.IsNullOrWhiteSpace(socCode.ONetOccupationalCode))
                {
                    var occupationSkills = skillsFrameworkService.GetRelatedSkillMapping(socCode.ONetOccupationalCode);

                    reportAuditRepository.CreateAudit(ActionDetailsKey, $"Found {string.Join(", ", occupationSkills.Select(oc => oc.Name))} skills : for occupation code {socCode.ONetOccupationalCode} from SkillFramework Service");
                        var rankGenerated = 1;
                    foreach (var occupationSkill in occupationSkills)
                    {
                        var socSkillToAdd = new SocSkillMatrix
                        {
                            Title = $"{socCode.SOCCode}-{occupationSkill.Name}",
                            SocCode = socCode.SOCCode,
                            Skill = occupationSkill.Name,
                            ONetRank = occupationSkill.Score,
                            Rank = rankGenerated,
                            ONetElementId = occupationSkill.Id
                        };
                        if (!importedSocs.Contains(socSkillToAdd.Title.ToLowerInvariant()))
                        {
                            try
                            {
                                socSkillMatrixRepository.UpsertSocSkillMatrix(socSkillToAdd);
                                reportAuditRepository.CreateAudit(ActionDetailsKey, $"Added/Updated Soc Skill Matrix profile {socSkillToAdd.Title}  into socskill matrix repo");
                                rankGenerated++;
                                if (!addedSocs.Contains(socCode.SOCCode))
                                {
                                    addedSocs.Add(socCode.SOCCode);
                                }

                            }
                            catch (Exception ex)
                            {
                                reportAuditRepository.CreateAudit(ErrorDetailsKey, $"Error adding  {socSkillToAdd.Title} into socskill matrix repo with exception {ex.Message}");
                            }
                        }
                       
                    }
                }
            }

            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Used the following {addedSocs.Count} soc codes {string.Join(", ", addedSocs)} to add soc skill matrices in the repo");
 
            return new BuildSocMatrixResponse {Success = true};
        }

        public UpdateJpSocSkillMatrixResponse UpdateJpSocSkillMatrix()
        {
           
            var allJobProfiles = jobProfileRepository.GetLiveJobProfiles().ToList();
            var jobprofilesToUpdate = allJobProfiles.Where(jp => !jp.HasRelatedSocSkillMatrices && !string.IsNullOrWhiteSpace(jp.ONetOccupationalCode)).Take(50);
            var jobprofilesWithSkills = allJobProfiles.Count(jp => jp.HasRelatedSocSkillMatrices);
            var jobprofilesWithoutSkills = allJobProfiles.Count(jp => !jp.HasRelatedSocSkillMatrices);
            var jobprofilesWithoutOcCode = allJobProfiles.Where(jp => string.IsNullOrWhiteSpace(jp.ONetOccupationalCode));


            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allJobProfiles.Count} jobprofiles  in Sitefinity");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobprofilesWithoutSkills} jobprofiles without related skills Sitefinity");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobprofilesWithSkills} jobprofiles have already got related skill in Sitefinity");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobprofilesWithoutOcCode.Count()} jobprofiles without coccupational codes with the following urlnames {string.Join(", ", jobprofilesWithoutOcCode.Select(jp => jp.UrlName))}");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Going to update {jobprofilesToUpdate.Count()} jobprofiles with related skill in Sitefinity");

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                var socSkillMatrixData = jobProfileSocCodeRepository.GetSocSkillMatricesBySocCode(jobProfile.SOCCode);

                reportAuditRepository.CreateAudit(ActionDetailsKey, $"Found {string.Join(", ", socSkillMatrixData.Select(oc => oc.Title))} soc skills : for job profile soccode {jobProfile.SOCCode} from SkillFramework repository");
                if (socSkillMatrixData.Any())
                {
                    jobProfileRepository.UpdateSocSkillMatrices(jobProfile, socSkillMatrixData.ToList());
                    reportAuditRepository.CreateAudit(ActionDetailsKey, $"Updated Job Profile {jobProfile.UrlName} with the following socskilmatrices {string.Join(", ", socSkillMatrixData.ToList().Select(sk => sk.Title))}");
                }
            }

            return new UpdateJpSocSkillMatrixResponse {Success = true};
        }
    }
}