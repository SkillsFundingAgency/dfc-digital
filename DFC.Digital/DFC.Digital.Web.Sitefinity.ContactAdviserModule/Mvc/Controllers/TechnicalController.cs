using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.ContactAdviserModule.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for Technical form
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "Technical", Title = "Technical", SectionName = SitefinityConstants.ContactAdviserSection)]
    public class TechnicalController : BaseDfcController
    {
        #region Private Fields
        private IEmailTemplateRepository emailTemplateRepository;
        private ISitefinityCurrentContext sitefinityCurrentContext;

        #endregion Private Fields

        #region Constructors

        public TechnicalController(IEmailTemplateRepository emailTemplateRepository, ISitefinityCurrentContext sitefinityCurrentContext, IApplicationLogger applicationLogger) : base(applicationLogger)
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
        [RelativeRoute("")]
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// contact adviser on Index of the specified urlname.
        /// </summary>
        /// <param name="urlName">The urlname.</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [RelativeRoute("{urlname}")]
        public ActionResult Index(string urlName)
        {
            return View("Index");
        }

        /// <summary>
        /// Updates the form and sends the data to next form.
        /// </summary>
        /// <param name="model">The Email Template model.</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [RelativeRoute("{urlname}")]
        public ActionResult Index(EmailTemplate model)
        {
            return View("Index", model);
        }

        /// <summary>
        /// Updates the form and sends the data to next form.
        /// </summary>
        /// <param name="model">The Email Template model.</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [RelativeRoute("{urlname}")]
        public ActionResult Adviser(EmailTemplate model)
        {
            return View("Adviser", model);
        }

        /// <summary>
        /// Updates the form and sends the data to next form.
        /// </summary>
        /// <param name="model">The Email Template model.</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [RelativeRoute("{urlname}")]
        public ActionResult Feedback(EmailTemplate model)
        {
            return View("Feedback", model);
        }

        /// <summary>
        /// Updates the form and sends the data to next form.
        /// </summary>
        /// <param name="model">The Email Template model.</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [RelativeRoute("{urlname}")]
        public ActionResult Technical(EmailTemplate model)
        {
            return View("Technical", model);
        }

        /// <summary>
        /// Updates the form and sends the data to next form.
        /// </summary>
        /// <param name="model">The Email Template model.</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [RelativeRoute("{urlname}")]
        public ActionResult YourDetails(EmailTemplate model)
        {
            return View("YourDetails", model);
        }

        #endregion Actions
    }
}