using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileOverloadForSearchConverter : IDynamicModuleConverter<JobProfileOverloadForSearch>
    {
        #region Fields

        private const string SocField = "SOC";
        private const string RelatedJobProfileCategoriesField = "JobProfileCategories";

        private const string RelatedInterestsField = "RelatedInterests";
        private const string RelatedEnablersField = "RelatedEnablers";
        private const string RelatedEntryQualificationsField = "RelatedEntryQualifications";
        private const string RelatedTrainingRoutesField = "RelatedTrainingRoutes";
        private const string RelatedJobAreasField = "RelatedJobAreas";
        private const string RelatedPreferredTaskTypesField = "RelatedPreferredTaskTypes";
        private const string RelatedSkills = "RelatedSkills";

        private readonly IDynamicContentExtensions dynamicContentExtensions;

        #endregion Fields

        #region Ctor

        public JobProfileOverloadForSearchConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        #endregion Ctor

        public JobProfileOverloadForSearch ConvertFrom(DynamicContent content)
        {
            var jobProfile = new JobProfileOverloadForSearch
            {
                //Need to use a string to get the content cannot use JobProfile.JobProfileCategories as this is already used in the search
                //index and we will get a clash
                JobProfileCategoryIdCollection =
                    dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, RelatedJobProfileCategoriesField)
            };

            var socItem = dynamicContentExtensions.GetRelatedSearchItems(content, SocField, 1).FirstOrDefault();
            if (socItem != null)
            {
                jobProfile.SOCCode = dynamicContentExtensions.GetFieldValue<Lstring>(socItem, nameof(JobProfile.SOCCode));
                jobProfile.ONetOccupationalCode = dynamicContentExtensions.GetFieldValue<Lstring>(socItem, nameof(JobProfile.ONetOccupationalCode));
            }

            //PSF
            jobProfile.RelatedInterests = dynamicContentExtensions.GetRelatedSearchItemsUrl(content, RelatedInterestsField);
            jobProfile.RelatedEnablers = dynamicContentExtensions.GetRelatedSearchItemsUrl(content, RelatedEnablersField);
            jobProfile.RelatedTrainingRoutes = dynamicContentExtensions.GetRelatedSearchItemsUrl(content, RelatedTrainingRoutesField);
            jobProfile.RelatedPreferredTaskTypes = dynamicContentExtensions.GetRelatedSearchItemsUrl(content, RelatedPreferredTaskTypesField);
            jobProfile.RelatedJobAreas = dynamicContentExtensions.GetRelatedSearchItemsUrl(content, RelatedJobAreasField);

            var skills = dynamicContentExtensions.GetRelatedSearchItemsUrl(content, RelatedSkills);

            if (skills != null)
            {
                jobProfile.RelatedSkills = new List<string>();

                foreach (var skill in skills)
                {
                    jobProfile.RelatedSkills.Add(skill.Substring(skill.IndexOf("-") + 1).Replace(" ", "-"));
                }
            }

            var relatedRelatedEntryQualifications = dynamicContentExtensions.GetRelatedSearchItems(content, RelatedEntryQualificationsField, 100);
            jobProfile.RelatedEntryQualifications = relatedRelatedEntryQualifications?.AsQueryable().Select(x => $"{x.UrlName}");
            jobProfile.EntryQualificationLowestLevel = GetLowestLevel(relatedRelatedEntryQualifications);

            return jobProfile;
        }

        private double GetLowestLevel(IEnumerable<DynamicContent> entryQualifications)
        {
            double retlevel = double.MaxValue;
            foreach (DynamicContent dc in entryQualifications)
            {
                if (!dynamicContentExtensions.GetFieldValue<bool>(dc, nameof(PreSearchFilter.NotApplicable)))
                {
                    var order = (double)dynamicContentExtensions.GetFieldValue<decimal?>(dc, nameof(PreSearchFilter.Order));
                    if (order < retlevel)
                    {
                        retlevel = order;
                    }
                }
            }

            return retlevel == double.MaxValue ? 0 : retlevel;
        }
    }
}