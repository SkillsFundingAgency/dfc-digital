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

        private IEmailTemplateRepository emailTemplateRepository;
        private ISitefinityCurrentContext sitefinityCurrentContext;
        private readonly IMapper mapper;
        private readonly ISessionStorage<ContactUsViewModel> sessionStorage;

        #endregion Private Fields

        #region Properties

        [DisplayName("Next page")]
        public string NextPage { get; set; } = "/contact-us/your-details/";

        #endregion Properties

        #region Constructors

        public FeedbackController(
            IEmailTemplateRepository emailTemplateRepository,
            ISitefinityCurrentContext sitefinityCurrentContext,
            IApplicationLogger applicationLogger,
            IMapper mapper,
            ISessionStorage<ContactUsViewModel> sessionStorage) : base(applicationLogger)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.sitefinityCurrentContext = sitefinityCurrentContext;
            this.mapper = mapper;
            this.sessionStorage = sessionStorage;
        }

        #endregion Constructors

        #region Actions

        // GET: ContactAdviser

        /// <summary>
        /// entry point to the widget to show contact adviser form.
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// Updates the form and sends the data to next form.
        /// </summary>
        /// <param name="model">The Email Template model.</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult Index(ContactUsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedModel = mapper.Map(model, sessionStorage.Get());
                sessionStorage.Save(mappedModel);

                return Redirect($"{NextPage}?contactOption={model.ContactOption}");
            }

            return View("Index", model);
        }

        #endregion Actions
    }
}