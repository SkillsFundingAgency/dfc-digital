using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Service.LMIFeed
{
    public interface IAsheHttpClientProxy
    {
        Task<HttpResponseMessage> EstimatePayMdAsync(string socCode);
    }
}