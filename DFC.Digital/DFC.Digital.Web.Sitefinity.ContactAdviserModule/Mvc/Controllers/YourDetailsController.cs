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
            return View(model);
        }

        [HttpPost]
        public ActionResult Sumbit(ContactUsViewModel viewModel)
        {
            var result = asyncHelper.Synchronise(() => sendEmailService.SendEmailAsync(new ContactUsRequest
            {
                FirstName = viewModel.FirstName,
                Email = viewModel.Email,
                TemplateName = viewModel.ContactOption.ToString(),
                LastName = viewModel.LastName,
                Message = viewModel.Message
            }));

            return View("SendResult", viewModel);
        }
        #endregion Actions
    }
}
