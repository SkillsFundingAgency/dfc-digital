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

            return new FrameworkSkillsImportResponse { Success = true };
        }

        public UpdateSocOccupationalCodeResponse UpdateSocCodesOccupationalCode()
        {
            var allSocCodes = jobProfileSocCodeRepository.GetSocCodes().ToList();
            var occupationalCodeMappings = skillsFrameworkService.GetAllSocMappings();

            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allSocCodes.Count} SOCs in the Sitefinity ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {occupationalCodeMappings.Count()} socOccupation Code Mappings from Framework Service");
            var updatedCount = 0;
            var totalRecordsCount = 0;
            var noActionCount = 0;

            foreach (var socCode in allSocCodes)
            {
                var occupationalCode = occupationalCodeMappings.FirstOrDefault(occmap => occmap.SOCCode.Equals(socCode.SOCCode, StringComparison.OrdinalIgnoreCase))
                    ?.ONetOccupationalCode ?? string.Empty;

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

            return new UpdateSocOccupationalCodeResponse { Success = true };
        }

        public SkillsServiceResponse ImportForSocs(string jobProfileSocs)
        {
            if (string.IsNullOrWhiteSpace(jobProfileSocs))
            {
                throw new ArgumentNullException(nameof(jobProfileSocs));
            }

            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Updating Job profiles for SOC(s) - {jobProfileSocs}");
            var startingNumber = skillsFrameworkService.GetSocMappingStatus();
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Before Import  - SOCs awaiting import-{startingNumber.AwaitingUpdate} : SOCs completed-{startingNumber.UpdateCompleted} :  SOCs started but not completed-{startingNumber.SelectedForUpdate} ");

            var socs = jobProfileSocs.Split(',');
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Importing {socs.Count()} SOC(s)");
            foreach (var soc in socs)
            {
                ImportForSingleSoc(soc.Trim());
            }

            var endingNumber = skillsFrameworkService.GetSocMappingStatus();
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"After Import  - SOCs awaiting import-{endingNumber.AwaitingUpdate} : SOCs completed-{endingNumber.UpdateCompleted} :  SOCs started but not completed-{endingNumber.SelectedForUpdate} ");
            return new SkillsServiceResponse { Success = true };
        }

        public void ResetAllSocStatus()
        {
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Reseting status of all SOCs to awaiting import");
            skillsFrameworkService.ResetAllSocStatus();
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Status has been reset");

        }

        public void ResetStartedSocStatus()
        {
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Reseting status of all SOCs stuck in started import status");
            skillsFrameworkService.ResetStartedSocStatus();
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Status has been reset");
        }

        public SocMappingStatus GetSocMappingStatus()
        {
            return skillsFrameworkService.GetSocMappingStatus();
        }

        public string GetNextBatchOfSOCsToImport(int batchsize)
        {
            var Socs = skillsFrameworkService.GetNextBatchSocMappingsForUpdate(batchsize);
            return string.Join(",", Socs.ToList().Select(s => s.SOCCode));
        }

        public void ImportForSingleSoc(string jobProfileSoc)
        {
            var lockedJobProfiles = new List<JobProfileOverloadForWhatItTakes>();
            reportAuditRepository.CreateAudit(ActionDetailsKey, $"Updating Job profiles for SOC - {jobProfileSoc}");
            var soc = jobProfileSocCodeRepository.GetBySocCode(jobProfileSoc);
            if (soc == null)
            {
                reportAuditRepository.CreateAudit(ErrorDetailsKey, $"SOC - {jobProfileSoc} NOT found in Sitefinity!");
                soc = new SocCode {SOCCode = jobProfileSoc};
            }
            else
            {
                skillsFrameworkService.SetSocStatusSelectedForUpdate(soc);
                var jobProfilesForSoc = jobProfileSocCodeRepository.GetLiveJobProfilesBySocCode(soc.SOCCode).ToList();
                reportAuditRepository.CreateAudit(ActionDetailsKey,
                    $"Found {jobProfilesForSoc.Count} job profiles for SOC {soc.SOCCode}");

                lockedJobProfiles = jobProfilesForSoc.Where(x => x.Locked).ToList();
                foreach (var lockedJobProfile in lockedJobProfiles)
                {
                    reportAuditRepository.CreateAudit(SummaryDetailsKey, $"{lockedJobProfile.UrlName} could not be updated as it is locked or already in a Draft status with Soc code {jobProfileSoc}");
                }

                var safeJobProfiles = jobProfilesForSoc.Where(x => !x.Locked).ToList();
                //We have safe job linked to the SOC
                if (safeJobProfiles.Any())
                {
                    var socSkillMatrixData = CreateSocSkillsMatrixRecords(soc);
                    var digitalSkillLevel = skillsFrameworkService.GetDigitalSkillLevel(soc.ONetOccupationalCode);
                    reportAuditRepository.CreateAudit(ActionDetailsKey,
                        $"Got {digitalSkillLevel} for Occupational Code : {soc.ONetOccupationalCode} SOC {soc.SOCCode} from SkillFramework Service");
                    var digitSkillValue = Convert.ToInt32(digitalSkillLevel).ToString();

                    //DO all profiles that exist for this code
                    foreach (var profile in safeJobProfiles)
                    {
                        if (socSkillMatrixData.Any())
                        {
                            profile.DigitalSkillsLevel = digitSkillValue;
                            jobProfileRepository.UpdateSocSkillMatrices(profile, socSkillMatrixData);
                            reportAuditRepository.CreateAudit(ActionDetailsKey,
                                $"Linked Job Profile {profile.UrlName} with the following socskilmatrices {string.Join(", ", socSkillMatrixData.ToList().Select(sk => sk.Title))}");
                        }
                    }

                    reportAuditRepository.CreateAudit(ActionDetailsKey, $"Updated job profiles SOC {jobProfileSoc}");
                }
                else
                {
                    reportAuditRepository.CreateAudit(ErrorDetailsKey,
                        $"Found {safeJobProfiles.Count} unlocked job profiles for SOC {soc.SOCCode}");
                }

            }
            if (!lockedJobProfiles.Any())
            {
                skillsFrameworkService.SetSocStatusCompleted(soc);
                reportAuditRepository.CreateAudit(ActionDetailsKey, $"Set status to Completed for SOC {soc.SOCCode}");
            }
            reportAuditRepository.CreateAudit(ActionDetailsKey, "-----------------------------------------------------------------------------------------------------------------------------");
            reportAuditRepository.CreateAudit(ActionDetailsKey, " ");
        }

        public IList<SocSkillMatrix> CreateSocSkillsMatrixRecords(SocCode soc)
        {
            if (soc == null)
            {
                throw new ArgumentNullException(nameof(soc));
            }

            var occupationSkills = skillsFrameworkService.GetRelatedSkillMapping(soc.ONetOccupationalCode);
            //Create SOC skill matrix records  
            reportAuditRepository.CreateAudit(ActionDetailsKey, $"Found {occupationSkills.Count()} Skills for {soc.ONetOccupationalCode} SOC {soc.SOCCode} from SkillFramework Service");
            //Save it as an error as well
            if (occupationSkills.Count() == 0)
            {
                reportAuditRepository.CreateAudit(ErrorDetailsKey, $"Found {occupationSkills.Count()} Skills for {soc.ONetOccupationalCode} SOC {soc.SOCCode} from SkillFramework Service");
            }

            var rankGenerated = 1;
            var socSkillMatrixData = new List<SocSkillMatrix>();

            foreach (var occupationSkill in occupationSkills)
            {
                var socSkillToAdd = new SocSkillMatrix
                {
                    Title = $"{soc.SOCCode}-{occupationSkill.Name}",
                    SocCode = soc.SOCCode,
                    Skill = occupationSkill.Name,
                    ONetRank = occupationSkill.Score,
                    Rank = rankGenerated,
                    ONetElementId = occupationSkill.Id,
                    ONetAttributeType = occupationSkill.Category.ToString()
                };
                socSkillMatrixRepository.UpsertSocSkillMatrix(socSkillToAdd);
                socSkillMatrixData.Add(socSkillToAdd);
                reportAuditRepository.CreateAudit(ActionDetailsKey, $"Added/Updated Soc Skill Matrix profile {socSkillToAdd.Title}  into socskill matrix repo");
                rankGenerated++;
            }

            return socSkillMatrixData;
        }

        public UpdateSocOccupationalCodeResponse ExportNewONetMappings()
        {
            var allSocCodes = jobProfileSocCodeRepository.GetSocCodes().ToList();
            var occupationalCodeMappings = skillsFrameworkService.GetAllSocMappings();

            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {allSocCodes.Count} SOCs in the Sitefinity ");
            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {occupationalCodeMappings.Count()} socOccupation Code Mappings from Framework Service");

            var newSOCS = allSocCodes.Where(s => !occupationalCodeMappings.Any(o=> o.SOCCode == s.SOCCode)).Where(n => n.ONetOccupationalCode != string.Empty);

            reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Found {newSOCS.Count()} SOCs with ONet mappings that are not already in the ONet Framework Service");

            if (newSOCS.Count() > 0)
            {
                skillsFrameworkService.AddNewSOCMappings(newSOCS);
                reportAuditRepository.CreateAudit(SummaryDetailsKey, $"Added new SOCs to the ONet Framework Service");
            }

            return new UpdateSocOccupationalCodeResponse { Success = true };
        }
    }
}