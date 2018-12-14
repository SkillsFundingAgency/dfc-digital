﻿using DFC.Digital.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for customising page titles
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "SetCanonicalUrl", Title = "Set Canonical Url", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class SetCanonicalUrlController : SfBaseDfcController
    {
        #region Constructors

        public SetCanonicalUrlController(IApplicationLogger applicationLogger) : base(applicationLogger)
        {
        }

        #endregion Constructors

        #region Actions

        // GET: for CustomisePageTitle

        /// <summary>
        /// entry point to the widget to customise page title.
        /// </summary>
        /// <returns>result</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            return PageHandlerResult();
        }

        /// <summary>
        /// CustomisePageTitle on Index of the specified UrlName.
        /// </summary>
        /// <param name="urlName">The UrlName.</param>
        /// <returns>result</returns>
        [HttpGet]
        [RelativeRoute("{urlName}")]
        public ActionResult Index(string urlName)
        {
            return PageHandlerResult();
        }

        #endregion Actions

        #region Private Methods

        private ActionResult PageHandlerResult()
        {
            SetupPreRenderEventHandler();

            return new EmptyResult();
        }

        #endregion

    }
}