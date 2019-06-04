using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DFC.Digital.Web.Sitefinity.WebApi.Mvc.Controllers
{
    public class ReadConfigController : ApiController
    {
        private readonly IConfigurationProvider configurationProvider;

        public ReadConfigController(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        [ApiAuthorize]
        [HttpGet]
        public IHttpActionResult Index(string key)
        {
            var readConfig = this.configurationProvider.GetConfig<string>(key);

            if (string.IsNullOrEmpty(readConfig))
            {
                return this.NotFound();
            }

            return this.Ok(readConfig);
        }
    }
}