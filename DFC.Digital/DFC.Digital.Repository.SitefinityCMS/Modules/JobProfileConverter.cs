﻿using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static List<InfoItem> GetRelatedInfoItems(DynamicContent content, string relatedField)
        {
            List<InfoItem> infoItems = new List<InfoItem>();
            var relatedItems = content.GetRelatedItems<DynamicContent>(relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    var infoItem = new InfoItem
                    {
                        Title = relatedItem.GetValueOrDefault<Lstring>(nameof(InfoItem.Title)),
                        Info = relatedItem.GetValueOrDefault<Lstring>(nameof(InfoItem.Info))
                    };
                    infoItems.Add(infoItem);
                }
            }

            return infoItems;
        }

        public static List<LinkItem> GetRelatedLinkItems(DynamicContent content, string relatedField)
        {
            List<LinkItem> linkItems = new List<LinkItem>();
            var relatedItems = content.GetRelatedItems<DynamicContent>(relatedField);
            if (relatedItems != null)
            {
                foreach (var relatedItem in relatedItems)
                {
                    var linkItem = new LinkItem
                    {
                        Title = relatedItem.GetValueOrDefault<Lstring>(nameof(LinkItem.Title)),
                        Url = relatedItem.GetValueOrDefault<Lstring>(nameof(LinkItem.Url))
                    };
                    linkItems.Add(linkItem);
                }
            }

            return linkItems;
        }

        public JobProfile ConvertFrom(DynamicContent content)
        {
            Stopwatch time = new Stopwatch();
            time.Start();

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
                IsImported = content?.GetValueOrDefault<bool>(nameof(JobProfile.IsImported)),

                // How To Become section
                EntryRoutes = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.EntryRoutes)),

                // UNIVERSITY
                UniversityRelevantSubjects = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.UniversityRelevantSubjects)),
                UniversityFurtherRouteInfo = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.UniversityFurtherRouteInfo)),

                // choices
                UniversityRequirements = (content?.GetValueOrDefault<ChoiceOption>(nameof(JobProfile.UniversityRequirements)))?.Text,

                // related
                RelatedUniversityRequirement = GetRelatedInfoItems(content, nameof(JobProfile.RelatedUniversityRequirement)),

                // related
                RelatedUniversityLinks = GetRelatedLinkItems(content, nameof(JobProfile.RelatedUniversityLinks)),

                // COLLEGE
                CollegeRelevantSubjects = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.CollegeRelevantSubjects)),
                CollegeFurtherRouteInfo = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.CollegeFurtherRouteInfo)),

                // choices
                CollegeRequirements = (content?.GetValueOrDefault<ChoiceOption>(nameof(JobProfile.CollegeRequirements)))?.Text,

                // related
                RelatedCollegeRequirements = GetRelatedInfoItems(content, nameof(JobProfile.RelatedCollegeRequirements)),

                // related
                RelatedCollegeLinks = GetRelatedLinkItems(content, nameof(JobProfile.RelatedCollegeLinks)),

                // APPRENTICESHIP
                ApprenticeshipRelevantSubjects = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.ApprenticeshipRelevantSubjects)),
                ApprenticeshipFurtherRouteInfo = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.ApprenticeshipFurtherRouteInfo)),

                // choices
                ApprenticeshipRequirements = (content?.GetValueOrDefault<ChoiceOption>(nameof(JobProfile.ApprenticeshipRequirements)))?.Text,

                // related
                RelatedApprenticeshipRequirements = GetRelatedInfoItems(content, nameof(JobProfile.RelatedApprenticeshipRequirements)),

                // related
                RelatedApprenticeshipLinks = GetRelatedLinkItems(content, nameof(JobProfile.RelatedApprenticeshipLinks)),

                // OTHER
                Work = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.Work)),
                Volunteering = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.Volunteering)),
                DirectApplication = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.DirectApplication)),
                OtherRoutes = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.OtherRoutes)),

                // related
                RelatedRestrictions = GetRelatedInfoItems(content, nameof(JobProfile.RelatedRestrictions)),
                OtherRequirements = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.OtherRequirements)),

                // related
                RelatedRegistrations = GetRelatedInfoItems(content, nameof(JobProfile.RelatedRegistrations)),
                ProfessionalAndIndustryBodies = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.ProfessionalAndIndustryBodies)),
                CareerTips = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.CareerTips)),
                FurtherInformation = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.FurtherInformation)),
                IsHTBCaDReady = content?.GetValueOrDefault<bool>(nameof(JobProfile.IsHTBCaDReady)) ?? false
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

            time.Stop();
            jobProfile.EntryRoutes = jobProfile.HowToBecome + "<p>* * * Execution time: " + time.ElapsedMilliseconds + " milliseconds * * *</p>";
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