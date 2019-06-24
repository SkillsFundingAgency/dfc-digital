﻿using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Web.Mvc;
using AutoMapper;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "JobProfileStructuredData", Title = "JobProfile Structured Data", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileStructuredDataController : BaseDfcController
    {
        private readonly IStructuredDataInjectionRepository structuredDataInjectionRepository;
        private readonly IMapper mapper;

        public JobProfileStructuredDataController(IMapper mapper, IStructuredDataInjectionRepository structuredDataInjectionRepository, IApplicationLogger loggingService) : base(loggingService)
        {
            this.structuredDataInjectionRepository = structuredDataInjectionRepository;
            this.mapper = mapper;
        }
        #region Actions

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            return View(new JobProfileStructuredDataViewModel
            {
                InPreviewMode = true
            });
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
            var dataStructure = structuredDataInjectionRepository.GetByJobProfileUrlName(urlName);

            if (dataStructure != null)
            {
                return View(mapper.Map<JobProfileStructuredDataViewModel>(dataStructure));
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            Index().ExecuteResult(ControllerContext);
        }

        #endregion Actions
    }
}