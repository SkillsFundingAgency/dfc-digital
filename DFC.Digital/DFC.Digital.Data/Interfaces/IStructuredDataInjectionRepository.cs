using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Data.Interfaces
{
    public interface IStructuredDataInjectionRepository
    {
        StructuredDataInjection GetByJobProfileUrlName(string urlName);

        Type GetContentType();

        string GetProviderName();
    }
}
