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
    /// Custom Widget for Technical form
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "Technical", Title = "Technical", SectionName = SitefinityConstants.ContactUsSection)]
    public class TechnicalController : BaseDfcController
    {
        #region Constructors

        public TechnicalController(IApplicationLogger applicationLogger) : base(applicationLogger)
        {
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Next Page URL")]
        public string NextPageUrl { get; set; } = "technical-feedback";

        [DisplayName("Page Title")]
        public string Title { get; set; } = "Report a technical issue";

        [DisplayName("Page Introduction Text")]
        public string PageIntroduction { get; set; } = "Give us as much detail as possible. For example, the web browser you were using when you had a problem, what you were trying to do, what happened and any error messages that were on the screen.";

        [DisplayName("Personal Information Text")]
        public string PersonalInformation { get; set; } = "Do not include any personal or sign in information.";

        [DisplayName("Character Limit")]
        public string CharacterLimit { get; set; } = "Character limit is 1000.";

        [DisplayName("Message Label")]
        public string MessageLabel { get; set; } = "Include links to the problem page and any page headings. This will help us to fix the issue more quickly.";


        #endregion Public Properties

        #region Actions

        /// <summary>
        /// entry point to the widget to show technical feedback form.
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new ContactUsTechnicalViewModel();
            return View("Index", AddWidgetPropertyFields(model));
        }

        [HttpPost]
        public ActionResult Index(ContactUsTechnicalViewModel model)
        {
            if (ModelState.IsValid)
            {
                //return Redirect("/feedback_your_details");
                return RedirectToAction("Index", "YourDetailsController");
            }

            //Put the non bound data fields back
            return View("Index", AddWidgetPropertyFields(model));

            #endregion Actions
        }

        private ContactUsTechnicalViewModel AddWidgetPropertyFields(ContactUsTechnicalViewModel model)
        {
            model.ContactOption = ContactOption.Technical;
            model.NextPageUrl = this.NextPageUrl;
            model.Title = this.Title;
            model.MessageLabel = this.MessageLabel;
            model.PageIntroduction = this.PageIntroduction;
            model.PersonalInformation = this.PersonalInformation;
            model.CharacterLimit = this.CharacterLimit;
            return model;
        }
    }
}