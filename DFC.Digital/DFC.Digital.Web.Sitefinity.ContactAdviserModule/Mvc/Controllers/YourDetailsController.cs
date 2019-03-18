using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using DFC.Digital.Web.Sitefinity.Core;
using System;
using System.ComponentModel;
using System.Globalization;
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
        private readonly ISessionStorage<ContactUsViewModel> sessionStorage;

        #endregion Private Fields

        #region Constructors

        public YourDetailsController(
            IApplicationLogger applicationLogger,
            INoncitizenEmailService<ContactUsRequest> sendEmailService,
            IAsyncHelper asyncHelper,
            IMapper mapper,
            ISessionStorage<ContactUsViewModel> sessionStorage) : base(applicationLogger)
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

        [DisplayName("Failure to Send Message")]
        public string FailureMessage { get; set; } = "Unfortunately we encountered a problem. We'll get back to you as soon as possible.";

        [DisplayName("Success Message on details submission")]
        public string SuccessMessage { get; set; } = "We'll get back to you as soon as possible.";

        #endregion Properties

        #region Actions

        [HttpGet]
        public ActionResult Index(ContactOption contactOption = ContactOption.Feedback)
        {
            var data = sessionStorage.Get();
            var model = data ?? new ContactUsViewModel
            {
                ContactOption = contactOption,
                PageIntroduction = PageIntroduction,
                PageTitle = PageTitle,
                PageIntroductionTwo = PageIntroductionTwo
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ContactUsViewModel viewModel)
        {
            var dateOfBirth = DoCustomValidation(viewModel);

            if (ModelState.IsValid)
            {
                var result = asyncHelper.Synchronise(() => sendEmailService.SendEmailAsync(new ContactUsRequest
                {
                    FirstName = viewModel.FirstName,
                    Email = viewModel.Email,
                    TemplateName = viewModel.ContactOption.ToString(),
                    LastName = viewModel.LastName,
                    Message = viewModel.Message,
                    TermsAndConditions = viewModel.TermsAndConditions,
                    PostCode = viewModel.PostCode,
                    Subject = viewModel.ContactOption.ToString(),
                    ContactOption = viewModel.ContactOption.ToString(),
                    ContactAdviserQuestionType = viewModel.ContactAdivserQuestionType.ToString(),
                    DateOfBirth = dateOfBirth,
                    IsContactable = viewModel.IsContactable,
                    FeedbackQuestionType = viewModel.FeedbackQuestionType.ToString()
                }));

                return View("ThankYou", new ContactUsResultViewModel { Message = result ? SuccessMessage : FailureMessage });
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(ContactUsTechnicalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return View("ThankYou", new ContactUsResultViewModel { Message = SuccessMessage });

              //return View("ThankYouPage", new ContactUsResultViewModel { Success = true });
            }

            return View("Index", viewModel);
        }

        private DateTime DoCustomValidation(ContactUsViewModel viewModel)
        {
            if (viewModel.ContactOption == ContactOption.ContactAdviser)
            {
                var dateOfBirthDay = viewModel.DobDay;
                var dateOfBirthMonth = viewModel.DobMonth;
                var dateOfBirthYear = viewModel.DobYear;

                var enGb = new CultureInfo("en-GB");
                var dob = string.Empty;
                if (!string.IsNullOrEmpty(dateOfBirthDay) && !string.IsNullOrEmpty(dateOfBirthMonth) &&
                    !string.IsNullOrEmpty(dateOfBirthYear))
                {
                    dob =
                        $"{dateOfBirthDay.PadLeft(2, '0')}/{dateOfBirthMonth.PadLeft(2, '0')}/{dateOfBirthYear.PadLeft(4, '0')}";
                }

                if (DateTime.TryParseExact(dob, "dd/MM/yyyy", enGb, DateTimeStyles.AdjustToUniversal, out var dateOfBirth))
                {
                    if (DateTime.Now.Year - dateOfBirth.Year < 13)
                    {
                        ModelState.AddModelError(nameof(ContactUsViewModel.DateOfBirth), "You must 13 years or over");
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(ContactUsViewModel.DateOfBirth), "Please enter a valid date");
                }

                return dateOfBirth;
            }
            else
            {
                ModelState.Remove(nameof(ContactUsViewModel.PostCode));

                return default(DateTime);
            }
        }

        #endregion Actions
    }
}
