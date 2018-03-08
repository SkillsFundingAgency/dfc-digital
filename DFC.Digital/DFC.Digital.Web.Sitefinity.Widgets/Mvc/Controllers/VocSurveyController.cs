using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core.Base;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for VOC Survey email Capture
    /// </summary>
    /// <seealso cref="BaseDfcController" />
    [ControllerToolboxItem(Name = "Vocsurvey", Title = "VOC Survey", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class VocSurveyController : BaseDfcController
    {
        #region Private Fields

        private IGovUkNotify govUkNotifyService;
        private IWebAppContext webAppContext;

        #endregion Private Fields

        #region Constructors

        public VocSurveyController(IGovUkNotify govUkNotify, IWebAppContext webAppContext, IApplicationLogger applicationLogger) : base(applicationLogger)
        {
            govUkNotifyService = govUkNotify;
            this.webAppContext = webAppContext;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Instructions for no email address")]
        public string DontHaveEmailText { get; set; } = "Don't have an email address? Leave your feedback online instead.";

        [DisplayName("Age limit text")]
        public string AgeLimitText { get; set; } = "(you need to be 16 or older to be eligible for this survey)";

        [DisplayName("Email submitted successfully text")]
        public string EmailSentText { get; set; } = "Thanks, we’ve sent you an email with a link to the survey.";

        [DisplayName("Email not submitted successfully text")]
        public string EmailNotSentText { get; set; } = "Unfortunately, we’ve not been able to send you an email with a link to the survey.";

        [DisplayName("Form Intro Text")]
        public string FormIntroText { get; set; } = "We'll send you a link to a feedback form. It only takes 2 minutes to fill in. Don't worry we won't send you spam or share your email address with anyone.";

        [DisplayName("Online Survey link")]
        public string OnlineSurveyLink { get; set; } = "https://beta.nationalcareersservice.org.uk/feedback-survey";

        #endregion Public Properties

        #region Actions

        [HttpGet]
        [RelativeRoute("")]
        [RelativeRoute("{urlname}")]
        public ActionResult Index()
        {
            return ReturnSurveyViewModel();
        }

        /// <summary>
        /// This will be called when the citizen has not got a js enabled.
        /// </summary>
        /// <param name="vocSurveyViewModel">voice of customer survey model</param>
        /// <returns>Success or failure message to screen</returns>
        [HttpPost]
        [RelativeRoute("")]
        public ActionResult Index(VocSurveyViewModel vocSurveyViewModel)
        {
            if (!string.IsNullOrEmpty(vocSurveyViewModel.EmailAddress))
            {
                bool response = SendByNotifyService(vocSurveyViewModel.EmailAddress);
                var resultViewModel = new EmailSubmissionViewModel
                {
                    ResponseMessage = response ? EmailSentText : EmailNotSentText
                };

                return View("Response", resultViewModel);
            }

            return View("Index", new VocSurveyViewModel());
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns> jsonresult response</returns>
        [HttpPost]
        public ActionResult SendEmail(string emailAddress)
        {
            bool response = SendByNotifyService(emailAddress);
            return new JsonResult { Data = response };
        }

        #endregion Actions

        private ActionResult ReturnSurveyViewModel()
        {
            return View("Index", new VocSurveyViewModel
            {
                DontHaveEmailText = DontHaveEmailText,
                AgeLimitText = AgeLimitText,
                EmailNotSentText = EmailNotSentText,
                EmailSentText = EmailSentText,
                FormIntroText = FormIntroText,
                OnlineSurveyLink = OnlineSurveyLink
            });
        }

        private bool SendByNotifyService(string emailAddress)
        {
            var vocSurveyPersonalisation = webAppContext.GetVocCookie(Constants.VocPersonalisationCookieName);

            var response = govUkNotifyService.SubmitEmail(emailAddress, vocSurveyPersonalisation);
            return response;
        }
    }
}