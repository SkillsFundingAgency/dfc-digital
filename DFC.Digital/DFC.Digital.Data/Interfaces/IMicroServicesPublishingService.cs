using DFC.Digital.Data.Model;
using System;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IMicroServicesPublishingService
    {
        Task<bool> PostPageDataAsync(string microServiceEndPointConfigKey,  MicroServicesPublishingPageData compositePageData);

        Task<bool> DeletePageAsync(string microServiceEndPointConfigKey, Guid pageId);

        Task<bool> PostDynamicContentDataAsync(string microServiceEndPointConfigKey, JobProfile compositeDynamicContentData);
    }
}
