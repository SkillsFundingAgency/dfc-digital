using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IMicroServicesPublishingService
    {
        Task<bool> PostPageDataAsync(string microServiceEndPointConfigKey,  MicroServicesPublishingPageData compositePageData);
    }
}
