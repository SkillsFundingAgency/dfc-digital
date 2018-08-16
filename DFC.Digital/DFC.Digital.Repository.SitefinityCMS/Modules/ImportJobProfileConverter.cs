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
    public class ImportJobProfileConverter : IDynamicModuleConverter<JobProfileOverloadForWhatItTakes>
    {
        #region Fields

        private const string SocField = "SOC";
        private const string RelatedSkillsField = "RelatedSkills";
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        #endregion Fields

        #region Ctor

        public ImportJobProfileConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        #endregion Ctor

        public JobProfileOverloadForWhatItTakes ConvertFrom(DynamicContent content)
        {
            var jobProfile = new JobProfileOverloadForWhatItTakes();

            jobProfile.UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.UrlName));
            jobProfile.Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.Title));
            jobProfile.DigitalSkillsLevel = dynamicContentExtensions.GetFieldChoiceValue(content, nameof(JobProfile.DigitalSkillsLevel));

            var socItem = dynamicContentExtensions.GetRelatedItems(content, SocField, 1).FirstOrDefault();
            if (socItem != null)
            {
                jobProfile.SOCCode = dynamicContentExtensions.GetFieldValue<Lstring>(socItem, nameof(JobProfile.SOCCode));
                jobProfile.ONetOccupationalCode = dynamicContentExtensions.GetFieldValue<Lstring>(socItem, nameof(JobProfile.ONetOccupationalCode));
            }

            jobProfile.HasRelatedSocSkillMatrices = dynamicContentExtensions.GetRelatedItems(content, RelatedSkillsField, 1)?.Any() == true;

            return jobProfile;
        }
    }
}