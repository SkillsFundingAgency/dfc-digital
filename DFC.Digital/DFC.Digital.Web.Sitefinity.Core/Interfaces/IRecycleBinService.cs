using System.ServiceModel;
using System.ServiceModel.Web;

namespace DFC.Digital.Web.Sitefinity.Core.Interfaces
{
    [ServiceContract]
    public interface IRecycleBinService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
       void RecycleBinClearAppVacancies();
    }
}
