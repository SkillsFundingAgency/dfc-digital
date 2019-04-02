using AutoMapper;
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
    /// Custom Widget for Feedback form
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "Feedback", Title = "Feedback", SectionName = SitefinityConstants.ContactUsSection)]
    public class FeedbackController : BaseDfcController
    {
        #region Private Fields

        private readonly IMapper mapper;
        private readonly IWebAppContext context;
        private readonly ISessionStorage<ContactUs> sessionStorage;
        private IEmailTemplateRepository emailTemplateRepository;
        private ISitefinityCurrentContext sitefinityCurrentContext;

        #endregion Private Fields

        #region Constructors

        public FeedbackController(
            IEmailTemplateRepository emailTemplateRepository,
            ISitefinityCurrentContext sitefinityCurrentContext,
            IApplicationLogger applicationLogger,
            IMapper mapper,
            IWebAppContext context,
            ISessionStorage<ContactUs> sessionStorage) : base(applicationLogger)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.sitefinityCurrentContext = sitefinityCurrentContext;
            this.mapper = mapper;
            this.context = context;
            this.sessionStorage = sessionStorage;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Next Page URL")]
        public string NextPageUrl { get; set; } = "/contact-us/your-details/";

        [DisplayName("Page Title")]
        public string Title { get; set; } = " What is your feedback about?";

        [DisplayName("Personal Information Text")]
        public string PersonalInformation { get; set; } = "Do not include any personal or sign in information.";

        [DisplayName("Character Limit")]
        public string CharacterLimit { get; set; } = "Character limit is 1000.";

        [DisplayName("Relative page url to select option page")]
        public string ContactOptionPage { get; set; } = "/contact-us/select-option/";

        [DisplayName("Continue Button Text")]
        public string ContinueText { get; set; } = "Continue";

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
                var sessionModel = sessionStorage.Get() ?? new ContactUs();
                if (sessionModel.ContactUsOption == null)
                {
                    return Redirect(ContactOptionPage);
                }
            }

            var model = new GeneralFeedbackViewModel();
            return View("Index", AddWidgetPropertyFields(model));
        }

        /// <summary>
        /// Updates the form and sends the data to next form.
        /// </summary>
        /// <param name="model">The Email Template model.</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult Index(GeneralFeedbackViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = mapper.Map(model, sessionStorage.Get());

                sessionStorage.Save(mappedModel);

                return Redirect($"{NextPageUrl}");
            }

            return View("Index", AddWidgetPropertyFields(model));
        }

        #endregion Actions

        private GeneralFeedbackViewModel AddWidgetPropertyFields(GeneralFeedbackViewModel model)
        {
            model.NextPage = this.NextPageUrl;
            model.Title = this.Title;
            model.PersonalInformation = this.PersonalInformation;
            model.CharacterLimit = this.CharacterLimit;
            model.ContinueText = ContinueText;
            return model;
        }
    }
}