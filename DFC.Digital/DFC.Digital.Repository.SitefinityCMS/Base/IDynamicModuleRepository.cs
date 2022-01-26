using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Data.Model.OrchardCore;
using DFC.Digital.Data.Model.OrchardCore.Uniform;
using System;
using System.Collections.Generic;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IDynamicModuleRepository<T> : IRepository<DynamicContent>, IUnitOfWork
    {
        Type GetContentType();

        void Initialise(string contentType, string dynamicModuleName);

        string GetProviderName();

        DynamicContent Create();

        [IgnoreInputInInterception]
        void Add(DynamicContent entity, string changeComment);

        [IgnoreInputInInterception]
        void Update(DynamicContent entity, string changeComment);

        [IgnoreInputInInterception]
        void Publish(DynamicContent entity, string changeComment);

        [IgnoreOutputInInterception]
        DynamicContent Checkout(string urlName);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        DynamicContent GetMaster(DynamicContent entity);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        DynamicContent GetTemp(DynamicContent entity);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        DynamicContent CheckinTemp(DynamicContent entity);

        bool IsCheckedOut(DynamicContent entity);

        IEnumerable<OcUniform> GetAllUniforms();

        IEnumerable<OcApprenticeshipLink> GetAllApprenticeshipLinks();
    }
}