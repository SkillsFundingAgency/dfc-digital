﻿using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileConverter : IDynamicModuleConverter<JobProfile>
    {
        #region Fields

        private const string SocField = "SOC";
        private const string RelatedInterestsField = "RelatedInterests";
        private const string RelatedEnablersField = "RelatedEnablers";
        private const string RelatedEntryQualificationsField = "RelatedEntryQualifications";
        private const string RelatedTrainingRoutesField = "RelatedTrainingRoutes";
        private const string RelatedJobProfileCategoriesField = "JobProfileCategories";
        private const string RelatedJobAreasField = "RelatedJobAreas";
        private const string RelatedPreferredTaskTypesField = "RelatedPreferredTaskTypes";

        private readonly IRelatedClassificationsRepository relatedClassificationsRepository;

        #endregion Fields

        #region Ctor

        public JobProfileConverter(IRelatedClassificationsRepository relatedClassificationsRepository)
        {
            this.relatedClassificationsRepository = relatedClassificationsRepository;
        }

        #endregion Ctor

        public static IQueryable<string> GetRelatedContentUrl(DynamicContent content, string relatedField)
        {
            var relatedContent = content.GetRelatedItems<DynamicContent>(relatedField);
            return relatedContent.Select(x => $"{x.UrlName}");
        }

        public JobProfile ConvertFrom(DynamicContent content)
        {
            var jobProfile = new JobProfile
            {
                Title = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.Title)),
                AlternativeTitle = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.AlternativeTitle)),
                Overview = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.Overview)),
                IsLMISalaryFeedOverriden = content?.GetValueOrDefault<bool?>(nameof(JobProfile.IsLMISalaryFeedOverriden)),
                SalaryStarter = content?.GetValueOrDefault<decimal?>(nameof(JobProfile.SalaryStarter)),
                SalaryExperienced = content?.GetValueOrDefault<decimal?>(nameof(JobProfile.SalaryExperienced)),
                MinimumHours = content?.GetValueOrDefault<decimal?>(nameof(JobProfile.MinimumHours)),
                MaximumHours = content?.GetValueOrDefault<decimal?>(nameof(JobProfile.MaximumHours)),
                CareerPathAndProgression = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.CareerPathAndProgression)),
                Skills = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.Skills)),
                HowToBecome = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.HowToBecome)),
                WhatYouWillDo = content?.GetValue<Lstring>(nameof(JobProfile.WhatYouWillDo)),
                WorkingHoursPatternsAndEnvironment = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.WorkingHoursPatternsAndEnvironment)),
                Salary = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.Salary)),
                CourseKeywords = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.CourseKeywords)),

                //Need to use a string to get the content cannot use JobProfile.JobProfileCategories as this is already used in the search
                //index and we will get a clash
                JobProfileCategoryIdCollection = content?.GetValueOrDefault<IList<Guid>>(RelatedJobProfileCategoriesField),
                UrlName = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.UrlName)),
                DoesNotExistInBAU = content?.GetValueOrDefault<bool>(nameof(JobProfile.DoesNotExistInBAU)),
                BAUSystemOverrideUrl = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.BAUSystemOverrideUrl)),
                IsImported = content?.GetValueOrDefault<bool>(nameof(JobProfile.IsImported))
            };

            var socItem = content.GetRelatedItems<DynamicContent>(SocField).FirstOrDefault();
            if (socItem != null)
            {
                jobProfile.SOCCode = socItem.GetValueOrDefault<Lstring>(nameof(JobProfile.SOCCode));
            }

            jobProfile.WorkingHoursDetails = relatedClassificationsRepository.GetRelatedClassifications(content, nameof(JobProfile.WorkingHoursDetails), nameof(JobProfile.WorkingHoursDetails)).FirstOrDefault();
            jobProfile.WorkingPattern = relatedClassificationsRepository.GetRelatedClassifications(content, nameof(JobProfile.WorkingPattern), nameof(JobProfile.WorkingPattern)).FirstOrDefault();
            jobProfile.WorkingPatternDetails = relatedClassificationsRepository.GetRelatedClassifications(content, nameof(JobProfile.WorkingPatternDetails), nameof(JobProfile.WorkingPatternDetails)).FirstOrDefault();

            jobProfile.RelatedInterests = GetRelatedContentUrl(content, RelatedInterestsField);
            jobProfile.RelatedEnablers = GetRelatedContentUrl(content, RelatedEnablersField);
            jobProfile.RelatedEntryQualifications = GetRelatedContentUrl(content, RelatedEntryQualificationsField);
            jobProfile.RelatedTrainingRoutes = GetRelatedContentUrl(content, RelatedTrainingRoutesField);
            jobProfile.RelatedPreferredTaskTypes = GetRelatedContentUrl(content, RelatedPreferredTaskTypesField);
            jobProfile.RelatedJobAreas = GetRelatedContentUrl(content, RelatedJobAreasField);

            return jobProfile;
        }
    }
}