using DFC.Digital.Data.Interfaces;
using System.Net;
using System.Web.Http;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Telerik.Sitefinity.Utilities.MS.ServiceModel.Web;
using Telerik.Sitefinity.Web.Services;

namespace DFC.Digital.Web.Sitefinity.WebApi
{
    public class AVFeedRecycleBinController : ApiController
    {
        private readonly IRecycleBinRepository recyleBinRepository;

        public AVFeedRecycleBinController(IRecycleBinRepository recyleBinRepository)
        {
            this.recyleBinRepository = recyleBinRepository;
        }

        // DELETE dfcapi/avfeedrecyclebin
        [ApiAuthorize]
        public IHttpActionResult Delete(int count)
        {
            var isThisLastPage = recyleBinRepository.DeleteVacanciesPermanently(count);
            return isThisLastPage ? StatusCode(HttpStatusCode.OK) : StatusCode(HttpStatusCode.PartialContent);
        }
    }
}