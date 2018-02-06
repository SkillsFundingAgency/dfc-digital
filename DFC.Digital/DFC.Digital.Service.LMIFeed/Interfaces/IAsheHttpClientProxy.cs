using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Service.LMIFeed.Interfaces
{
    public interface IAsheHttpClientProxy
    {
        Task<HttpResponseMessage> EstimatePayMdAsync(string socCode);
    }
}