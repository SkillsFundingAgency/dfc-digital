using AutoMapper;
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
    /// Custom Widget for YourDetails form
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "YourDetails", Title = "Your Details", SectionName = SitefinityConstants.ContactUsSection)]
    public class YourDetailsController : BaseDfcController
    {
        #region Private Fields
        private readonly INoncitizenEmailService<ContactUsRequest> sendEmailService;
        private readonly IAsyncHelper asyncHelper;
        private readonly IMapper mapper;
        private readonly ISessionStorage<ContactUs> sessionStorage;

        #endregion Private Fields

        #region Constructors

        public YourDetailsController(
            IApplicationLogger applicationLogger,
            INoncitizenEmailService<ContactUsRequest> sendEmailService,
            IAsyncHelper asyncHelper,
            IMapper mapper,
            ISessionStorage<ContactUs> sessionStorage) : base(applicationLogger)
        {
            this.sendEmailService = sendEmailService;
            this.asyncHelper = asyncHelper;
            this.mapper = mapper;
            this.sessionStorage = sessionStorage;
        }
        #endregion Constructors

        #region Properties
        [DisplayName("Page Title")]
        public string PageTitle { get; set; } = "Enter your details";

        [DisplayName("Page Introduction Text")]
        public string PageIntroduction { get; set; } = "We need your details so that one of our advisers can contact you.";

        [DisplayName("Page Introduction Second Text")]
        public string PageIntroductionTwo { get; set; } = "Our advisers will use your date of birth and postcode to give you information that's relevant to you, for example on courses and funding.";

        [DisplayName("Failure Page URL")]
        public string FailurePageUrl { get; set; } = "/error/500";

        [DisplayName("Success Page Url")]
        public string SuccessPageUrl { get; set; } = "/contactus/thank-you";

        [DisplayName("Contact Option (ContactAdvisor, Technical, Feedback)")]
        public ContactOption ContactOption { get; set; } = ContactOption.ContactAdviser;

        [DisplayName("Template for sending email to Serco")]
        public string TemplateName { get; set; } = "ContactAdviser";

        #endregion Properties

        #region Actions

        [HttpGet]
        public ActionResult Index()
        {
            if (ContactOption == ContactOption.ContactAdviser)
            {
                var viewModel = new ContactUsWithDobPostcodeViewModel
                {
                    PageTitle = PageTitle,
                    PageIntroduction = PageIntroduction,
                    PageIntroductionTwo = PageIntroductionTwo
                };
                return View("ContactAdvisor", viewModel);
            }
            else
            {
                var viewModel = new ContactUsWithConsentViewModel
                {
                    PageTitle = PageTitle,
                    PageIntroduction = PageIntroduction
                };
                return View("Feedback", viewModel);
            }
        }

        [HttpPost]
        public ActionResult SubmitDetails(ContactUsWithDobPostcodeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var data = sessionStorage.Get();
                var result = asyncHelper.Synchronise(() => sendEmailService.SendEmailAsync(new ContactUsRequest
                {
                    FirstName = viewModel.PersonalContactDetails.Firstname,
                    Email = viewModel.PersonalContactDetails.EmailAddress,
                    TemplateName = TemplateName,
                    LastName = viewModel.PersonalContactDetails.Lastname,
                    Message = data.GeneralFeedback.Feedback,
                    TermsAndConditions = viewModel.DateOfBirthPostcodeDetails.AcceptTermsAndConditions,
                    PostCode = viewModel.DateOfBirthPostcodeDetails.Postcode,
                    ContactOption = data.ContactUsOption.ContactOptionType.ToString(),
                    ContactAdviserQuestionType = data.ContactAnAdviserFeedback?.ContactAdviserQuestionType.ToString(),
                    DateOfBirth = viewModel.DateOfBirthPostcodeDetails.DateOfBirth,
                    FeedbackQuestionType = data.TechnicalFeedback.ToString()
                }));

                if (result)
                {
                    return Redirect(SuccessPageUrl);
                }
                else
                {
                    return Redirect(FailurePageUrl);
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Submit(ContactUsWithConsentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var data = sessionStorage.Get();
                var result = asyncHelper.Synchronise(() => sendEmailService.SendEmailAsync(new ContactUsRequest
                {
                    FirstName = viewModel.PersonalContactDetails.Firstname,
                    Email = viewModel.PersonalContactDetails.EmailAddress,
                    TemplateName = TemplateName,
                    LastName = viewModel.PersonalContactDetails.Lastname,
                    Message = data.GeneralFeedback.Feedback,
                    IsContactable = viewModel.ConsentDetails.IsContactable,
                    ContactOption = data.ContactUsOption.ContactOptionType.ToString(),
                    ContactAdviserQuestionType = data.ContactAnAdviserFeedback?.ContactAdviserQuestionType.ToString(),
                    FeedbackQuestionType = data.TechnicalFeedback.ToString()
                }));

                if (result)
                {
                    return Redirect(SuccessPageUrl);
                }
                else
                {
                    return Redirect(FailurePageUrl);
                }
            }

            return View(viewModel);
        }
        #endregion Actions
    }
}
