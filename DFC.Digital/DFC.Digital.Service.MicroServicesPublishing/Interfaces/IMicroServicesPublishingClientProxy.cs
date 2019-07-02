using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Service.MicroServicesPublishing
{
    public interface IMicroServicesPublishingClientProxy
    {
         Task<HttpResponseMessage> PostDataAsync(string postEndPoint, string pageDataJson);
    }
}
