using DFC.Digital.Core;
using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

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
        private readonly IContentPropertyConverter<HowToBecome> htbContentPropertyConverter;
        private readonly IContentPropertyConverter<WhatYouWillDo> whatYouWillDoPropertyConverter;
        private readonly IJobProfileCategoryRepository jobProfileCategoryRepository;

        #endregion Fields

        #region Ctor

        public DynamicContentConverter(IRelatedClassificationsRepository relatedClassificationsRepository, IDynamicContentExtensions dynamicContentExtensions, IContentPropertyConverter<HowToBecome> htbContentPropertyConverter, IContentPropertyConverter<WhatYouWillDo> whatYouWillDoPropertyConverter, IJobProfileCategoryRepository jobProfileCategoryRepository)
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
            var jobProfileMessage = new JobProfileMessage
            {
                JobProfileId = dynamicContentExtensions.GetFieldValue<Guid>(content, "Id"),
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.Title)),
                AlternativeTitle = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.AlternativeTitle)),
                WidgetContentTitle = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.WidgetContentTitle)),
                Overview = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.Overview)),
                IsLMISalaryFeedOverriden = dynamicContentExtensions.GetFieldValue<bool?>(content, nameof(JobProfileMessage.IsLMISalaryFeedOverriden)),
                SalaryStarter = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfileMessage.SalaryStarter)),
                SalaryExperienced = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfileMessage.SalaryExperienced)),
                MinimumHours = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfileMessage.MinimumHours)),
                MaximumHours = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfileMessage.MaximumHours)),
                CareerPathAndProgression = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.CareerPathAndProgression)),
                CourseKeywords = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.CourseKeywords)),

                //Need to use a string to get the content cannot use JobProfileMessage.JobProfileCategories as this is already used in the search
                //index and we will get a clash
                UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.UrlName)),
                DoesNotExistInBAU = dynamicContentExtensions.GetFieldValue<bool>(content, nameof(JobProfileMessage.DoesNotExistInBAU)),
                BAUSystemOverrideUrl = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.BAUSystemOverrideUrl)),
                IsImported = dynamicContentExtensions.GetFieldValue<bool>(content, nameof(JobProfileMessage.IsImported)),

                // How To Become section
                HowToBecomeData = htbContentPropertyConverter.ConvertFrom(content),
                Restrictions = GetRestrictions(content, RelatedRestrictionsField),
                OtherRequirements = dynamicContentExtensions.GetFieldValue<Lstring>(content, OtherRequirementsField),
                DynamicTitlePrefix = dynamicContentExtensions.GetFieldChoiceLabel(content, nameof(JobProfileMessage.DynamicTitlePrefix)),

                //What You will do section
                WhatYouWillDoData = whatYouWillDoPropertyConverter.ConvertFrom(content),
                RelatedSkills = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedSkillsField)?.ToList(),
                DigitalSkillsLevel = dynamicContentExtensions.GetFieldChoiceLabel(content, nameof(JobProfileMessage.DigitalSkillsLevel))
            };

            var socItem = dynamicContentExtensions.GetRelatedItems(content, Constants.SocField, 1).FirstOrDefault();

            if (socItem != null)
            {
                jobProfileMessage.SOCCode = dynamicContentExtensions.GetFieldValue<Lstring>(socItem, nameof(JobProfileMessage.SOCCode));
                jobProfileMessage.ONetOccupationalCode =
                    dynamicContentExtensions.GetFieldValue<Lstring>(socItem, nameof(JobProfileMessage.ONetOccupationalCode));
            }

            jobProfileMessage.WorkingHoursDetails = relatedClassificationsRepository.GetRelatedClassifications(content, nameof(JobProfileMessage.WorkingHoursDetails), nameof(JobProfileMessage.WorkingHoursDetails)).FirstOrDefault();
            jobProfileMessage.WorkingPattern = relatedClassificationsRepository.GetRelatedClassifications(content, nameof(JobProfileMessage.WorkingPattern), nameof(JobProfileMessage.WorkingPattern)).FirstOrDefault();
            jobProfileMessage.WorkingPatternDetails = relatedClassificationsRepository.GetRelatedClassifications(content, nameof(JobProfileMessage.WorkingPatternDetails), nameof(JobProfileMessage.WorkingPatternDetails)).FirstOrDefault();

            //PSF
            jobProfileMessage.RelatedInterests = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedInterestsField);
            jobProfileMessage.RelatedEnablers = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedEnablersField);
            jobProfileMessage.RelatedEntryQualifications = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedEntryQualificationsField);
            jobProfileMessage.RelatedTrainingRoutes = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedTrainingRoutesField);
            jobProfileMessage.RelatedPreferredTaskTypes = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedPreferredTaskTypesField);
            jobProfileMessage.RelatedJobAreas = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedJobAreasField);

            jobProfileMessage.SocCodeId = jobProfileMessage.SOCCode.Substring(0, 2);
            jobProfileMessage.LastModified = dynamicContentExtensions.GetFieldValue<DateTime>(content, nameof(JobProfileMessage.LastModified));
            jobProfileMessage.IncludeInSiteMap = content.IncludeInSitemap;
            jobProfileMessage.CanonicalName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfileMessage.Title)).ToLower();
            jobProfileMessage.JobProfileCategories = GetJobCategories(dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, RelatedJobProfileCategoriesField));
            return jobProfileMessage;
        }

        private IEnumerable<Restriction> GetRestrictions(DynamicContent content, string relatedField)
        {
            var restrictions = new List<Restriction>();
            var relatedItems = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    restrictions.Add(new Restriction
                    {
                        Id = dynamicContentExtensions.GetFieldValue<Guid>(relatedItem, nameof(InfoItem.Id)),
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(InfoItem.Title)),
                        Info = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(InfoItem.Info)),
                        CType = nameof(Restriction)
                    });
                }
            }

            return restrictions;
        }

        private IEnumerable<JobProfileCategory> GetJobCategories(IList<Guid> categoryIds)
        {
            IEnumerable<JobProfileCategory> jobProfileCategories = jobProfileCategoryRepository.GetByIds(categoryIds);
            if (jobProfileCategories != null)
            {
                foreach (var jobProfileCategory in jobProfileCategories.ToList())
                {
                    jobProfileCategories.Append(new JobProfileCategory
                    {
                        Id = jobProfileCategory.Id,
                        Title = jobProfileCategory.Title
                    });
                }
            }

            return jobProfileCategories;
        }
    }
}