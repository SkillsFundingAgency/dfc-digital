using DFC.Digital.Data.Model;
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

        public static List<LinkItem> ParseLinkItems(string stringLinks)
        {
            List<LinkItem> linkItems = new List<LinkItem>();

            if (!string.IsNullOrEmpty(stringLinks))
            {
                string[] links = stringLinks.Split('|');

                if (links != null)
                {
                    foreach (var link in links)
                    {
                        var linkItem = new LinkItem
                        {
                            Title = link.Split('^')[0].Trim(),
                            Url = link.Split('^')[1].Trim()
                        };
                        linkItems.Add(linkItem);
                    }
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
                SpecificUniversityLinks = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.SpecificUniversityLinks)),

                // related
                UniversityLinks = GetRelatedLinkItems(content, nameof(JobProfile.UniversityLinks)),

                // related to be deleted
                //Universities { get; set; }
                UniversityInformation = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.UniversityInformation)),
                UniversityEntryRequirements = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.UniversityEntryRequirements)),

                // COLLEGE
                CollegeRelevantSubjects = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.CollegeRelevantSubjects)),
                CollegeEntryRequirements = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.CollegeEntryRequirements)),

                // related
                Colleges = GetRelatedInfoItems(content, nameof(JobProfile.Colleges)),
                CollegeFindACourse = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.CollegeFindACourse)),
                CollegeFurtherRouteInfo = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.CollegeFurtherRouteInfo)),

                // APPRENTICESHIP
                ApprenticeshipFurtherRouteInfo = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.ApprenticeshipFurtherRouteInfo)),
                ApprenticeshipRelevantSubjects = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.ApprenticeshipRelevantSubjects)),

                // related
                Apprenticeships = GetRelatedInfoItems(content, nameof(JobProfile.Apprenticeships)),
                ApprenticeshipEntryRequirements = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.ApprenticeshipEntryRequirements)),

                // OTHER
                Work = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.Work)),
                Volunteering = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.Volunteering)),
                DirectApplication = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.DirectApplication)),
                OtherRoutes = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.OtherRoutes)),
                AgeLimitation = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.AgeLimitation)),

                // related
                Restrictions = GetRelatedInfoItems(content, nameof(JobProfile.Restrictions)),

                // choices
                DBScheckReason = (content?.GetValueOrDefault<ChoiceOption>(nameof(JobProfile.DBScheckReason)))?.Text,
                MedicalTestReason = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.MedicalTestReason)),
                FullDrivingLicence = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.FullDrivingLicence)),
                OtherRestrictions = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.OtherRestrictions)),
                OtherRequirements = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.OtherRequirements)),

                // related
                CommonRegistrations = GetRelatedInfoItems(content, nameof(JobProfile.CommonRegistrations)),
                OtherRegistration = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.OtherRegistration)),
                ProfessionalAndIndustryBodies = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.ProfessionalAndIndustryBodies)),
                CareerTips = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.CareerTips)),
                MoreInfo = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.MoreInfo)),
                IsHTBCaDReady = content?.GetValueOrDefault<bool>(nameof(JobProfile.IsHTBCaDReady)) ?? false
            };

            jobProfile.SpecificUniversityLinkList = ParseLinkItems(jobProfile.SpecificUniversityLinks);
            jobProfile.DBSCheck = jobProfile.Restrictions.FirstOrDefault(x => x.Title.Equals(nameof(JobProfile.DBSCheck), StringComparison.OrdinalIgnoreCase))?.Info;
            jobProfile.Restrictions.Remove(jobProfile.Restrictions.SingleOrDefault(x => x.Title.Equals(nameof(JobProfile.DBSCheck), StringComparison.OrdinalIgnoreCase)));
            jobProfile.MedicalTest = jobProfile.Restrictions.FirstOrDefault(x => x.Title.Equals(nameof(JobProfile.MedicalTest), StringComparison.OrdinalIgnoreCase))?.Info;
            jobProfile.Restrictions.Remove(jobProfile.Restrictions.SingleOrDefault(x => x.Title.Equals(nameof(JobProfile.MedicalTest), StringComparison.OrdinalIgnoreCase)));

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