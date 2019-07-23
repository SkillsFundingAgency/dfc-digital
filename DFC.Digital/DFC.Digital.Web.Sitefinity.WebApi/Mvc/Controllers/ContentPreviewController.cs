﻿using DFC.Digital.Web.Sitefinity.Core;
using System.Web.Http;

namespace DFC.Digital.Web.Sitefinity.WebApi.Mvc.Controllers
{
    public class ContentPreviewController : ApiController
    {
        private readonly ICompositePageBuilder compositePageBuilder;

        public ContentPreviewController(ICompositePageBuilder compositePageBuilder)
        {
            this.compositePageBuilder = compositePageBuilder;
        }

        [ApiAuthorize(Roles = "MicroservicePreview")]
        [Route("dfcapi/contentpreview/{name}")]
        public IHttpActionResult Get(string name)
        {
            var preViewPage = compositePageBuilder.GetCompositePreviewPage(name);
            return Json(preViewPage);
        }
    }
}
