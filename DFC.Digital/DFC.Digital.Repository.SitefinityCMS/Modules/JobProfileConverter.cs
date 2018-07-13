using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using System;
using System.Collections.Generic;
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
        private const string OtherRequirementsField = "OtherRequirements";
        private const string RelatedRestrictionsField = "RelatedRestrictions";
        private const string RelatedSkillsField = "RelatedSkills";
        private const string RelatedSkillField = "RelatedSkill";

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
                WhatItTakesSkills = GetRelatedSkills(content, RelatedSkillsField),
                DigitalSkillsLevel = dynamicContentExtensions.GetFieldValue<ChoiceOption>(content, nameof(JobProfile.DigitalSkillsLevel)).Text,
                HowToBecome = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.HowToBecome)),
                WhatYouWillDo = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.WhatYouWillDo)),
                WorkingHoursPatternsAndEnvironment = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.WorkingHoursPatternsAndEnvironment)),
                Salary = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.Salary)),
                CourseKeywords = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.CourseKeywords)),

                //Need to use a string to get the content cannot use JobProfile.JobProfileCategories as this is already used in the search
                //index and we will get a clash
                JobProfileCategoryIdCollection = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, RelatedJobProfileCategoriesField),
                UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.UrlName)),
                DoesNotExistInBAU = dynamicContentExtensions.GetFieldValue<bool>(content, nameof(JobProfile.DoesNotExistInBAU)),
                BAUSystemOverrideUrl = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.BAUSystemOverrideUrl)),
                IsImported = dynamicContentExtensions.GetFieldValue<bool>(content, nameof(JobProfile.IsImported)),

                // How To Become section
                HowToBecomeData = htbContentPropertyConverter.ConvertFrom(content),
                Restrictions = GetRestrictions(content, RelatedRestrictionsField),
                OtherRequirements = dynamicContentExtensions.GetFieldValue<Lstring>(content, OtherRequirementsField)
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
            return jobProfile;
        }

        private IEnumerable<WhatItTakesSkill> GetRelatedSkills(DynamicContent content, string relatedField)
        {
            var skills = new List<WhatItTakesSkill>();
            var relatedItems = dynamicContentExtensions.GetRelatedItems(content, relatedField);
            if (relatedItems != null)
            {
                string skillDescription;

                foreach (var relatedItem in relatedItems)
                {
                    skillDescription = string.Empty;
                    try
                    {
                        skillDescription = GetSkillDescription(relatedItem);
                    }
                    catch (LoggedException)
                    {
                        //This is here in case somebody deletes the related skill in Sitefinity, This is an error
                        //The exception will have been logged and the system needs to continue but not display this skill to the citizen
                    }

                    if (skillDescription != string.Empty)
                    {
                        skills.Add(new WhatItTakesSkill
                        {
                            Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(WhatItTakesSkill.Title)),
                            Description = skillDescription,
                            Contextualised = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(WhatItTakesSkill.Contextualised)),
                        });
                    }
                }
            }

            return skills;
        }

        private string GetSkillDescription(DynamicContent relatedSkill)
        {
            var skill = dynamicContentExtensions.GetRelatedItems(relatedSkill, RelatedSkillField).FirstOrDefault();
            return dynamicContentExtensions.GetFieldValue<Lstring>(skill, nameof(WhatItTakesSkill.Description));
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
                        Title = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(InfoItem.Title)),
                        Info = dynamicContentExtensions.GetFieldValue<Lstring>(relatedItem, nameof(InfoItem.Info))
                    });
                }
            }

            return restrictions;
        }
    }
}