using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Base;
using DFC.Digital.Repository.SitefinityCMS.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

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
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly IContentPropertyConverter<HowToBecome> htbContentPropertyConverter;

        #endregion Fields

        #region Ctor

        public JobProfileConverter(IRelatedClassificationsRepository relatedClassificationsRepository, IDynamicContentExtensions dynamicContentExtensions, IContentPropertyConverter<HowToBecome> htbContentPropertyConverter)
        {
            this.relatedClassificationsRepository = relatedClassificationsRepository;
            this.htbContentPropertyConverter = htbContentPropertyConverter;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        #endregion Ctor

        public IQueryable<string> GetRelatedContentUrl(DynamicContent content, string relatedField)
        {
            var relatedContent = dynamicContentExtensions.GetRelatedItems(content, relatedField, 100);
            return relatedContent.Select(x => $"{x.UrlName}");
        }

        public JobProfile ConvertFrom(DynamicContent content)
        {
            Stopwatch time = new Stopwatch();
            time.Start();

            var jobProfile = new JobProfile
            {
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.Title)),
                AlternativeTitle = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.AlternativeTitle)),
                Overview = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.Overview)),
                IsLMISalaryFeedOverriden = dynamicContentExtensions.GetFieldValue<bool?>(content, nameof(JobProfile.IsLMISalaryFeedOverriden)),
                SalaryStarter = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfile.SalaryExperienced)),
                SalaryExperienced = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfile.SalaryStarter)),
                MinimumHours = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfile.MinimumHours)),
                MaximumHours = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(JobProfile.MaximumHours)),
                CareerPathAndProgression = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.CareerPathAndProgression)),
                Skills = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.Skills)),
                HowToBecome = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.HowToBecome)),
                WhatYouWillDo = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.WhatYouWillDo)),
                WorkingHoursPatternsAndEnvironment = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.WorkingHoursPatternsAndEnvironment)),
                Salary = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.Salary)),
                CourseKeywords = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.CourseKeywords)),

                //Need to use a string to get the content cannot use JobProfile.JobProfileCategories as this is already used in the search
                //index and we will get a clash
                  JobProfileCategoryIdCollection = content?.GetValueOrDefault<IList<Guid>>(RelatedJobProfileCategoriesField),
                UrlName = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.UrlName)),
                DoesNotExistInBAU = content?.GetValueOrDefault<bool>(nameof(JobProfile.DoesNotExistInBAU)),
                BAUSystemOverrideUrl = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.BAUSystemOverrideUrl)),
                IsImported = content?.GetValueOrDefault<bool>(nameof(JobProfile.IsImported)),

                // How To Become section
                HowToBecomes = new EnumerableQuery<HowToBecome>(new List<HowToBecome> { htbContentPropertyConverter.ConvertFrom(content) }),

                IsHTBCaDReady = content?.GetValueOrDefault<bool>(nameof(JobProfile.IsHTBCaDReady)) ?? false
            };

            var socItem = dynamicContentExtensions.GetRelatedItems(content, SocField, 1).FirstOrDefault();
            if (socItem != null)
            {
                jobProfile.SOCCode = dynamicContentExtensions.GetFieldValue<Lstring>(socItem, nameof(JobProfile.SOCCode));
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

            time.Stop();

            //  jobProfile.EntryRoutes = jobProfile.HowToBecome + "<p>* * * Execution time: " + time.ElapsedMilliseconds + " milliseconds * * *</p>";
            jobProfile.HowToBecome = jobProfile.HowToBecome + "<p>* * * Execution time: " + time.ElapsedMilliseconds + " milliseconds * * *</p>";
            jobProfile.Skills = jobProfile.Skills + "<p>* * * Execution time: " + time.ElapsedMilliseconds + " milliseconds * * *</p>";
            jobProfile.WhatYouWillDo = jobProfile.WhatYouWillDo + "<p>* * * Execution time: " + time.ElapsedMilliseconds + " milliseconds * * *</p>";
            jobProfile.Salary = jobProfile.Salary + "<p>* * * Execution time: " + time.ElapsedMilliseconds + " milliseconds * * *</p>";
            jobProfile.WorkingHoursPatternsAndEnvironment = jobProfile.WorkingHoursPatternsAndEnvironment + "<p>* * * Execution time: " + time.ElapsedMilliseconds + " milliseconds * * *</p>";
            jobProfile.CareerPathAndProgression = jobProfile.CareerPathAndProgression + "<p>* * * Execution time: " + time.ElapsedMilliseconds + " milliseconds * * *</p>";
            time.Reset();

            return jobProfile;
        }
    }
}