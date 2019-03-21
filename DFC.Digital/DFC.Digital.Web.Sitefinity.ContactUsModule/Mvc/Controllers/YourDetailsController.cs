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

        [DisplayName("Non Adviser Introduction Text")]
        public string NonAdviserIntroduction { get; set; } = "We need your details so that one of our advisers can contact you.";

        [DisplayName("Adviser Introduction Text")]
        public string AdviserIntroduction { get; set; } = "We need your details so that one of our advisers can contact you.";

        [DisplayName("Adviser Introduction Second Text")]
        public string AdviserIntroductionTwo { get; set; } = "Our advisers will use your date of birth and postcode to give you information that's relevant to you, for example on courses and funding.";

        [DisplayName("Failure Page URL")]
        public string FailurePageUrl { get; set; } = "/error/500";

        [DisplayName("Success Page Url")]
        public string SuccessPageUrl { get; set; } = "/contactus/thank-you";

        [DisplayName("Contact Option (ContactAdvisor, Technical, Feedback)")]
        public ContactOption ContactOption { get; set; } = ContactOption.ContactAdviser;

        [DisplayName("Template Url Name  in Configurations e.g contact-an-advisor")]
        public string TemplateName { get; set; } = "ContactAdviser";

        [DisplayName("Date Of Birth hint")]
        public string DateOfBirthHint { get; set; } = "For example, 31 3 1980";

        [DisplayName("Post coode hint")]
        public string PostcodeHint { get; set; } = "For example, SW1A 1AA";

        [DisplayName("Terms and Conditions Header Text")]
        public string TermsAndConditionsText { get; set; } = "Terms and Conditions";

        [DisplayName("Do you want us to contact you text")]
        public string DoYouWantUsToContactUsText { get; set; } = "Do you want us to contact you?";

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
                    PageIntroduction = AdviserIntroduction,
                    PageIntroductionTwo = AdviserIntroductionTwo,
                    PostcodeHint = PostcodeHint,
                    DateOfBirthHint = DateOfBirthHint,
                    TermsAndConditionsText = TermsAndConditionsText
                };
                return View("ContactAdvisor", viewModel);
            }
            else
            {
                var viewModel = new ContactUsWithConsentViewModel
                {
                    PageTitle = PageTitle,
                    PageIntroduction = NonAdviserIntroduction,
                    DoYouWantUsToContactUsText = DoYouWantUsToContactUsText,
                    TermsAndConditionsText = TermsAndConditionsText
                };
                return View("Feedback", viewModel);
            }
        }

        [HttpPost]
        public ActionResult SubmitDetails(ContactUsWithDobPostcodeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var data = sessionStorage.Get() ?? new ContactUs();
                var result = asyncHelper.Synchronise(() => sendEmailService.SendEmailAsync(new ContactUsRequest
                {
                    FirstName = viewModel.Firstname,
                    Email = viewModel.EmailAddress,
                    TemplateName = TemplateName,
                    LastName = viewModel.Lastname,
                    Message = data.GeneralFeedback?.Feedback,
                    TermsAndConditions = viewModel.AcceptTermsAndConditions,
                    PostCode = viewModel.Postcode,
                    ContactOption = data.ContactUsOption?.ContactOptionType.ToString(),
                    ContactAdviserQuestionType = data.ContactAnAdviserFeedback?.ContactAdviserQuestionType.ToString(),
                    DateOfBirth = viewModel.DateOfBirth
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

            viewModel.PageTitle = PageTitle;
            viewModel.PageIntroduction = AdviserIntroduction;
            viewModel.PageIntroductionTwo = AdviserIntroductionTwo;
            viewModel.DateOfBirthHint = DateOfBirthHint;
            viewModel.PostcodeHint = PostcodeHint;
            viewModel.TermsAndConditionsText = TermsAndConditionsText;

            return View("ContactAdvisor", viewModel);
        }

        [HttpPost]
        public ActionResult Submit(ContactUsWithConsentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var data = sessionStorage.Get() ?? new ContactUs();
                var result = asyncHelper.Synchronise(() => sendEmailService.SendEmailAsync(new ContactUsRequest
                {
                    FirstName = viewModel.Firstname,
                    Email = viewModel.EmailAddress,
                    TemplateName = TemplateName,
                    LastName = viewModel.Lastname,
                    Message = ContactOption == ContactOption.Feedback ? data.GeneralFeedback?.Feedback : data.TechnicalFeedback?.Message,
                    IsContactable = viewModel.IsContactable,
                    ContactOption = data.ContactUsOption?.ContactOptionType.ToString(),
                    FeedbackQuestionType = data.GeneralFeedback?.FeedbackQuestionType.ToString()
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

            viewModel.PageTitle = PageTitle;
            viewModel.PageIntroduction = NonAdviserIntroduction;
            viewModel.DoYouWantUsToContactUsText = DoYouWantUsToContactUsText;
            viewModel.TermsAndConditionsText = TermsAndConditionsText;
            return View(viewModel);
        }

        #endregion Actions
    }
}