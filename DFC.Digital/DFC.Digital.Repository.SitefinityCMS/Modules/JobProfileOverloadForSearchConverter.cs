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
            jobProfile.RelatedInterests = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedInterestsField);
            jobProfile.RelatedEnablers = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedEnablersField);
            jobProfile.RelatedEntryQualifications = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedEntryQualificationsField);
            jobProfile.RelatedTrainingRoutes = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedTrainingRoutesField);
            jobProfile.RelatedPreferredTaskTypes = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedPreferredTaskTypesField);
            jobProfile.RelatedJobAreas = dynamicContentExtensions.GetRelatedContentUrl(content, RelatedJobAreasField);

            return jobProfile;
        }
    }
}