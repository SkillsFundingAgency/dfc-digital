using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public class JobProfileRepository : DynamicModuleRepository, IJobProfileRepository
    {
        #region Fields

        private const string JobprofileContentType = "Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile";
        private const string ModuleName = "Job Profile";

        private IDynamicModuleConverter<JobProfile> converter;

        #endregion Fields

        #region Ctor

        public JobProfileRepository(IDynamicModuleConverter<JobProfile> converter)
            : base(ModuleName, JobprofileContentType)
        {
            this.converter = converter;
        }

        #endregion Ctor

        #region IJobProfileRepository Implementations

        /// <summary>
        /// Returns a jobprofile for normal front end view.
        /// Only live profiles are returned.
        /// </summary>
        /// <param name="urlName">URL of the jobprofile to return</param>
        /// <returns>JobProfile</returns>
        public JobProfile GetByUrlName(string urlName)
        {
            return ConvertDynamicContent(Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible == true));
        }

        /// <summary>
        /// Returns a jobprofile for Preview mode to a user logged into the backed.
        /// Profiles that are not live are still returned
        /// </summary>
        /// <param name="urlName">URL of the jobprofile to return</param>
        /// <returns>JobProfile</returns>
        public JobProfile GetByUrlNameForPreview(string urlName)
        {
            return ConvertDynamicContent(Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Temp));
        }

        public Type GetContentType()
        {
            return DynamicModuleContentType;
        }

        public string GetProviderName()
        {
            return ProviderName;
        }

        private JobProfile ConvertDynamicContent(DynamicContent dynamicContent)
        {
            if (dynamicContent != null)
            {
                return converter.ConvertFrom(dynamicContent);
            }

            return null;
        }

        #endregion IJobProfileRepository Implementations
    }
}