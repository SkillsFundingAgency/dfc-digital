using DFC.Digital.Core;
using System.Web.Http;

namespace DFC.Digital.Web.Sitefinity.WebApi.Mvc.Controllers
{
    public class CompUIPreviewDomainController : ApiController
    {
        private readonly IConfigurationProvider configurationProvider;

        public CompUIPreviewDomainController(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        [ApiAuthorize(Roles = "BackendUsers")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            var readConfig = this.configurationProvider.GetConfig<string>(Constants.MicroServiceHelpPreviewEndPoint);

            if (string.IsNullOrEmpty(readConfig))
            {
                return this.NotFound();
            }

            return this.Ok(readConfig);
        }
    }
}