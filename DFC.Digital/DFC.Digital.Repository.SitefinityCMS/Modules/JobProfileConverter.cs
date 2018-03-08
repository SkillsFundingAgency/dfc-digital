using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Base;
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

        #region Fields

        public JobProfileConverter(IRelatedClassificationsRepository relatedClassificationsRepository)
        {
            this.relatedClassificationsRepository = relatedClassificationsRepository;
        }

        #endregion Fields

        public JobProfile ConvertFrom(DynamicContent content)
        {
            var jobProfile = new JobProfile
            {
                Title = content?.GetValue<Lstring>(nameof(JobProfile.Title)),
                AlternativeTitle = content?.GetValue<Lstring>(nameof(JobProfile.AlternativeTitle)),
                Overview = content?.GetValue<Lstring>(nameof(JobProfile.Overview)),
                IsLMISalaryFeedOverriden = content?.GetValue<bool?>(nameof(JobProfile.IsLMISalaryFeedOverriden)),
                SalaryStarter = content?.GetValue<decimal?>(nameof(JobProfile.SalaryStarter)),
                SalaryExperienced = content?.GetValue<decimal?>(nameof(JobProfile.SalaryExperienced)),
                MinimumHours = content?.GetValue<decimal?>(nameof(JobProfile.MinimumHours)),
                MaximumHours = content?.GetValue<decimal?>(nameof(JobProfile.MaximumHours)),
                CareerPathAndProgression = content?.GetValue<Lstring>(nameof(JobProfile.CareerPathAndProgression)),
                Skills = content?.GetValue<Lstring>(nameof(JobProfile.Skills)),
                HowToBecome = content?.GetValue<Lstring>(nameof(JobProfile.HowToBecome)),
                WhatYouWillDo = content?.GetValue<Lstring>(nameof(JobProfile.WhatYouWillDo)),
                WorkingHoursPatternsAndEnvironment = content?.GetValue<Lstring>(nameof(JobProfile.WorkingHoursPatternsAndEnvironment)),
                Salary = content?.GetValue<Lstring>(nameof(JobProfile.Salary)),
                CourseKeywords = content?.GetValue<Lstring>(nameof(JobProfile.CourseKeywords)),

                //Need to use a string to get the content cannot use JobProfile.JobProfileCategories as this is already used in the search
                //index and we will get a clash
                JobProfileCategoryIdCollection = content?.GetValue<IList<Guid>>(RelatedJobProfileCategoriesField),
                UrlName = content?.GetValue<Lstring>(nameof(JobProfile.UrlName)),
                DoesNotExistInBAU = content?.GetValue<bool>(nameof(JobProfile.DoesNotExistInBAU)),
                BAUSystemOverrideUrl = content?.GetValue<Lstring>(nameof(JobProfile.BAUSystemOverrideUrl))
            };

            var socItem = content.GetRelatedItems<DynamicContent>(SocField).FirstOrDefault();
            if (socItem != null)
            {
                jobProfile.SOCCode = socItem.GetValue<Lstring>(nameof(JobProfile.SOCCode));
            }

            jobProfile.WorkingHoursDetails = relatedClassificationsRepository.GetRelatedClassifications(content, nameof(JobProfile.WorkingHoursDetails), nameof(JobProfile.WorkingHoursDetails)).FirstOrDefault();
            jobProfile.WorkingPattern = relatedClassificationsRepository.GetRelatedClassifications(content, nameof(JobProfile.WorkingPattern), nameof(JobProfile.WorkingPattern)).FirstOrDefault();
            jobProfile.WorkingPatternDetails = relatedClassificationsRepository.GetRelatedClassifications(content, nameof(JobProfile.WorkingPatternDetails), nameof(JobProfile.WorkingPatternDetails)).FirstOrDefault();

            jobProfile.RelatedInterests = GetRelatedContentIdAndUrl(content, RelatedInterestsField);
            jobProfile.RelatedEnablers = GetRelatedContentIdAndUrl(content, RelatedEnablersField);
            jobProfile.RelatedEntryQualifications = GetRelatedContentIdAndUrl(content, RelatedEntryQualificationsField);
            jobProfile.RelatedTrainingRoutes = GetRelatedContentIdAndUrl(content, RelatedTrainingRoutesField);
            jobProfile.RelatedPreferredTaskTypes = GetRelatedContentIdAndUrl(content, RelatedPreferredTaskTypesField);
            jobProfile.RelatedJobAreas = GetRelatedContentIdAndUrl(content, RelatedJobAreasField);

            return jobProfile;
        }

        public IQueryable<string> GetRelatedContentIdAndUrl(DynamicContent content, string relatedField)
        {
            var relatedContent = content.GetRelatedItems<DynamicContent>(relatedField);
            return relatedContent.Select(x => $"{x.Id}|{x.UrlName}");
        }
    }
}