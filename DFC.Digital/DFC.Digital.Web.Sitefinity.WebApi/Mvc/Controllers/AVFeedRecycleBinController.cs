﻿using DFC.Digital.Data.Interfaces;
using System.Net;
using System.Web.Http;
using Telerik.Sitefinity.Utilities.MS.ServiceModel.Web;
using Telerik.Sitefinity.Web.Services;

namespace DFC.Digital.Web.Sitefinity.WebApi.Mvc.Controllers
{
    public class AVFeedRecycleBinController : ApiController
    {
        private readonly IRecycleBinRepository recyleBinRepository;

        public AVFeedRecycleBinController(IRecycleBinRepository recyleBinRepository)
        {
            this.recyleBinRepository = recyleBinRepository;
        }

        // DELETE dfcapi/avfeedrecyclebin
        [Authorize]
        public IHttpActionResult Delete([FromBody]int count)
        {
            var isThisLastPage = recyleBinRepository.DeleteVacanciesPermanently(count);
            return isThisLastPage ? StatusCode(HttpStatusCode.OK) : StatusCode(HttpStatusCode.PartialContent);
        }
    }
}