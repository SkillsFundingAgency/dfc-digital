using System;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Service.SkillsFrameworkData
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
           
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allOnetSkills} Frameworkskills in the repo");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {onetSkills.Count} SkillFramework skills to import");

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
            var socCodesToUpdate = allSocCodes.Where(soc => string.IsNullOrWhiteSpace(soc.ONetOccupationalCode));
            var socCodesNotUpdated = allSocCodes.Count(soc => string.IsNullOrWhiteSpace(soc.ONetOccupationalCode));
            var socCodesUpdated = allSocCodes.Count(soc => !string.IsNullOrWhiteSpace(soc.ONetOccupationalCode));
            var occupationalCodeMappings = skillsFrameworkService.GetAllSocMappings();

          
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allSocCodes.Count} socs in the repo ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socCodesUpdated} socs already updated with occupational codes ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socCodesNotUpdated} socs not yet updated with occupational codes ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {occupationalCodeMappings.Count()} socOccupation Code Mappings from Framework Service");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Going to update {socCodesToUpdate.Count()} socs with occupational code ");

            var updatedSocs = new List<string>();
            foreach (var socCode in socCodesToUpdate)
            {
                var occupationalCode = occupationalCodeMappings
                    .FirstOrDefault(occmap => occmap.SOCCode.Equals(socCode.SOCCode, StringComparison.OrdinalIgnoreCase))
                    ?.ONetOccupationalCode;
                reportAuditRepository.CreateAudit(ActionDetailsKey, $"Found {occupationalCode} for SocCocde : {socCode.SOCCode} from SkillFramework Service");

                if (!string.IsNullOrWhiteSpace(occupationalCode) &&
                    !socCode.ONetOccupationalCode.Equals(occupationalCode, StringComparison.OrdinalIgnoreCase))
                {
                    socCode.ONetOccupationalCode = occupationalCode;
                    jobProfileSocCodeRepository.UpdateSocOccupationalCode(socCode);
                    reportAuditRepository.CreateAudit(ActionDetailsKey, $"Updated Soc code {socCode.SOCCode} with occupational code : {occupationalCode}");
                    updatedSocs.Add($"{socCode.SOCCode}-{socCode.ONetOccupationalCode}");
                }
            }

            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Used the following {updatedSocs.Count} soc code/onet occupational code combination {string.Join(", ", updatedSocs)} to update soc codes repo");

            return new UpdateSocOccupationalCodeResponse {Success = true};
        }

        public UpdateJpDigitalSkillsResponse UpdateJobProfilesDigitalSkills()
        {
            var allJobProfiles = jobProfileRepository.GetLiveJobProfiles().ToList();
            var jobprofilesToUpdate = allJobProfiles.Where(jp => string.IsNullOrWhiteSpace(jp.DigitalSkillsLevel) && !string.IsNullOrWhiteSpace(jp.ONetOccupationalCode)).ToList();
            var jobProfilesUpdated = allJobProfiles.Count(jp => !string.IsNullOrWhiteSpace(jp.DigitalSkillsLevel));
            var jobProfileswithoutDigitalSkills = allJobProfiles.Count(jp => string.IsNullOrWhiteSpace(jp.DigitalSkillsLevel));

            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allJobProfiles.Count} job profiles in the repo");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found a total of {jobProfileswithoutDigitalSkills} job profiles without an SkillFramework Digital skill");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobProfilesUpdated} job profiles already updated with SkillFramework Digital skill");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Going to update {jobprofilesToUpdate.Count} job profiles with an SkillFramework Digital skill");

            foreach (var jobProfile in jobprofilesToUpdate)
            {
                if (!string.IsNullOrWhiteSpace(jobProfile.ONetOccupationalCode))
                {
                    var digitalSkillLevel = skillsFrameworkService.GetDigitalSkillLevel(jobProfile.ONetOccupationalCode);

                    reportAuditRepository.CreateAudit(ActionDetailsKey, $"Found {digitalSkillLevel} for Occupational Code : {jobProfile.ONetOccupationalCode} from SkillFramework Service");

                    var digitSkillValue = Convert.ToInt32(digitalSkillLevel).ToString();
                    if (string.IsNullOrWhiteSpace(jobProfile.DigitalSkillsLevel) || !jobProfile.DigitalSkillsLevel.Equals(digitSkillValue, StringComparison.OrdinalIgnoreCase))
                    {
                        jobProfile.DigitalSkillsLevel = digitSkillValue;
                        jobProfileRepository.UpdateDigitalSkill(jobProfile);
                        reportAuditRepository.CreateAudit(ActionDetailsKey, $"Updated Job profile {jobProfile.UrlName} with digital skill level : {digitalSkillLevel}");
                    }
                }
            }

            return new UpdateJpDigitalSkillsResponse {Success = true};
        }

        public BuildSocMatrixResponse BuildSocMatrixData()
        {
            var socSkillMatrices = socSkillMatrixRepository.GetSocSkillMatrices().ToList();
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socSkillMatrices.Count} soc skill matrices in the repo");

            var importedSocs = socSkillMatrices.Select(socSkil => socSkil.Title.ToLowerInvariant()).Distinct().ToList();
            var importSocCount = socSkillMatrices.Select(socSkil => socSkil.SocCode.ToLowerInvariant()).ToList();
            var allSocCodes = jobProfileSocCodeRepository.GetLiveSocCodes().ToList();
            var socCodesToUpdate = allSocCodes.Where(sc => !string.IsNullOrWhiteSpace(sc.ONetOccupationalCode) && !string.IsNullOrWhiteSpace(sc.SOCCode) && importSocCount.Count(x => x.Equals(sc.SOCCode)) < 20).ToList();
            var socCodesWithRelatedSocSkills = allSocCodes.Count(sc => !string.IsNullOrWhiteSpace(sc.ONetOccupationalCode) && importSocCount.Count(x => x.Equals(sc.SOCCode)) >= 20);
            var socCodesWithoutRelatedSocs = allSocCodes.Count(sc => !string.IsNullOrWhiteSpace(sc.ONetOccupationalCode) && importSocCount.Count(x => x.Equals(sc.SOCCode)) < 20);
            var socCodesWithoutOcCode = allSocCodes.Where(sc => string.IsNullOrWhiteSpace(sc.ONetOccupationalCode));

            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allSocCodes.Count()} soc codes  in the repo  ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socCodesWithRelatedSocSkills} soc codes with 20 skill matrices in the repo");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {string.Join(", ", importedSocs)} soc codes in the skill matrix repo  ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socCodesWithoutRelatedSocs} soc codes without 20 skill matrices in the repo, based on soc Code");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {socCodesWithoutOcCode.Count()} soc codes without coccupational codes with the following urlnames {string.Join(", ", socCodesWithoutOcCode.Select(jp => jp.UrlName))}");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Going to  use {socCodesToUpdate.Count()} soc codes to add soc skill matrices in the repo, based on soc Code");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Going to  use {string.Join(", ", socCodesToUpdate.Select(sc => sc.SOCCode))} socs to add soc skill matrices in the repo, based on soc Code");

            var addedSocs = new List<string>();
            foreach (var socCode in socCodesToUpdate)
            {
               
                if (!string.IsNullOrWhiteSpace(socCode.ONetOccupationalCode))
                {
                    var occupationSkills = skillsFrameworkService.GetRelatedSkillMapping(socCode.ONetOccupationalCode);

                    reportAuditRepository.CreateAudit(ActionDetailsKey, $"Found {string.Join(", ", occupationSkills.Select(oc => oc.Id))} skills : for occupation code {socCode.ONetOccupationalCode} from SkillFramework Service");
                        var rankGenerated = 1;
                    foreach (var occupationSkill in occupationSkills)
                    {
                        var socSkillToAdd = new SocSkillMatrix
                        {
                            Title = $"{socCode.SOCCode}-{occupationSkill.Name}",
                            SocCode = socCode.SOCCode,
                            Skill = occupationSkill.Name,
                            ONetRank = occupationSkill.Score,
                            Rank = rankGenerated
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


            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allJobProfiles.Count} jobprofiles  in the repo  ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobprofilesWithoutSkills} jobprofiles without related skill matrices in the repo");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobprofilesWithSkills} jobprofiles with related skill matrices in the repo");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {jobprofilesWithoutOcCode.Count()} jobprofiles without coccupational codes with the following urlnames {string.Join(", ", jobprofilesWithoutOcCode.Select(jp => jp.UrlName))}");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Going to update {jobprofilesToUpdate.Count()} jobprofiles with related skill matrices in the repo");

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