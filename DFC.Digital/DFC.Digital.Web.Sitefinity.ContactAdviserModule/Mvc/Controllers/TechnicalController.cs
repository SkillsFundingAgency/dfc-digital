using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System.Web.Mvc;
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
        #region Private Fields
        private IEmailTemplateRepository emailTemplateRepository;
        private ISitefinityCurrentContext sitefinityCurrentContext;

        #endregion Private Fields

        #region Constructors

        public TechnicalController(ISitefinityCurrentContext sitefinityCurrentContext, IApplicationLogger applicationLogger) : base(applicationLogger)
        {
            this.sitefinityCurrentContext = sitefinityCurrentContext;
        }

        #endregion Constructors

        #region Actions

        /// <summary>
        /// entry point to the widget to show technical feedback form.
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            return View("Index");
        }
        #endregion Actions
    }
}