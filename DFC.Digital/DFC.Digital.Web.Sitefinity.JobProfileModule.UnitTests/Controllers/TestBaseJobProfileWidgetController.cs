using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using System;
using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class TestBaseJobProfileWidgetController : BaseJobProfileWidgetController
    {
        public TestBaseJobProfileWidgetController(IWebAppContext webAppContext, IJobProfileRepository jobProfileRepository, IApplicationLogger loggingService, ISitefinityPage sitefinityPage) : base(webAppContext, jobProfileRepository, loggingService, sitefinityPage)
        {
        }

        protected override ActionResult GetDefaultView()
        {
            throw new NotImplementedException();
        }

        protected override ActionResult GetEditorView()
        {
            throw new NotImplementedException();
        }
    }
}