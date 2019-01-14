using DFC.Digital.Data.Interfaces;
using DFC.Digital.Repository.SitefinityCMS.Modules;
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
        private readonly IRecycleBinRepository recyleBinRepository;

        public AVFeedRecycleBinController(IRecycleBinRepository recyleBinRepository)
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
