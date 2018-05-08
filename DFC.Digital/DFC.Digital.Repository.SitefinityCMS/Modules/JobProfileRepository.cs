using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public class JobProfileRepository : IJobProfileRepository
    {
        #region Fields

        private readonly IDynamicModuleRepository<JobProfile> repository;
        private readonly IDynamicModuleConverter<JobProfile> converter;

        private Dictionary<string, JobProfile> cachedJobProfiles = new Dictionary<string, JobProfile>();

        #endregion Fields

        #region Ctor

        public JobProfileRepository(IDynamicModuleRepository<JobProfile> repository, IDynamicModuleConverter<JobProfile> converter)
        {
            this.repository = repository;
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
            var key = urlName.ToLower();
            if (!cachedJobProfiles.ContainsKey(key))
            {
                var jobProfile = ConvertDynamicContent(repository.Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible == true));
                if (jobProfile?.IsImported == false)
                {
                    cachedJobProfiles.Add(key, jobProfile);
                }
            }

            return cachedJobProfiles[key];
        }

        /// <summary>
        /// Returns a jobprofile for Preview mode to a user logged into the backed.
        /// Profiles that are not live are still returned
        /// </summary>
        /// <param name="urlName">URL of the jobprofile to return</param>
        /// <returns>JobProfile</returns>
        public JobProfile GetByUrlNameForPreview(string urlName)
        {
            return ConvertDynamicContent(repository.Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Temp));
        }

        public JobProfile GetByUrlNameForSearchIndex(string urlName, bool isPublishing)
        {
            return ConvertDynamicContent(repository.Get(item => item.UrlName == urlName && item.Status == (isPublishing ? ContentLifecycleStatus.Master : ContentLifecycleStatus.Live)));
        }

        public Type GetContentType()
        {
            return repository.GetContentType();
        }

        public string GetProviderName()
        {
            return repository.GetProviderName();
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