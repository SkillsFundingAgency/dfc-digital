using System.Linq;
using System.Web.Http;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;

namespace DFC.Digital.Web.Sitefinity.WebApi.Mvc.Controllers
{
    public class ContentPreviewController : ApiController
    {
        private readonly IJobProfileRepository jobProfileRepository;
        private readonly ISitefinityPage sitefinityPage;

        public ContentPreviewController(IJobProfileRepository jobProfileRepository, ISitefinityPage sitefinityPage)
        {
            this.jobProfileRepository = jobProfileRepository;
            this.sitefinityPage = sitefinityPage;
        }

        [ApiAuthorize]
        [Route("dfcapi/contentpreview/{contentType}/{urlName}")]
        public IHttpActionResult Get(string contentType, string urlName)
        {
            switch (contentType)
            {
                case "Page":
                    return Json(sitefinityPage.GetPagePreviewByUrlName(urlName).Where(item => !string.IsNullOrWhiteSpace(item.Key)));
                case nameof(JobProfile):
                    return Json(jobProfileRepository.GetByUrlNameForPreview(urlName));
                default:
                    break;
            }

            return null;
        }
    }
}
