﻿using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using DFC.Digital.Web.Sitefinity.Core;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for Contact Adviser form
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "ContactAdviser", Title = "Contact Adviser", SectionName = SitefinityConstants.ContactUsSection)]
    public class ContactAdviserController : BaseDfcController
    {
        #region Private Fields

        private readonly IEmailTemplateRepository emailTemplateRepository;
        private readonly ISitefinityCurrentContext sitefinityCurrentContext;
        private readonly IMapper mapper;
        private readonly ISessionStorage<ContactUs> sessionStorage;

        #endregion Private Fields

        #region Constructors

        public ContactAdviserController(
            IEmailTemplateRepository emailTemplateRepository,
            ISitefinityCurrentContext sitefinityCurrentContext,
            IApplicationLogger applicationLogger,
            IMapper mapper,
            ISessionStorage<ContactUs> sessionStorage) : base(applicationLogger)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.sitefinityCurrentContext = sitefinityCurrentContext;
            this.mapper = mapper;
            this.sessionStorage = sessionStorage;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Next Page URL")]
        public string NextPageUrl { get; set; } = "/contact-us/your-details-adviser/";

        [DisplayName("Page Title")]
        public string Title { get; set; } = "What is your query about?";

        [DisplayName("Relative page url to select option page")]
        public string ContactOptionPageUrl { get; set; } = "/contact-us/select-option/";

        #endregion Public Properties

        #region Actions

        // GET: ContactAdviser

        /// <summary>
        /// entry point to the widget to show contact adviser form.
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var sessionModel = sessionStorage.Get() ?? new ContactUs();

            if (sessionModel.ContactUsOption == null)
            {
                return Redirect(ContactOptionPageUrl);
            }

            var model = new ContactAdviserViewModel
            {
                Title = Title
            };

            return View("Index", model);
        }

        /// <summary>
        /// Updates the form and sends the data to next form.
        /// </summary>
        /// <param name="model">The Email Template model.</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult Index(ContactAdviserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = mapper.Map(model, sessionStorage.Get());

                sessionStorage.Save(mappedModel);

                return Redirect($"{NextPageUrl}");
            }

            model.Title = Title;
            return View("Index", model);
        }

        #endregion Actions
    }
}