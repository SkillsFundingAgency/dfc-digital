using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IDynamicModuleRepository<T> : IRepository<DynamicContent>
    {
        Type GetContentType();

        void Initialise(string contentType, string dynamicModuleName);

        string GetProviderName();

        void AddOnImport(DynamicContent entity, string changeComment, bool enforcePublishing = false);

        void UpdateOnImport(DynamicContent entity, JobProfileImporting bauJobProfile, Dictionary<string, string> propertyMappings, string changeComment, bool enforcePublishing = false);

        string UpdateRelatedCareers(DynamicContent entity, JobProfileImporting bauJobProfile, string changeComment, bool enforcePublishing = false);
    }
}