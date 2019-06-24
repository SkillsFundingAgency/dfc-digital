using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ICompositeUIService
    {
        Task<bool> PostPageDataAsync(string microServiceEndPointConfigKey,  CompositePageData compositePageData);
    }
}
