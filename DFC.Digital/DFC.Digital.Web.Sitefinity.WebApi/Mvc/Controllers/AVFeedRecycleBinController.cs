using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DFC.Digital.Web.Sitefinity.WebApi.Mvc.Controllers
{
    public class AVFeedRecycleBinController : ApiController
    {
        private readonly IRecyleBinRepository recyleBinRepository;

        public AVFeedRecycleBinController(IRecyleBinRepository recyleBinRepository)
        {
            this.recyleBinRepository = recyleBinRepository;
        }

        // DELETE dfcapi/avfeedrecyclebin
        [Authorize]
        public void Delete([FromBody]int count)
        {
            recyleBinRepository.DeleteVacanciesPermanently(count);
        }
    }
}
