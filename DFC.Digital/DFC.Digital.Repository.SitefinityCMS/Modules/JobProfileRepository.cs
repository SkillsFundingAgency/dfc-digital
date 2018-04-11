﻿using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using System;
using System.Collections.Generic;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;

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
                var jobProfile = ConvertDynamicContent(repository.Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible == true
                                                        && item.ApprovalWorkflowState != "Draft"));
                cachedJobProfiles.Add(key, jobProfile);
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

        public string AddOrUpdateJobProfileByProperties(BauJobProfile bauJobProfile, Dictionary<string, string> propertyMappings, string changeComment, bool enforcePublishing, bool disableUpdate)
        {
            string actionTaken = string.Empty;
            var betaProfile = repository.Get(item => item.UrlName == bauJobProfile.UrlName && item.Status == ContentLifecycleStatus.Master);

            if (betaProfile != null)
            {
                if (disableUpdate)
                {
                    actionTaken = "NOT updated as set in the preferencies.";
                }
                else
                {
                    repository.UpdateOnImport(betaProfile, bauJobProfile, propertyMappings, changeComment, enforcePublishing);
                    actionTaken = "updated.";
                }
            }
            else
            {
                betaProfile = repository.CreateEntity();
                betaProfile.UrlName = bauJobProfile.UrlName;
                betaProfile.SetPropertyValue("Title", bauJobProfile.Title);
                foreach (var propertyMapping in propertyMappings)
                {
                    betaProfile.SetValue(propertyMapping.Key, $"From import => {bauJobProfile.GetPropertyValue(propertyMapping.Value) as string}");
                }

                repository.AddOnImport(betaProfile, changeComment, enforcePublishing);
                actionTaken = "inserted.";
            }

            return actionTaken;
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