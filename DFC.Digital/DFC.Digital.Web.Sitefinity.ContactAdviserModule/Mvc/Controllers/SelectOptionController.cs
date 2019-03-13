﻿using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using DFC.Digital.Web.Sitefinity.Core;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for Select Option form
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "SelectOption", Title = "Select an Option", SectionName = SitefinityConstants.ContactUsSection)]
    public class SelectOptionController : BaseDfcController
    {
        #region Private Fields
        private IEmailTemplateRepository emailTemplateRepository;
        private ISitefinityCurrentContext sitefinityCurrentContext;

        #endregion Private Fields

        #region Constructors

        public SelectOptionController(IEmailTemplateRepository emailTemplateRepository, ISitefinityCurrentContext sitefinityCurrentContext, IApplicationLogger applicationLogger) : base(applicationLogger)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.sitefinityCurrentContext = sitefinityCurrentContext;
        }

        #endregion Constructors

        #region Actions

        // GET: ContactAdviser

        /// <summary>
        /// entry point to the widget to show contact adviser form.
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// Updates the form and sends the data to next form.
        /// </summary>
        /// <param name="model">The Email Template model.</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult Index(ContactUsViewModel model)
        {
            switch (model.ContactOption)
            {
                case ContactOption.Technical:
                    Response.Redirect("/technical");
                    break;
                case ContactOption.Feedback:
                    Response.Redirect("/feedback");
                    break;
                default:
                    Response.Redirect("/contact-adviser");
                    break;
            }

            return View("Index");
        }

        #endregion Actions
    }
}