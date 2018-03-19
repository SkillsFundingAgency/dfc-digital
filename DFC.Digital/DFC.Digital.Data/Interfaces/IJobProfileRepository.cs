using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileRepository
    {
        JobProfile GetByUrlName(string urlName);

        JobProfile GetByUrlNameForPreview(string urlName);

        string GetProviderName();

        Type GetContentType();

        JobProfile GetByUrlNameForSearchIndex(string urlName);
    }
}