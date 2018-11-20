using System.ServiceModel;
using System.ServiceModel.Web;

namespace DFC.Digital.Web.Sitefinity.Core.Interfaces
{
    [ServiceContract]
    public interface IRecycleBinService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
       void ClearAppVacanciesRecycleBin();
    }
}
