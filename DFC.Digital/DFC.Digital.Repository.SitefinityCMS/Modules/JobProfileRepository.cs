using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Data.Model.OrchardCore;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using DFC.Digital.Repository.SitefinityCMS.OrchardCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public class JobProfileRepository : IJobProfileRepository
    {
        #region Fields

        private const string RelatedSkillField = "RelatedSkills";
        private const string UpdateComment = "Updated via the SkillsFramework import process";
        private static readonly OrchardCoreIdGenerator OrchardCoreIdGenerator = new OrchardCoreIdGenerator();
        private readonly IDynamicModuleRepository<JobProfile> repository;
        private readonly IDynamicModuleRepository<SocSkillMatrix> socSkillRepository;
        private readonly IDynamicModuleConverter<JobProfile> converter;
        private readonly IDynamicModuleConverter<JobProfileOverloadForWhatItTakes> converterForWITOnly;
        private readonly IDynamicModuleConverter<JobProfileOverloadForSearch> converterForSearchableFieldsOnly;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private Dictionary<string, JobProfile> cachedJobProfiles = new Dictionary<string, JobProfile>();

        #endregion Fields

        #region Ctor

        public JobProfileRepository(
            IDynamicModuleRepository<JobProfile> repository,
            IDynamicModuleConverter<JobProfile> converter,
            IDynamicContentExtensions dynamicContentExtensions,
            IDynamicModuleRepository<SocSkillMatrix> socSkillRepository,
            IDynamicModuleConverter<JobProfileOverloadForWhatItTakes> converterLight,
            IDynamicModuleConverter<JobProfileOverloadForSearch> converterForSearchableFieldsOnly)
        {
            this.repository = repository;
            this.converter = converter;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.socSkillRepository = socSkillRepository;
            this.converterForWITOnly = converterLight;
            this.converterForSearchableFieldsOnly = converterForSearchableFieldsOnly;
        }

        #endregion Ctor

        #region IJobProfileRepository Implementations

        /// <summary>
        /// Returns a jobprofile for normal front end view.
        /// Only live profiles are returned.
        /// </summary>
        /// <param name="urlName">URL of the jobprofile to return</param>
        /// <returns>JobProfile</returns>
        public JobProfile GetByUrlName(string urlName)
        {
            var key = urlName.ToLower();
            if (!cachedJobProfiles.ContainsKey(key))
            {
                var jobProfile = ConvertDynamicContent(repository.Get(item =>
                    item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible == true));
                cachedJobProfiles.Add(key, jobProfile);
            }

            return cachedJobProfiles[key]?.IsImported == true ? null : cachedJobProfiles[key];
        }

        /// <summary>
        /// Returns a jobprofile for Preview mode to a user logged into the backed.
        /// Profiles that are not live are still returned
        /// </summary>
        /// <param name="urlName">URL of the jobprofile to return</param>
        /// <returns>JobProfile</returns>
        public JobProfile GetByUrlNameForPreview(string urlName)
        {
            return ConvertDynamicContent(repository.Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Temp));
        }

        public JobProfile GetByUrlNameForSearchIndex(string urlName, bool isPublishing)
        {
            var content = repository.Get(item => item.UrlName == urlName && item.Status == (isPublishing ? ContentLifecycleStatus.Master : ContentLifecycleStatus.Live));
            return converterForSearchableFieldsOnly.ConvertFrom(content);
        }

        public IEnumerable<JobProfileOverloadForWhatItTakes> GetLiveJobProfiles()
        {
            var jobProfiles = repository.GetMany(item => item.Status == ContentLifecycleStatus.Live && item.Visible).ToList();

            if (jobProfiles.Any())
            {
                var jobProfileOverloadForWhatItTakesList = new List<JobProfileOverloadForWhatItTakes>();
                foreach (var jobProfile in jobProfiles)
                {
                    jobProfileOverloadForWhatItTakesList.Add(converterForWITOnly.ConvertFrom(jobProfile));
                }

                return jobProfileOverloadForWhatItTakesList;
            }

            return Enumerable.Empty<JobProfileOverloadForWhatItTakes>();
        }

        public RepoActionResult UpdateSocSkillMatrices(JobProfileOverloadForWhatItTakes jobProfile, IEnumerable<SocSkillMatrix> socSkillMatrices)
        {
            var jobprofile = repository.Get(item =>
                item.UrlName == jobProfile.UrlName && item.Status == ContentLifecycleStatus.Live && item.Visible);

            var skillMatrices = socSkillMatrices as IList<SocSkillMatrix> ?? socSkillMatrices.ToList();
            if (jobprofile != null && skillMatrices.Any())
            {
                var master = repository.GetMaster(jobprofile);

                dynamicContentExtensions.DeleteRelatedFieldValues(master, RelatedSkillField);

                float ordinal = 1;
                foreach (var socSkillMatrix in skillMatrices)
                {
                    var relatedSocSkillItem = socSkillRepository.Get(d => d.Status == ContentLifecycleStatus.Master && d.UrlName == socSkillMatrix.SfUrlName);

                    if (relatedSocSkillItem != null)
                    {
                        dynamicContentExtensions.SetRelatedFieldValue(master, relatedSocSkillItem, RelatedSkillField, socSkillMatrix.Rank.HasValue ? (float)socSkillMatrix.Rank.Value : ordinal);
                    }

                    ordinal++;
                }

                dynamicContentExtensions.SetFieldValue(master, nameof(JobProfile.DigitalSkillsLevel), jobProfile.DigitalSkillsLevel);

                repository.Commit();

                repository.Update(master, UpdateComment);

                repository.Commit();
            }

            return new RepoActionResult { Success = true };
        }

        public Type GetContentType()
        {
            return repository.GetContentType();
        }

        public string GetProviderName()
        {
            return repository.GetProviderName();
        }

        public IEnumerable<JobProfile> GetAllJobProfiles()
        {
            var jobProfilesDynamicContentItems = repository.GetMany(item => item.Status == ContentLifecycleStatus.Live && item.Visible).ToList();

            if (jobProfilesDynamicContentItems.Any())
            {
                var jobProfiles = new List<JobProfile>();

                foreach (var jobProfilesDynamicContentItem in jobProfilesDynamicContentItems)
                {
                    jobProfiles.Add(ConvertDynamicContent(jobProfilesDynamicContentItem));
                }

                return jobProfiles;
            }

            return Enumerable.Empty<JobProfile>();
        }

        public JobProfile ConvertDynamicContent(DynamicContent dynamicContent)
        {
            if (dynamicContent != null)
            {
                return converter.ConvertFrom(dynamicContent);
            }

            return null;
        }

        public IEnumerable<JobProfileUrl> GetAllJobProfileUrls()
        {
            var jobProfilesDynamicContentItems = repository.GetMany(item => item.Status == ContentLifecycleStatus.Live && item.Visible).ToList();

            if (jobProfilesDynamicContentItems.Any())
            {
                var jobProfileUrls = new List<JobProfileUrl>();

                foreach (var jobProfilesDynamicContentItem in jobProfilesDynamicContentItems)
                {
                    jobProfileUrls.Add(ConvertFromToJobProfileUrl(jobProfilesDynamicContentItem));
                }

                return jobProfileUrls;
            }

            return Enumerable.Empty<JobProfileUrl>();
        }

        public JobProfileUrl ConvertFromToJobProfileUrl(DynamicContent content)
        {
            var jobProfileUrl = new JobProfileUrl
            {
                Id = dynamicContentExtensions.GetFieldValue<Guid>(content, nameof(JobProfileUrl.Id)),
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileUrl.Title)),
                UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileUrl.UrlName))
            };

            return jobProfileUrl;
        }

        public OcJobProfile GetJobProfileByUrlName(string urlName)
        {
            return ConvertDynamicContentToOcJobProfile(repository.Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible == true));
        }

        public OcJobProfile ConvertDynamicContentToOcJobProfile(DynamicContent content)
        {
            var relatedHiddenAlternativeTitles = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.HiddenAlternativeTitle);
            var relatedWorkingHoursDetails = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.WorkingHoursDetails);
            var relatedWorkingPattern = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.WorkingPattern);
            var relatedWorkingPatternDetails = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.WorkingPatternDetails);
            var relatedJobProfileSpecialism = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.JobProfileSpecialism);
            var relatedUniversityEntryRequirements = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.UniversityEntryRequirements);
            var relatedCollegeEntryRequirements = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.CollegeEntryRequirements);
            var relatedApprenticeshipEntryRequirements = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, ItemTypes.ApprenticeshipEntryRequirements);
            var relatedJobProfileCategories = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, SitefinityFields.RelatedJobProfileCategories);

            var relatedUniversityRequirements = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedUniversityRequirement);
            var relatedUniversityLinks = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedUniversityLinks);
            var relatedCollegeRequirements = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedCollegeRequirements);
            var relatedCollegeLinks = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedCollegeLinks);
            var relatedApprenticeshipRequirements = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedApprenticeshipRequirements);
            var relatedApprenticeshipLinks = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedApprenticeshipLinks);
            var relatedRestrictions = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedRestrictions);
            var relatedLocations = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedLocations);
            var relatedEnvironments = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedEnvironments);
            var relatedUniforms = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedUniforms);
            var relatedCareerProfiles = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedCareerProfiles);
            var relatedSOC = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.SOC);
            var relatedRegistrations = dynamicContentExtensions.GetRelatedItemsIds(content, SitefinityFields.RelatedRegistrations);

            var ocJobProfile = new OcJobProfile
            {
                SitefinityId = dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.Id),
                ContentItemId = OrchardCoreIdGenerator.GenerateUniqueId(),
                ContentType = ItemTypes.JobProfile,
                DisplayText = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title),
                Latest = true,
                Published = true,
                ModifiedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.LastModified),
                PublishedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.PublicationDate),
                CreatedUtc = dynamicContentExtensions.GetFieldValue<DateTime>(content, SitefinityFields.DateCreated),
                TitlePart = new Titlepart() { Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Title) },
                JobProfile = new Jobprofile()
                {
                    AlternativeTitle = new Alternativetitle() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.AlternativeTitle)) },
                    WidgetContentTitle = new Widgetcontenttitle() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.WidgetContentTitle)) },
                    Overview = new Overview() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.Overview)) },
                    Salarystarterperyear = new Salarystarterperyear() { Value = (float)dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfile.SalaryStarter)) },
                    Salaryexperiencedperyear = new Salaryexperiencedperyear() { Value = (float)dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfile.SalaryExperienced)) },
                    Minimumhours = new Minimumhours() { Value = (float)dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfile.MinimumHours)) },
                    Maximumhours = new Maximumhours() { Value = (float)dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfile.MaximumHours)) },
                    HiddenAlternativeTitleSf = relatedHiddenAlternativeTitles?.ToList(),
                    HiddenAlternativeTitle = new HiddenalternativetitleIds(),
                    WorkingHoursDetailsSf = relatedWorkingHoursDetails?.ToList(),
                    WorkingHoursDetails = new Workinghoursdetails(),
                    WorkingPatternSf = relatedWorkingPattern?.ToList(),
                    WorkingPattern = new Workingpattern(),
                    WorkingPatternDetailsSf = relatedWorkingPatternDetails?.ToList(),
                    WorkingPatternDetails = new Workingpatterndetails(),
                    JobProfileSpecialismSf = relatedJobProfileSpecialism?.ToList(),
                    JobProfileSpecialism = new JobprofilespecialismIds(),
                    Entryroutes = new Entryroutes() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.EntryRoutes) },
                    Universityrelevantsubjects = new Universityrelevantsubjects() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.UniversityRelevantSubjects) },
                    Universityfurtherrouteinfo = new Universityfurtherrouteinfo() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.UniversityFurtherRouteInfo) },
                    UniversityEntryRequirementsSf = relatedUniversityEntryRequirements?.ToList(),
                    UniversityEntryRequirements = new UniversityentryrequirementsIds(),
                    RelatedUniversityRequirementsSf = relatedUniversityRequirements?.ToList(),
                    RelatedUniversityRequirements = new Relateduniversityrequirements(),
                    Collegerelevantsubjects = new Collegerelevantsubjects() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.CollegeRelevantSubjects) },
                    Collegefurtherrouteinfo = new Collegefurtherrouteinfo() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.CollegeFurtherRouteInfo) },
                    RelatedUniversityLinksSf = relatedUniversityLinks?.ToList(),
                    RelatedUniversityLinks = new Relateduniversitylinks(),
                    CollegeEntryRequirementsSf = relatedCollegeEntryRequirements?.ToList(),
                    CollegeEntryRequirements = new CollegeentryrequirementsIds(),
                    RelatedCollegeRequirementsSf = relatedCollegeRequirements?.ToList(),
                    RelatedCollegeRequirements = new Relatedcollegerequirements(),
                    RelatedCollegeLinksSf = relatedCollegeLinks?.ToList(),
                    RelatedCollegeLinks = new Relatedcollegelinks(),
                    Apprenticeshiprelevantsubjects = new Apprenticeshiprelevantsubjects() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.ApprenticeshipRelevantSubjects) },
                    Apprenticeshipfurtherroutesinfo = new Apprenticeshipfurtherroutesinfo() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.ApprenticeshipFurtherRouteInfo) },
                    ApprenticeshipEntryRequirementsSf = relatedApprenticeshipEntryRequirements?.ToList(),
                    ApprenticeshipEntryRequirements = new ApprenticeshipentryrequirementsIds(),
                    RelatedApprenticeshipRequirementsSf = relatedApprenticeshipRequirements?.ToList(),
                    RelatedApprenticeshipRequirements = new Relatedapprenticeshiprequirements(),
                    RelatedApprenticeshipLinksSf = relatedApprenticeshipLinks?.ToList(),
                    RelatedApprenticeshipLinks = new Relatedapprenticeshiplinks(),
                    Work = new Work() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Work) },
                    Volunteering = new Volunteering() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.Volunteering) },
                    Directapplication = new Directapplication() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.DirectApplication) },
                    Otherroutes = new Otherroutes() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.OtherRoutes) },
                    Careertips = new Careertips() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.CareerTips) },
                    Furtherinformation = new Furtherinformation() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.FurtherInformation) },
                    RelatedrestrictionsSf = relatedRestrictions?.ToList(),
                    Relatedrestrictions = new Relatedrestrictions(),
                    Otherrequirements = new Otherrequirements() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.OtherRequirements) },
                    DigitalSkills = new Digitalskills() { ContentItemIds = GetDigitalSkill(dynamicContentExtensions.GetFieldChoiceLabel(content, nameof(SitefinityFields.DigitalSkillsLevel))) },
                    Relatedskills = new Relatedskills() { ContentItemIds = new string[] { } },
                    JobProfileCategorySf = relatedJobProfileCategories?.ToList(),
                    JobProfileCategory = new JobprofilecategoryIds(),
                    Daytodaytasks = new Daytodaytasks() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.WYDDayToDayTasks) },
                    RelatedLocationsSf = relatedLocations?.ToList(),
                    RelatedLocations = new Relatedlocations(),
                    RelatedEnvironmentsSf = relatedEnvironments?.ToList(),
                    RelatedEnvironments = new Relatedenvironments(),
                    RelatedUniformsSf = relatedUniforms?.ToList(),
                    RelatedUniforms = new Relateduniforms(),
                    Coursekeywords = new Coursekeywords() { Text = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.CourseKeywords) },
                    Careerpathandprogression = new Careerpathandprogression() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.CareerPathAndProgression) },
                    RelatedcareerprofilesSf = relatedCareerProfiles?.ToList(),
                    Relatedcareerprofiles = new Relatedcareerprofiles() { ContentItemIds = new string[] { } },
                    SOCCodeSf = relatedSOC?.ToList(),
                    SOCCode = new SoccodeIds(),
                    RelatedRegistrationsSf = relatedRegistrations?.ToList(),
                    RelatedRegistrations = new Relatedregistrations(),
                    Professionalandindustrybodies = new Professionalandindustrybodies() { Html = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.ProfessionalAndIndustryBodies) },
                    DynamicTitlePrefix = new Dynamictitleprefix() { ContentItemIds = GetDynamicTitlePrefix(dynamicContentExtensions.GetFieldChoiceLabel(content, nameof(SitefinityFields.DynamicTitlePrefix))) }
                },
                PreviewPart = new Previewpart() { }, //????
                PageLocationPart = new Pagelocationpart() { UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.UrlName), DefaultPageForLocation = false, RedirectLocations = null, FullUrl = $"/{dynamicContentExtensions.GetFieldValue<Lstring>(content, SitefinityFields.UrlName)}" },
                SitemapPart = new Sitemappart() { OverrideSitemapConfig = false, ChangeFrequency = 0, Priority = 5, Exclude = false },
                ContentApprovalPart = new Contentapprovalpart { ReviewStatus = 0, ReviewType = 0, IsForcePublished = false },
                GraphSyncPart = new Graphsyncpart() { Text = $"<<contentapiprefix>>/jobprofile/{dynamicContentExtensions.GetFieldValue<Guid>(content, SitefinityFields.OriginalContentId)}" },
                AuditTrailPart = new Audittrailpart() { Comment = "Initial version imported by Migration Tool.", ShowComment = false }
            };

            return ocJobProfile;
        }

        private string[] GetDigitalSkill(string digitalSkill)
        {
            string digitalSkillOrchardCoreId = string.Empty;
            switch (digitalSkill.Trim().ToLowerInvariant())
            {
                case DigitalSkills.ComputerSystemsAndApplications:
                    digitalSkillOrchardCoreId = "4nbq6e0zgpcnj0r4jqn89mcq55";
                    break;
                case DigitalSkills.SoftwarePackages:
                    digitalSkillOrchardCoreId = "4jewsa5k17pr75c1ptm9k7xkng";
                    break;
                case DigitalSkills.ComputerAndTheMainSoftwarePackages:
                    digitalSkillOrchardCoreId = "4k0twbe5kxane7x548sc730pxz";
                    break;
                case DigitalSkills.BasicTasksOnComputer:
                    digitalSkillOrchardCoreId = "42ek1wf6g6k7479ygtb8tzqhnm";
                    break;
                default:
                    digitalSkillOrchardCoreId = "4pggmxezyz04cxqhtm89cvf8x3";
                    break;
            }

            return new string[] { digitalSkillOrchardCoreId };
        }

        private string[] GetDynamicTitlePrefix(string dynamicTitlePrefix)
        {
            string dynamicTitlePrefixOrchardCoreId = string.Empty;
            switch (dynamicTitlePrefix.Trim().ToLowerInvariant())
            {
                case DynamicTitlePrefixes.PrefixWithA:
                    dynamicTitlePrefixOrchardCoreId = "45stsxyyd5wxcygw4mkcr3g05b";
                    break;
                case DynamicTitlePrefixes.PrefixWithAn:
                    dynamicTitlePrefixOrchardCoreId = "4bntcf1j07w6b3ptj63f1vxsm1";
                    break;
                case DynamicTitlePrefixes.NoPrefix:
                    dynamicTitlePrefixOrchardCoreId = "4avc9zyzc0h482tvn9kqjps253";
                    break;
                case DynamicTitlePrefixes.NoTitle:
                    dynamicTitlePrefixOrchardCoreId = "41dd2nk0h3jqa0fgce4aq0y2c5";
                    break;
                case DynamicTitlePrefixes.AsDefined:
                    dynamicTitlePrefixOrchardCoreId = "4efw08fyykqqe7qn7jqtcvtdch";
                    break;
                default:
                    dynamicTitlePrefixOrchardCoreId = "4cesm5r8t0aa827bp3gn4pgsxx";
                    break;
            }

            return new string[] { dynamicTitlePrefixOrchardCoreId };
        }

        #endregion IJobProfileRepository Implementations
    }
}