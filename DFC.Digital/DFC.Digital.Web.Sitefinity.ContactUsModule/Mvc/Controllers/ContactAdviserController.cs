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

        private readonly IMapper mapper;
        private readonly IWebAppContext context;
        private readonly ISessionStorage<ContactUs> sessionStorage;

        #endregion Private Fields

        #region Constructors

        public ContactAdviserController(
            IApplicationLogger applicationLogger,
            IMapper mapper,
            IWebAppContext context,
            ISessionStorage<ContactUs> sessionStorage) : base(applicationLogger)
        {
            this.mapper = mapper;
            this.context = context;
            this.sessionStorage = sessionStorage;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Personal Information Hint Text")]
        public string PersonalInformation { get; set; } = "Do not include any personal or sign in information.";

        [DisplayName("Next Page URL")]
        public string NextPage { get; set; } = "/contact-us/your-details-adviser/";

        [DisplayName("Page Title")]
        public string Title { get; set; } = "What is your query about?";

        [DisplayName("Relative page URL to select option page")]
        public string ContactOptionPage { get; set; } = "/contact-us/select-option/";

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
            if (!context.IsContentAuthoringSite)
            {
                var sessionModel = sessionStorage.Get();
                if (sessionModel is null || sessionModel.ContactUsOption?.ContactOptionType != ContactOption.ContactAdviser)
                {
                    return Redirect(ContactOptionPage);
                }
            }

            var model = new ContactAdviserViewModel();
            AddWidgetProperties(model);

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

                return Redirect($"{NextPage}");
            }

            AddWidgetProperties(model);
            return View("Index", model);
        }

        #endregion Actions
        private void AddWidgetProperties(ContactAdviserViewModel model)
        {
            model.Title = Title;
            model.Hint = PersonalInformation;
            model.NextPage = NextPage;
        }
    }
}
