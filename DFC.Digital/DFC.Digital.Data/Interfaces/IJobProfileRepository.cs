using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileRepository
    {
        JobProfile GetByUrlName(string urlName);

        JobProfile GetByUrlNameForPreview(string urlName);

        string GetProviderName();

        Type GetContentType();

        JobProfile GetByUrlNameForSearchIndex(string urlName, bool isPublishing);

        string AddOrUpdateJobProfileByProperties(JobProfileImporting bauJobProfile, Dictionary<string, string> propertyMappings, string changeComment, bool enforcePublishing, bool disableUpdate);

        string UpdateRelatedCareers(JobProfileImporting bauJobProfile, string changeComment, bool enforcePublishing);
    }
}