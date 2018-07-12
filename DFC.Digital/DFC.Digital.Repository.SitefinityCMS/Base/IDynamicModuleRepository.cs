using DFC.Digital.Data.Interfaces;
using System;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IDynamicModuleRepository<T> : IRepository<DynamicContent>, IUnitOfWork
    {
        Type GetContentType();

        void Initialise(string contentType, string dynamicModuleName);

        string GetProviderName();

        DynamicContent Create();

        void Add(DynamicContent entity, string changeComment);

        void Update(DynamicContent entity, string changeComment);

        void Publish(DynamicContent entity, string changeComment);

        DynamicContent Checkout(string urlName);

        DynamicContent GetMaster(DynamicContent entity);

        DynamicContent GetTemp(DynamicContent entity);

        DynamicContent CheckinTemp(DynamicContent entity);
    }
}