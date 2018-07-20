using DFC.Digital.Data.Interfaces;
using System;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IDynamicModuleRepository<T> : IQueryRepository<DynamicContent>
    {
        Type GetContentType();

        void Initialise(string contentType, string dynamicModuleName);

        string GetProviderName();
    }
}