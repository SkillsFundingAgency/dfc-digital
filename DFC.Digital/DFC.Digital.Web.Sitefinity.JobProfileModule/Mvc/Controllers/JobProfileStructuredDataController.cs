using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "JobProfileStructuredData", Title = "JobProfile Structured Data", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileStructuredDataController : BaseDfcController
    {
        #region private fields
        private readonly IStructuredDataInjectionRepository structuredDataInjectionRepository;
        private readonly IMapper mapper;
        private readonly ISitefinityPage sitefinityPage;
        #endregion

        #region Ctor
        public JobProfileStructuredDataController(ISitefinityPage sitefinityPage, IMapper mapper, IStructuredDataInjectionRepository structuredDataInjectionRepository, IApplicationLogger loggingService) : base(loggingService)
        {
            this.structuredDataInjectionRepository = structuredDataInjectionRepository;
            this.mapper = mapper;
            this.sitefinityPage = sitefinityPage;
        }

        #endregion

        #region Actions
        public string DefaultJobProfileLinkName { get; set; } = "plumber";
        #endregion

        #region Actions

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            return GetActionResult(sitefinityPage.GetDefaultJobProfileToUse(DefaultJobProfileLinkName));
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <param name="urlName">Invoked category</param>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("{urlName}")]
        public ActionResult Index(string urlName)
        {
            return GetActionResult(urlName);
        }

        private ActionResult GetActionResult(string urlName)
        {
            var dataStructure = structuredDataInjectionRepository.GetByJobProfileUrlName(urlName);

            if (dataStructure != null)
            {
                return View(mapper.Map<JobProfileStructuredDataViewModel>(dataStructure));
            }

            return new EmptyResult();
        }

        #endregion Actions
    }
}