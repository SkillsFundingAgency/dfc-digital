using System;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
using DFC.Digital.Web.Sitefinity.Core;
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
        private readonly ISendEmailService<ContactUsRequest> sendEmailService;
        private readonly IAsyncHelper asyncHelper;

        #endregion Private Fields

        #region Constructors

        public YourDetailsController(IApplicationLogger applicationLogger, ISendEmailService<ContactUsRequest> sendEmailService, IAsyncHelper asyncHelper) : base(applicationLogger)
        {
            this.sendEmailService = sendEmailService;
            this.asyncHelper = asyncHelper;
        }
        #endregion Constructors

        #region Properties
        public string FailureMessage { get; set; } = "Unfortunately we encountered a problem. We'll get back to you as soon as possible.";

        public string SuccessMessage { get; set; } = "We'll get back to you as soon as possible.";
        #endregion Properties

        #region Actions

        /// <summary>
        /// Updates the form and sends the data to next form.
        /// </summary>
        /// <param name="model">The Email Template model.</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return YourDetails(new ContactUsViewModel());
        }

        /// <summary>
        /// Updates the form and sends the data to next form.
        /// </summary>
        /// <param name="model">The Email Template model.</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult YourDetails(ContactUsViewModel model)
        {
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(ContactUsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = asyncHelper.Synchronise(() => sendEmailService.SendEmailAsync(new ContactUsRequest
                {
                    FirstName = viewModel.FirstName,
                    Email = viewModel.Email,
                    TemplateName = viewModel.ContactOption.ToString(),
                    LastName = viewModel.LastName,
                    Message = viewModel.Message,
                    TermsAndConditions = viewModel.TandCChecked,
                    PostCode = viewModel.PostCode,
                    Subject = viewModel.ContactOption.ToString(),
                    ContactOption = viewModel.ContactOption.ToString(),
                    ContactAdviserQuestionType = viewModel.ContactAdivserQuestionType.ToString(),
                    DateOfBirth = new DateTime(viewModel.DOBYear, viewModel.DOBMonth, viewModel.DOBDay),
                    IsContactable = viewModel.IsContactable,
                    FeedbackQuestionType = viewModel.FeedbackQuestionType.ToString()
                }));

                return View("ThankYouPage", new ContactUsResultViewModel { Success = result.Success, Message = result.Success ? SuccessMessage : FailureMessage});
            }

            return View("Index", viewModel);
        }

        #endregion Actions
    }
}
