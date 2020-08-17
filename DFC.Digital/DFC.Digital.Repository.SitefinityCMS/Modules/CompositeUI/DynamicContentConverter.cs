using DFC.Digital.Core;
using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Repository.SitefinityCMS.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.OpenAccess;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.GenericContent;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class DynamicContentConverter : IDynamicModuleConverter<JobProfileMessage>
    {
        #region Fields

        private const string RelatedSkillsField = "RelatedSkills";
        private const string RelatedInterestsField = "RelatedInterests";
        private const string RelatedEnablersField = "RelatedEnablers";
        private const string RelatedEntryQualificationsField = "RelatedEntryQualifications";
        private const string RelatedTrainingRoutesField = "RelatedTrainingRoutes";
        private const string RelatedJobProfileCategoriesField = "JobProfileCategories";
        private const string RelatedJobAreasField = "RelatedJobAreas";
        private const string RelatedPreferredTaskTypesField = "RelatedPreferredTaskTypes";
        private const string OtherRequirementsField = "OtherRequirements";
        private const string RelatedRestrictionsField = "RelatedRestrictions";
        private const string RelatedSkillField = "RelatedSkill";

        private readonly IRelatedClassificationsRepository relatedClassificationsRepository;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly IContentPropertyConverter<HowToBecomeData> htbContentPropertyConverter;
        private readonly IContentPropertyConverter<WhatYouWillDo> whatYouWillDoPropertyConverter;
        private readonly IJobProfileCategoryRepository jobProfileCategoryRepository;

        #endregion Fields

        #region Ctor

        public DynamicContentConverter(IRelatedClassificationsRepository relatedClassificationsRepository, IDynamicContentExtensions dynamicContentExtensions, IContentPropertyConverter<HowToBecomeData> htbContentPropertyConverter, IContentPropertyConverter<WhatYouWillDo> whatYouWillDoPropertyConverter, IJobProfileCategoryRepository jobProfileCategoryRepository)
        {
            this.relatedClassificationsRepository = relatedClassificationsRepository;
            this.htbContentPropertyConverter = htbContentPropertyConverter;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.whatYouWillDoPropertyConverter = whatYouWillDoPropertyConverter;
            this.jobProfileCategoryRepository = jobProfileCategoryRepository;
        }

        #endregion Ctor

        public JobProfileMessage ConvertFrom(DynamicContent content)
        {
            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(Constants.DynamicProvider);
            var jobProfileMessage = new JobProfileMessage
            {
                JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(content, content.GetContentItemIdKey()),
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.Title)),
                WidgetContentTitle = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.WidgetContentTitle)),
                AlternativeTitle = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.AlternativeTitle)),
                Overview = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.Overview)),
                SalaryStarter = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfileMessage.SalaryStarter)),
                SalaryExperienced = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfileMessage.SalaryExperienced)),
                MinimumHours = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfileMessage.MinimumHours)),
                MaximumHours = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfileMessage.MaximumHours)),
                CareerPathAndProgression = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.CareerPathAndProgression)),
                CourseKeywords = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.CourseKeywords)),

                //Need to use a string to get the content cannot use JobProfileMessage.JobProfileCategories as this is already used in the search
                //index and we will get a clash
                UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.UrlName)),
                IsImported = dynamicContentExtensions.GetFieldValue<bool>(content, nameof(JobProfileMessage.IsImported)),

                // How To Become section
                HowToBecomeData = htbContentPropertyConverter.ConvertFrom(content),
                RelatedCareersData = GetRelatedCareersData(content, Constants.RelatedCareerProfiles),
                Restrictions = GetRestrictions(content, RelatedRestrictionsField),
                OtherRequirements = dynamicContentExtensions.GetFieldValue<Lstring>(content, OtherRequirementsField),
                DynamicTitlePrefix = dynamicContentExtensions.GetFieldChoiceLabel(content, nameof(JobProfileMessage.DynamicTitlePrefix)),
                DigitalSkillsLevel = dynamicContentExtensions.GetFieldChoiceLabel(content, nameof(JobProfileMessage.DigitalSkillsLevel)),
            };

            jobProfileMessage.IncludeInSitemap = content.IncludeInSitemap;

            //What You will do section
            jobProfileMessage.WhatYouWillDoData = GetWYDRelatedDataForJobProfiles(content);

            //Related Skills Data
            jobProfileMessage.SocSkillsMatrixData = GetSocSkillMatrixItems(content, Constants.RelatedSkills);

            //Get SOC Code data
            var socItem = dynamicContentExtensions.GetRelatedItems(content, Constants.SocField, 1).FirstOrDefault();

            //SocCode Data
            jobProfileMessage.SocCodeData = GenerateSocData(content, socItem);

            //Working Pattern Details
            jobProfileMessage.WorkingPatternDetails = MapClassificationData(content.GetValue<TrackedList<Guid>>(Constants.WorkingPatternDetail));

            //Working Hours Details
            jobProfileMessage.WorkingHoursDetails = MapClassificationData(content.GetValue<TrackedList<Guid>>(Constants.WorkingHoursDetail));

            //Working Pattern
            jobProfileMessage.WorkingPattern = MapClassificationData(content.GetValue<TrackedList<Guid>>(Constants.WorkingPattern));

            //Hidden Alternative Title
            jobProfileMessage.HiddenAlternativeTitle = MapClassificationData(content.GetValue<TrackedList<Guid>>(Constants.HiddenAlternativeTitle));

            //Job Profile Specialism
            jobProfileMessage.JobProfileSpecialism = MapClassificationData(content.GetValue<TrackedList<Guid>>(Constants.JobProfileSpecialism));

            if (socItem != null)
            {
                jobProfileMessage.SocLevelTwo = dynamicContentExtensions.GetFieldValue<Lstring>(socItem, Constants.SOCCode).ToString().Substring(0, 2);
            }

            jobProfileMessage.LastModified = dynamicContentExtensions.GetFieldValue<DateTime>(content, nameof(JobProfileMessage.LastModified));
            jobProfileMessage.CanonicalName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.UrlName)).ToLower();
            jobProfileMessage.JobProfileCategories = GetJobCategories(dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, RelatedJobProfileCategoriesField));
            return jobProfileMessage;
        }

        private IEnumerable<SocSkillMatrixItem> GetSocSkillMatrixItems(DynamicContent content, string relatedField)
        {
            var relatedSkills = new List<SocSkillMatrixItem>();
            var relatedItems = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    relatedSkills.Add(new SocSkillMatrixItem
                    {
                        Id = dynamicContentExtensions.GetFieldValue<Guid>(relatedItem, content.GetContentItemIdKey()),
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(SocSkillMatrixItem.Title)),
                        Contextualised = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(SocSkillMatrixItem.Contextualised)),
                        ONetAttributeType = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(SocSkillMatrixItem.ONetAttributeType)),
                        ONetRank = dynamicContentExtensions.GetFieldValue<decimal?>(relatedItem, nameof(SocSkillMatrixItem.ONetRank)).GetValueOrDefault(0),
                        Rank = dynamicContentExtensions.GetFieldValue<decimal?>(relatedItem, nameof(SocSkillMatrixItem.Rank)).GetValueOrDefault(0),
                        RelatedSkill = GetRelatedSkillsData(content, relatedItem, nameof(SocSkillMatrixItem.RelatedSkill)),
                        RelatedSOC = GetRelatedSocsData(content, relatedItem, nameof(SocSkillMatrixItem.RelatedSOC))
                    });
                }
            }

            return relatedSkills;
        }

        private IEnumerable<FrameworkSkillItem> GetRelatedSkillsData(DynamicContent jobProfileContent, DynamicContent content, string relatedField)
        {
            var relatedSkillsData = new List<FrameworkSkillItem>();
            var relatedItems = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    relatedSkillsData.Add(new FrameworkSkillItem
                    {
                        Id = dynamicContentExtensions.GetFieldValue<Guid>(relatedItem, jobProfileContent.GetContentItemIdKey()),
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(FrameworkSkillItem.Title)),
                        Description = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(FrameworkSkillItem.Description)),
                        ONetElementId = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(FrameworkSkillItem.ONetElementId))
                    });
                }
            }

            return relatedSkillsData;
        }

        private IEnumerable<RelatedSocCodeItem> GetRelatedSocsData(DynamicContent jobprofileContent, DynamicContent content, string relatedField)
        {
            var relatedSocsData = new List<RelatedSocCodeItem>();
            var relatedItems = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    relatedSocsData.Add(GetRelatedSocData(jobprofileContent, relatedItem));
                }
            }

            return relatedSocsData;
        }

        private IEnumerable<RestrictionItem> GetRestrictions(DynamicContent content, string relatedField)
        {
            var restrictions = new List<RestrictionItem>();
            var relatedItems = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    restrictions.Add(new RestrictionItem
                    {
                        Id = dynamicContentExtensions.GetFieldValue<Guid>(relatedItem, relatedItem.GetContentItemIdKey()),
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(RestrictionItem.Title)),
                        Info = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(RestrictionItem.Info))
                    });
                }
            }

            return restrictions;
        }

        private SocCodeItem GenerateSocData(DynamicContent jobprofileContent, DynamicContent content)
        {
            var apprenticeshipStandardsData = content?.GetValue<TrackedList<Guid>>(Constants.ApprenticeshipStandards.ToLower());
            var apprenticeshipFrameworkData = content?.GetValue<TrackedList<Guid>>(Constants.ApprenticeshipFramework.ToLower());

            var socCodes = new SocCodeItem
            {
                Id = dynamicContentExtensions.GetFieldValue<Guid>(content, jobprofileContent.GetContentItemIdKey()),
                SOCCode = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocCode.SOCCode)),
                Description = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocCode.Description)),
                UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocCode.UrlName)),
                ONetOccupationalCode = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocCode.ONetOccupationalCode)),
                ApprenticeshipFramework = MapClassificationData(apprenticeshipFrameworkData),
                ApprenticeshipStandards = MapClassificationData(apprenticeshipStandardsData)
            };

            return socCodes;
        }

        private RelatedSocCodeItem GetRelatedSocData(DynamicContent jobprofileContent, DynamicContent content)
        {
            var socCodes = new RelatedSocCodeItem
            {
                Id = dynamicContentExtensions.GetFieldValue<Guid>(content, jobprofileContent.GetContentItemIdKey()),
                SOCCode = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocCode.SOCCode)).ToLower()
            };

            return socCodes;
        }

        private IEnumerable<Classification> MapClassificationData(TrackedList<Guid> classifications)
        {
            var classificationData = new List<Classification>();
            TaxonomyManager taxonomyManager = TaxonomyManager.GetManager();
            if (classifications != null)
            {
                foreach (var cat in classifications)
                {
                    classificationData.Add(new Classification
                    {
                        Id = taxonomyManager.GetTaxon(cat).Id,
                        Title = taxonomyManager.GetTaxon(cat).Title,
                        Url = taxonomyManager.GetTaxon(cat).UrlName,
                        Description = taxonomyManager.GetTaxon(cat).Description
                    });
                }
            }

            return classificationData;
        }

        private IEnumerable<JobProfileRelatedCareerItem> GetRelatedCareersData(DynamicContent content, string relatedField)
        {
            var jobProfileRelatedCareerData = new List<JobProfileRelatedCareerItem>();
            var relatedItems = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    jobProfileRelatedCareerData.Add(new JobProfileRelatedCareerItem
                    {
                        Id = dynamicContentExtensions.GetFieldValue<Guid>(relatedItem, content.GetContentItemIdKey()),
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(JobProfileRelatedCareerItem.Title)),
                        ProfileLink = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, Constants.Url),
                    });
                }
            }

            return jobProfileRelatedCareerData;
        }

        private IEnumerable<JobProfileCategoryItem> GetJobCategories(IList<Guid> categoryIds)
        {
            IEnumerable<JobProfileCategoryItem> jobProfileCategories = jobProfileCategoryRepository.GetByCategoryIds(categoryIds);
            List<JobProfileCategoryItem> jobProfileCategoriesData = new List<JobProfileCategoryItem>();
            if (jobProfileCategories != null)
            {
                foreach (var jobProfileCategory in jobProfileCategories.ToList())
                {
                    jobProfileCategoriesData.Add(new JobProfileCategoryItem
                    {
                        Id = jobProfileCategory.Id,
                        Title = jobProfileCategory.Title,
                        Name = jobProfileCategory.Name
                    });
                }
            }

            return jobProfileCategoriesData;
        }

        private WhatYouWillDoData GetWYDRelatedDataForJobProfiles(DynamicContent content)
        {
            var wydData = new WhatYouWillDoData
            {
                Introduction =
                        dynamicContentExtensions.GetFieldValue<Lstring>(content, Constants.WYDIntroduction),
                DailyTasks =
                        dynamicContentExtensions.GetFieldValue<Lstring>(content, Constants.WYDDayToDayTasks),
                Locations = GetWYDRelatedItems(content, Constants.RelatedLocations),
                Uniforms = GetWYDRelatedItems(content, Constants.RelatedUniforms),
                Environments = GetWYDRelatedItems(content, Constants.RelatedEnvironments),
            };

            return wydData;
        }

        private IEnumerable<WYDRelatedContentType> GetWYDRelatedItems(DynamicContent content, string relatedField)
        {
            var items = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            var relatedContentTypes = new List<WYDRelatedContentType>();
            if (items != null && items.Any())
            {
                foreach (var item in items)
                {
                    relatedContentTypes.Add(new WYDRelatedContentType
                    {
                        Id = dynamicContentExtensions.GetFieldValue<Guid>(item, content.GetContentItemIdKey()),
                        Description = dynamicContentExtensions.GetFieldValue<Lstring>(item, nameof(WYDRelatedContentType.Description)),
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(item, nameof(WYDRelatedContentType.Title)),
                        Url = dynamicContentExtensions.GetFieldValue<Lstring>(item, Constants.Url),
                        IsNegative = dynamicContentExtensions.GetFieldValue<bool>(item, nameof(WYDRelatedContentType.IsNegative))
                    });
                }

                return relatedContentTypes;
            }

            return Enumerable.Empty<WYDRelatedContentType>();
        }
    }
}