using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileRepository
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "It is a string field and not a url")]
        JobProfile GetByUrlName(string urlName);

        JobProfile GetByUrlNameForPreview(string urlName);

        string GetProviderName();

        Type GetContentType();

        JobProfile GetByUrlNameForSearchIndex(string urlName);
    }
}