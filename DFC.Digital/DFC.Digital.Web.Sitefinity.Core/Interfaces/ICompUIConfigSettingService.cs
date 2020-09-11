using System.ServiceModel;
using System.ServiceModel.Web;
using Telerik.Sitefinity.Utilities.MS.ServiceModel.Web;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [ServiceContract]
    public interface ICompUIConfigSettingService
    {
        /// <summary>
        /// Tests the connection to the service.
        /// </summary>
        [WebHelp(Comment = "Tests the connection to the service. Result is returned in JSON format.")]
        [WebGet(UriTemplate = "/GetFromKey/{key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetFromKey(string key);
    }
}
