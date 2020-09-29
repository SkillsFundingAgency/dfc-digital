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
        /// <returns>returns COMPUI preview Url for JobProfiles</returns>
        [WebHelp(Comment = "Service to get the COMPUI Domain Url for Job Profiles. Result is returned in JSON format.")]
        [WebGet(UriTemplate = "/GetUrl", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetUrl();
    }
}
