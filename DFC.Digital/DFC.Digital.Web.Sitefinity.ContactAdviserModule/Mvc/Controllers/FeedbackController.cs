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
    /// Custom Widget for Feedback form
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "Feedback", Title = "Feedback", SectionName = SitefinityConstants.ContactUsSection)]
    public class FeedbackController : BaseDfcController
    {
        #region Private Fields
        private IEmailTemplateRepository emailTemplateRepository;
        private ISitefinityCurrentContext sitefinityCurrentContext;

        #endregion Private Fields

        #region Constructors

        public FeedbackController(IEmailTemplateRepository emailTemplateRepository, ISitefinityCurrentContext sitefinityCurrentContext, IApplicationLogger applicationLogger) : base(applicationLogger)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.sitefinityCurrentContext = sitefinityCurrentContext;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Next Page URL")]
        public string NextPageUrl { get; set; } = "/contact-us/your-details/";

        [DisplayName("Page Title")]
        public string Title { get; set; } = " What is your feedback about?";


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
                NextPageUrl = NextPageUrl,
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
            return View("Index", model);
        }


        #endregion Actions
    }
}