using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using DFC.Digital.Web.Sitefinity.Core;
using System.ComponentModel;
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
        private readonly ISessionStorage<ContactUsViewModel> sessionStorage;

        #endregion Private Fields

        #region Constructors

        public SelectOptionController(
            IEmailTemplateRepository emailTemplateRepository,
            ISitefinityCurrentContext sitefinityCurrentContext,
            IApplicationLogger applicationLogger,
            ISessionStorage<ContactUsViewModel> sessionStorage) : base(applicationLogger)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.sitefinityCurrentContext = sitefinityCurrentContext;
            this.sessionStorage = sessionStorage;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Page Title")]
        public string Title { get; set; } = "Why would you like to contact us?";

        [DisplayName("Relative page url to general feedback")]
        public string GeneralFeedbackPage { get; set; } = "/contact-us/feedback/";

        [DisplayName("Relative page url to technical feedback")]
        public string TechnicalFeedbackPage { get; set; } = "/contact-us/technical/";

        [DisplayName("Relative page url to contact an adviser")]
        public string ContactAdviserPage { get; set; } = "/contact-us/contact-adviser/";

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
            var model = new ContactUsViewModel
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
        public ActionResult Index(ContactUsViewModel model)
        {
            if (ModelState.IsValid)
            {
                sessionStorage.Save(model);
                switch (model.SelectOption.ContactOptionType)
                {
                    case ContactOption.Technical:
                        return Redirect(TechnicalFeedbackPage);
                    case ContactOption.ContactAdviser:
                        return Redirect(ContactAdviserPage);
                    case ContactOption.Feedback:
                        return Redirect(GeneralFeedbackPage);
                    default:
                        return View("Index", model);
                }
            }

            return View("Index", model);
        }


        #endregion Actions
    }
}