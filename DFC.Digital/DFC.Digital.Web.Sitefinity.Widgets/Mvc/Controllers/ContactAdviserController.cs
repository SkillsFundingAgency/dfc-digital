using DFC.Digital.Core;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for ContactAdviser forms
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "ContactAdviser", Title = "Contact an adviser", SectionName = SitefinityConstants.CustomWidgetSection)]

    public class ContactAdviserController : BaseDfcController
    {
        #region Private Fields
        private ISitefinityCurrentContext sitefinityCurrentContext;

        #endregion Private Fields

        #region Constructors
        public ContactAdviserController(ISitefinityCurrentContext sitefinityCurrentContext, IApplicationLogger applicationLogger) : base(applicationLogger)
        {
            this.sitefinityCurrentContext = sitefinityCurrentContext;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Your Details Title Text")]
        public string YourDetailsTitleText { get; set; } = "Your details";

        [DisplayName("Contact Adviser SendTo Email Address")]
        public string ContactAdvisorSendToEmailAddress { get; set; }

        [DisplayName("Feedback SendTo Email Address")]
        public string FeedbackSendToEmailAddress { get; set; }

        [DisplayName("Technical Support SendTo Email Address")]
        public string TechnicalSupportSendToEmailAddress { get; set; }

        #endregion Public Properties

        #region Actions

        // GET: DfcBreadcrumb

        /// <summary>
        /// entry point to the widget to show a contact form.
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index(ContactAdviserViewModel model)
        {
            return View();
        }

        #endregion Actions
    }
}