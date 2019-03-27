using System.ComponentModel;
using System.Web.Mvc;
using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using DFC.Digital.Web.Sitefinity.Core;
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
        private readonly IMapper mapper;
        private readonly IWebAppContext context;
        private readonly ISessionStorage<ContactUs> sessionStorage;

        #region Constructors

        public TechnicalController(
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

        [DisplayName("Next Page URL")]
        public string NextPageUrl { get; set; } = "/contact-us/your-details/";

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

        [DisplayName("Relative page url to select option page")]
        public string ContactOptionPageUrl { get; set; } = "/contact-us/select-option/";

        [DisplayName("Continue Button Text")]
        public string ContinueText { get; set; } = "Continue";

        #endregion Public Properties

        #region Actions

        /// <summary>
        /// entry point to the widget to show technical feedback form.
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (!context.IsContentAuthoringSite)
            {
                var sessionModel = sessionStorage.Get() ?? new ContactUs();
                if (sessionModel.ContactUsOption == null)
                {
                    return Redirect(ContactOptionPageUrl);
                }
            }

            var model = new TechnicalFeedbackViewModel();
            return View("Index", AddWidgetPropertyFields(model));
        }

        [HttpPost]
        public ActionResult Index(TechnicalFeedbackViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = mapper.Map(model, sessionStorage.Get());
                sessionStorage.Save(mappedModel);

                return Redirect(NextPageUrl);
            }

            model.Title = Title;

            //Put the non bound data fields back
            return View("Index", AddWidgetPropertyFields(model));
        }
        #endregion Actions

        private TechnicalFeedbackViewModel AddWidgetPropertyFields(TechnicalFeedbackViewModel model)
        {
            model.NextPageUrl = NextPageUrl;
            model.Title = Title;
            model.MessageLabel = MessageLabel;
            model.PageIntroduction = PageIntroduction;
            model.PersonalInformation = PersonalInformation;
            model.CharacterLimit = CharacterLimit;
            model.ContinueText = ContinueText;
            return model;
        }
    }
}