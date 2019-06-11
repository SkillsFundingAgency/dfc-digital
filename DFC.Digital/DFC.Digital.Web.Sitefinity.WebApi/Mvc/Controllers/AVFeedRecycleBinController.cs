using DFC.Digital.Data.Interfaces;
using System.Net;
using System.Web.Http;

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
        [ApiAuthorize(Roles = "AVIntegration")]
        public IHttpActionResult Delete(int count)
        {
            var isThisLastPage = recyleBinRepository.DeleteVacanciesPermanently(count);
            return isThisLastPage ? StatusCode(HttpStatusCode.OK) : StatusCode(HttpStatusCode.PartialContent);
        }
    }
}