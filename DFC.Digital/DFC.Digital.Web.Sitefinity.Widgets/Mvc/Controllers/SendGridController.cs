using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using Newtonsoft.Json;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "SendGrid", Title = "Send Grid Test Harness", SectionName = SitefinityConstants.CustomAdminWidgetSection)]
    public class SendGridController : BaseDfcController
    {
        private readonly INoncitizenEmailService<ContactUsRequest> sendEmailService;
        private readonly IAsyncHelper asyncHelper;

        public SendGridController(IApplicationLogger applicationLogger, INoncitizenEmailService<ContactUsRequest> sendEmailService, IAsyncHelper asyncHelper) : base(applicationLogger)
        {
            this.sendEmailService = sendEmailService;
            this.asyncHelper = asyncHelper;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new SendGridViewModel());
        }

        [HttpPost]
        public ActionResult Index(SendGridViewModel viewModel)
        {
            var result = asyncHelper.Synchronise(() => sendEmailService.SendEmailAsync(new ContactUsRequest
            {
                FirstName = viewModel.FirstName,
                Email = viewModel.EmailAddress,
                TemplateName = viewModel.TemplateName,
                LastName = viewModel.LastName,
                ContactOption = viewModel.ContactOption,
                ContactAdviserQuestionType = viewModel.ContactAdviserQuestionType,
                DateOfBirth = viewModel.DateOfBirth,
                FeedbackQuestionType = viewModel.FeedbackQuestionType,
                Message = viewModel.Message,
                IsContactable = viewModel.IsContactable,
                Postcode = viewModel.Postcode,
                TermsAndConditions = viewModel.AcceptTermsAndConditions
            }));

            viewModel.SendResult = JsonConvert.SerializeObject(result);

            return View("SendResult", viewModel);
        }
    }
}